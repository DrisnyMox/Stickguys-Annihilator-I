using System.Collections;
using UnityEngine.UI;
using UnityEngine;


public class FrameSwitcher : MonoBehaviour {

	[SerializeField] Sprite[] frames;

	Image image;
	Coroutine corutinkO;

	
	void Awake()
	{
		image = GetComponent<Image>();
	}

	void OnEnable()
	{
		if (corutinkO != null)
			StopCoroutine(corutinkO);

		corutinkO = StartCoroutine(Perelistivatel());
	}

	IEnumerator Perelistivatel()
	{
		for (int i = 0; i < frames.Length; i++)
		{
			image.sprite = frames[i];
			yield return new WaitForSeconds(0.03f);
		}

		corutinkO = StartCoroutine(Perelistivatel());
	}


    private void OnDisable()
    {
		corutinkO = null;
	}
}
