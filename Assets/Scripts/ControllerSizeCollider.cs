using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControllerSizeCollider : MonoBehaviour {

	BoxCollider2D boxCollider;

	void Start () {
		boxCollider = GetComponent<BoxCollider2D> ();
		float width = GetComponent<Image>().GetPixelAdjustedRect().width;
		float height = GetComponent<Image>().GetPixelAdjustedRect().height;
		boxCollider.size = new Vector2 (width, height);
		Destroy (this);
	}
}
