using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupFadeOnEnable : MonoBehaviour {

	[SerializeField] float duration = 0.25f;
	[SerializeField] float startAlpha = 0f;
	[SerializeField] float targetAlpha = 1f;
	[SerializeField] bool ignoreTimeScale = true;
	[SerializeField] bool blockRaycastsAfterFade = true;
	[SerializeField] bool interactableAfterFade = true;

	CanvasGroup canvasGroup;
	Coroutine fadeCoroutine;

	void Awake () {
		canvasGroup = GetComponent<CanvasGroup> ();
	}

	void OnEnable () {
		if (fadeCoroutine != null) {
			StopCoroutine (fadeCoroutine);
		}

		canvasGroup.alpha = Mathf.Clamp01 (startAlpha);
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
		fadeCoroutine = StartCoroutine (FadeRoutine ());
	}

	IEnumerator FadeRoutine () {
		float safeDuration = Mathf.Max (duration, 0.0001f);
		float timer = 0f;

		while (timer < safeDuration) {
			timer += ignoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
			float t = Mathf.Clamp01 (timer / safeDuration);
			canvasGroup.alpha = Mathf.Lerp (startAlpha, targetAlpha, t);
			yield return null;
		}

		canvasGroup.alpha = Mathf.Clamp01 (targetAlpha);
		canvasGroup.blocksRaycasts = blockRaycastsAfterFade;
		canvasGroup.interactable = interactableAfterFade;
		fadeCoroutine = null;
	}
}
