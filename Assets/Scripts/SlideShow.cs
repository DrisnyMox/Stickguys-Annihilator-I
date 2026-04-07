using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideShow : MonoBehaviour {

	[SerializeField] Sprite[] slides;

	IEnumerator Start () {
		yield return new WaitForSeconds (1.5f);
		foreach (Sprite s in slides) {
			GetComponent<Image> ().sprite = s;
			yield return new WaitForSeconds (1.5f);
		}
		StartCoroutine (Start ());
	}
}
