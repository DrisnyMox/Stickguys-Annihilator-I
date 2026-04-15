using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [System.Serializable]
    public class MusicTrack
    {
        public AudioClip clip;
        [Tooltip("Может ли играть в главном меню?")]
        public bool playInMenu = true;
        [Tooltip("Может ли играть на уровнях?")]
        public bool playInLevels = true;
    }

    public static MusicController Instance { get; private set; }

    [Header("Настройки звука")]
    [Range(0f, 1f)]
    public float globalVolume = 0.5f;

    [Header("Список треков")]
    public List<MusicTrack> tracks = new List<MusicTrack>();

    private AudioSource audioSource;
    private AudioClip currentClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = GetComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.volume = globalVolume;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnValidate()
    {
        if (audioSource != null)
        {
            audioSource.volume = globalVolume;
        }
    }

    private void OnEnable()
    {
        // ВАЖНО: Подписываемся на событие ТОЛЬКО если это оригинальный бессмертный объект.
        // Это убивает баг с дубликатами на телефонах.
        if (Instance == this)
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnDisable()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Loading" || scene.name == "Editor Car")
        {
            return;
        }

        bool isMenu = scene.name == "Menu";
        PlayRandomTrack(isMenu);
    }

    private void PlayRandomTrack(bool isMenu)
    {
        if (tracks.Count == 0) return;

        List<MusicTrack> validTracks = new List<MusicTrack>();

        foreach (var track in tracks)
        {
            if (isMenu && track.playInMenu)
                validTracks.Add(track);
            else if (!isMenu && track.playInLevels)
                validTracks.Add(track);
        }

        if (validTracks.Count > 0)
        {
            AudioClip nextClip = validTracks[Random.Range(0, validTracks.Count)].clip;

            if (currentClip != nextClip || !audioSource.isPlaying)
            {
                // Запускаем безопасное переключение для WebGL
                StartCoroutine(SwitchTrackRoutine(nextClip));
            }
        }
        else
        {
            audioSource.Stop();
            currentClip = null;
        }
    }

    // Корутина для безопасного переключения звука в браузерах
    private IEnumerator SwitchTrackRoutine(AudioClip nextClip)
    {
        // 1. Принудительно останавливаем текущий звук
        audioSource.Stop();

        // 2. Меняем клип
        currentClip = nextClip;
        audioSource.clip = currentClip;

        // 3. Ждем ровно 1 кадр. Глазом это не заметить (0.016 сек), 
        // но браузеру этого хватит, чтобы очистить буфер Web Audio API.
        yield return null;

        // 4. Запускаем новый трек
        audioSource.Play();
    }
}