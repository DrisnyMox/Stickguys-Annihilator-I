using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CellResize : MonoBehaviour {

	IEnumerator Start () {
		yield return null;// new WaitForSeconds(0.01f);
		RectTransform parent = GetComponent<RectTransform> ();
		GridLayoutGroup grid = GetComponent<GridLayoutGroup> ();

		int h = Screen.height / 4;
		grid.cellSize = new Vector2 ((parent.rect.width - 20) / 4, h);
		Destroy (this);
	}

	void OnEnable(){
		StartCoroutine (Start());
	}
}
