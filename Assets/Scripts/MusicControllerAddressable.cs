using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;

[RequireComponent(typeof(AudioSource))]
public class MusicControllerAddressable : MonoBehaviour
{
    [System.Serializable]
    public class MusicTrackRef
    {
        public AssetReferenceT<AudioClip> clipReference;
        public bool playInMenu = true;
        public bool playInLevels = true;

        [HideInInspector] public bool isReady = false; // Прошел ли трек фоновый "прогрев"
        [HideInInspector] public AudioClip loadedClip; // Распакованный звук в памяти
    }

    public static MusicControllerAddressable Instance { get; private set; }

    [Header("Стартовый трек (Вшит в билд)")]
    [Tooltip("Этот трек запустится мгновенно при старте игры")]
    public AudioClip firstTrackDirect;

    [Header("Настройки звука")]
    [Range(0f, 1f)] public float globalVolume = 0.5f;

    [Header("Кэширование Addressables")]
    [Tooltip("Сколько треков одновременно держать в оперативной памяти")]
    public int maxCachedTracks = 5;

    [Header("Список остальных треков")]
    public List<MusicTrackRef> tracks = new List<MusicTrackRef>();

    // 3 Плеера: 2 для бесшовной игры (пинг-понг) и 1 для фонового прогрева
    private AudioSource[] playbackSources = new AudioSource[2];
    private AudioSource warmupSource;

    private int activeSourceIndex = 0;
    private AudioSource ActiveSource => playbackSources[activeSourceIndex];
    private AudioSource InactiveSource => playbackSources[1 - activeSourceIndex];

    private string currentTrackGuid;
    private string targetTrackGuid;
    private bool isFirstTrackPlaying = false;
    private bool isPreloading = false;

    // Словари для управления памятью (LRU Cache)
    private Dictionary<string, AsyncOperationHandle<AudioClip>> trackHandles = new Dictionary<string, AsyncOperationHandle<AudioClip>>();
    private List<string> cacheHistory = new List<string>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Инициализация трёх AudioSource
            AudioSource[] existingSources = GetComponents<AudioSource>();
            while (existingSources.Length < 3)
            {
                gameObject.AddComponent<AudioSource>();
                existingSources = GetComponents<AudioSource>();
            }

            playbackSources[0] = existingSources[0];
            playbackSources[1] = existingSources[1];
            warmupSource = existingSources[2];

            // Настройка плееров для воспроизведения
            foreach (var src in playbackSources)
            {
                src.loop = true;
                src.volume = globalVolume;
                src.playOnAwake = false;
            }

            // Настройка скрытого плеера для прогрева
            warmupSource.loop = true;
            warmupSource.volume = 0f; // Всегда строгий ноль!
            warmupSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (Instance != this) return;

        // 1. Мгновенно запускаем стартовый трек
        if (firstTrackDirect != null)
        {
            ActiveSource.clip = firstTrackDirect;
            ActiveSource.Play();
            isFirstTrackPlaying = true;
        }

        // 2. Ждем и запускаем фоновый прогрев
        if (tracks.Count > 0)
        {
            StartCoroutine(WaitAndStartPreload());
        }
    }

    private IEnumerator WaitAndStartPreload()
    {
        // В WebGL ждем, пока звук РЕАЛЬНО пойдет в колонки (time > 0)
        while (ActiveSource.time <= 0 && ActiveSource.isPlaying)
        {
            yield return null;
        }

        // Даем секунду форы, чтобы звук стабилизировался
        yield return new WaitForSeconds(1.0f);

        if (!isPreloading) StartCoroutine(PreloadAllTracksRoutine());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading" || scene.name == "Editor Car") return;

        // Защита стартового трека от перезапуска при первом входе в меню
        if (scene.name == "Menu" && isFirstTrackPlaying)
        {
            isFirstTrackPlaying = false;
            return;
        }

        isFirstTrackPlaying = false;
        PlayRandomTrack(scene.name == "Menu");
    }

    private void PlayRandomTrack(bool isMenu)
    {
        var readyTracks = tracks.Where(t => t.isReady && (isMenu ? t.playInMenu : t.playInLevels)).ToList();

        if (readyTracks.Count > 0)
        {
            var nextTrack = readyTracks[Random.Range(0, readyTracks.Count)];
            string nextGuid = nextTrack.clipReference.AssetGUID;

            if (currentTrackGuid != nextGuid || !ActiveSource.isPlaying)
            {
                targetTrackGuid = nextGuid;
                StartCoroutine(SeamlessTransition(nextTrack.loadedClip, nextGuid));
            }
        }
        else
        {
            // ЖЕСТКИЙ ОТКАТ: Если бандлы не готовы, а текущий плеер ЗАМОЛЧАЛ (тишина на сцене)
            // мы экстренно спасаемся вшитым стартовым треком!
            //if (currentTrackGuid != "builtin_fallback")
            //{
                targetTrackGuid = "builtin_fallback";
                StartCoroutine(SeamlessTransition(firstTrackDirect, "builtin_fallback"));
            //}
        }
    }

    private IEnumerator SeamlessTransition(AudioClip newClip, string nextGuid)
    {
        // Защита: если пока мы готовились, игрок уже переключил сцену
        if (targetTrackGuid != nextGuid) yield break;

        AudioSource nextSource = InactiveSource;
        nextSource.clip = newClip;
        nextSource.volume = 0f; // Стартуем в тишине для плавности
        nextSource.Play();

        // Ждем фактического старта звука в WebGL
        while (nextSource.time <= 0 && nextSource.isPlaying)
        {
            if (targetTrackGuid != nextGuid)
            {
                nextSource.Stop();
                yield break;
            }
            yield return null;
        }

        // Как только звук пошел, выключаем старый плеер и даем громкость новому
        ActiveSource.Stop();
        nextSource.volume = globalVolume;

        activeSourceIndex = 1 - activeSourceIndex; // Меняем плеера местами
        currentTrackGuid = nextGuid;
        UpdateHistory(nextGuid);
    }

    private IEnumerator PreloadAllTracksRoutine()
    {
        isPreloading = true;
        foreach (var track in tracks)
        {
            string guid = track.clipReference.AssetGUID;
            if (track.isReady) continue; // Пропускаем уже готовые

            // 1. Проверяем кэш и удаляем самое старое, если памяти мало
            if (trackHandles.Count >= maxCachedTracks) ReleaseOldest();

            // 2. Скачиваем/Достаем из кэша бандл
            var handle = Addressables.LoadAssetAsync<AudioClip>(track.clipReference);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                track.loadedClip = handle.Result;
                trackHandles.Add(guid, handle);

                // 3. ПРОГРЕВ НА ТРЕТЬЕМ ПЛЕЕРЕ
                warmupSource.Stop();
                warmupSource.clip = track.loadedClip;
                warmupSource.Play();

                // Ждем пока трек физически проиграет ~1 секунду в тишине
                float startWarmupTime = Time.realtimeSinceStartup;
                while (warmupSource.time < 1.0f && (Time.realtimeSinceStartup - startWarmupTime < 4f))
                {
                    yield return null;
                }

                warmupSource.Stop();
                warmupSource.clip = null; // Очищаем ссылку

                track.isReady = true; // Теперь трек доступен для рандома!
                UpdateHistory(guid);
                Debug.Log($"[MusicManager] Трек {guid} скачан и прогрет в фоне!");
            }

            // Микро-пауза, чтобы не повесить браузер телефона постоянной распаковкой
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("[MusicManager] Все музыкальные бандлы успешно загружены в RAM!");
        isPreloading = false;
    }

    private void UpdateHistory(string guid)
    {
        if (guid == "builtin_fallback") return; // Вшитый трек игнорируем для памяти

        if (cacheHistory.Contains(guid)) cacheHistory.Remove(guid);
        cacheHistory.Add(guid); // Добавляем в конец как самый свежий
    }

    private void ReleaseOldest()
    {
        // Ищем старый трек, но не трогаем тот, что играет, тот что ожидается, и тот, что сейчас прогревается
        string toRelease = cacheHistory.FirstOrDefault(g => g != currentTrackGuid && g != targetTrackGuid);

        if (toRelease != null)
        {
            if (trackHandles.TryGetValue(toRelease, out var handle))
            {
                Addressables.Release(handle);
            }
            trackHandles.Remove(toRelease);
            cacheHistory.Remove(toRelease);

            var track = tracks.Find(tr => tr.clipReference.AssetGUID == toRelease);
            if (track != null)
            {
                track.isReady = false;
                track.loadedClip = null;
            }
        }
    }

    private void OnValidate()
    {
        // Синхронизируем громкость только для играющих плееров (прогревочный всегда 0)
        if (playbackSources != null)
        {
            foreach (var s in playbackSources) if (s != null) s.volume = globalVolume;
        }
    }

    private void OnEnable() { if (Instance == this) SceneManager.sceneLoaded += OnSceneLoaded; }

    private void OnDisable()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            ClearAll();
        }
    }

    private void ClearAll()
    {
        foreach (var handle in trackHandles.Values)
        {
            if (handle.IsValid()) Addressables.Release(handle);
        }
        trackHandles.Clear();
        cacheHistory.Clear();
    }
}