using UnityEngine;
using System.Collections;

public class SpawnerScript : MonoBehaviour
{
    [Header("Основные настройки")]
    public GameObject prefabToSpawn;      // Префаб, который будем спавнить
    public Transform referenceTransform;  // Трансформ, от которого считаем расстояние

    [Header("Параметры позиции")]
    public float spawnOffsetX = 8f;       // Смещение по X
    public float fixedY = 0.8791468f;     // Фиксированная высота по Y

    [Header("Условия спавна")]
    public float spawnInterval = 5f;      // Интервал спавна (секунды)
    public float minDistance = 1f;        // Минимальная дистанция между заспавненными объектами

    private Vector3 lastSpawnPosition;    // Позиция последнего заспавненного объекта
    private bool firstSpawnDone = false;  // Флаг для первого спавна

    void Start()
    {
        // Проверка на назначенные ссылки в инспекторе
        if (referenceTransform == null)
        {
            Debug.LogWarning("SpawnerScript: Не назначен Reference Transform! Использую текущий объект.");
            referenceTransform = transform;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("SpawnerScript: Не назначен Prefab To Spawn!");
            return;
        }

        // Запускаем бесконечный цикл спавна
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            TrySpawn();
        }
    }

    void TrySpawn()
    {
        // Рассчитываем потенциальную позицию для спавна
        // Берем X от назначенного объекта + смещение, Y — фиксированный
        float spawnX = referenceTransform.position.x + spawnOffsetX;
        Vector3 potentialPos = new Vector3(spawnX, fixedY, referenceTransform.position.z);

        // Если это первый спавн, или дистанция до предыдущего больше порога
        if (!firstSpawnDone || Vector3.Distance(potentialPos, lastSpawnPosition) >= minDistance)
        {
            Instantiate(prefabToSpawn, potentialPos, Quaternion.identity);

            // Запоминаем позицию и ставим флаг, что первый спавн прошел
            lastSpawnPosition = potentialPos;
            firstSpawnDone = true;
        }
        else
        {
            Debug.Log("Спавн пропущен: новая позиция слишком близка к предыдущей.");
        }
    }
}