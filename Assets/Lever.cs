using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	[SerializeField] GameObject[] objectsForHide;
	[SerializeField] bool carOnly;

	void OnTriggerEnter2D( Collider2D col ){
		if (carOnly) {
			if (col.CompareTag ("CAR")) {
				GetComponent<SpriteRenderer> ().color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
				foreach (GameObject go in objectsForHide) {
					Destroy (go);
				}
			}
		} else {
			if (col.CompareTag ("CAR") || col.CompareTag ("BOBER") || col.CompareTag ("MEAT")) {
				GetComponent<SpriteRenderer> ().color = new Color (0.5f, 0.5f, 0.5f, 0.5f);
				foreach (GameObject go in objectsForHide) {
					Destroy (go);
				}
			}
		}
	}
}
