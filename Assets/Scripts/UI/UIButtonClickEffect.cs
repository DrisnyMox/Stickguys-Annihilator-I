using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIButtonClickEffect : MonoBehaviour
{
    [Header("Настройки анимации")]
    [Tooltip("Во сколько раз увеличится клон")]
    [SerializeField] private float targetScale = 1.5f;
    [Tooltip("Длительность анимации в реальных секундах")]
    [SerializeField] private float duration = 0.4f;

    public void PlayEffect()
    {
        // Корутина будет работать даже при Time.timeScale = 0
        StartCoroutine(AnimateCloneCoroutine());
    }

    private IEnumerator AnimateCloneCoroutine()
    {
        // 1. Создаем клон
        GameObject clone = Instantiate(gameObject, transform.parent);
        clone.transform.position = transform.position;
        clone.transform.SetSiblingIndex(transform.GetSiblingIndex());

        // 2. Чистим компоненты
        Destroy(clone.GetComponent<UIButtonClickEffect>());
        Button btn = clone.GetComponent<Button>();
        if (btn != null) Destroy(btn);

        LayoutElement layoutElement = clone.GetComponent<LayoutElement>();
        if (layoutElement == null) layoutElement = clone.AddComponent<LayoutElement>();
        layoutElement.ignoreLayout = true;

        CanvasGroup canvasGroup = clone.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = clone.AddComponent<CanvasGroup>();

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 1f;

        // 3. Подготовка к анимации
        Vector3 startScale = clone.transform.localScale;
        Vector3 endScale = startScale * targetScale;
        float elapsedTime = 0f;

        // 4. Цикл анимации с использованием реального времени
        while (elapsedTime < duration)
        {
            // ИСПОЛЬЗУЕМ unscaledDeltaTime вместо deltaTime
            elapsedTime += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsedTime / duration);

            // Плавное затухание (Ease Out)
            float smoothT = t * (2f - t);

            clone.transform.localScale = Vector3.Lerp(startScale, endScale, smoothT);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, smoothT);

            // Ждем следующего кадра (работает при любом timeScale)
            yield return null;
        }

        Destroy(clone);
    }
}