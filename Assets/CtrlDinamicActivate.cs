using UnityEngine;
using System.Collections;

public class CtrlDinamicActivate : MonoBehaviour {

	[SerializeField] GameObject[] enables;
	[SerializeField] GameObject[] disables;

	void Start(){
		foreach (GameObject enable in enables) {
			if(enable)
				enable.SetActive (false);
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
		
		if (col.CompareTag ("CAR")) {
			StartCoroutine (Activate ());
		}
	}

	IEnumerator Activate(){
		foreach (GameObject enable in enables) {
			if(enable) enable.SetActive (true);
			yield return null;
		}
		foreach (GameObject disable in disables) {
			if (disable.GetComponent<ComponentMenager> ()) {
				disable.GetComponent<ComponentMenager> ().Deactive ();
			} else if (disable) {
				disable.SetActive (false);
			}
			yield return null;
		}
	}
}
