using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [Tooltip("Имя сцены, которую нужно загрузить")]
    [SerializeField] private string sceneToLoad = "Menu";

    [Tooltip("Минимальное время загрузки в секундах")]
    [SerializeField] private float minLoadingTime = 0.5f;

    private void Start()
    {
        // Запускаем корутину при старте сцены
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        // 1. Фиксируем реальное время начала загрузки
        float startTime = Time.realtimeSinceStartup;

        // 2. Запускаем асинхронную загрузку сцены
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);

        // 3. Запрещаем автоматическое переключение сцены, пока мы не скажем true
        asyncLoad.allowSceneActivation = false;

        // 4. Ждем завершения загрузки сцены в память
        // Unity останавливает progress на 0.9f, если allowSceneActivation == false
        while (asyncLoad.progress < 0.9f)
        {
            yield return null; // Ждем следующий кадр
        }

        // 5. Проверяем, сколько времени заняла сама загрузка
        float elapsedTime = Time.realtimeSinceStartup - startTime;

        // 6. Если сцена загрузилась слишком быстро, ждем оставшееся время
        if (elapsedTime < minLoadingTime)
        {
            float timeToWait = minLoadingTime - elapsedTime;
            yield return new WaitForSecondsRealtime(timeToWait);
        }

        // 7. Разрешаем активацию и переходим в сцену Menu
        asyncLoad.allowSceneActivation = true;
    }
}