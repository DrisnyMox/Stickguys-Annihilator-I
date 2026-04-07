using UnityEngine;
using System.Collections;

public class TNTStandalone : MonoBehaviour {

	[SerializeField] GameObject TNT;
	GameObject tntClone;
	GameObject containerTNTStandalone;

	void Awake(){
		tntClone = (GameObject) Instantiate (TNT, transform.position, Quaternion.identity);
		tntClone.SetActive (false);
		containerTNTStandalone = GameObject.Find ("Container TNT Standalone");
		if (containerTNTStandalone)
			tntClone.transform.parent = containerTNTStandalone.transform;
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.CompareTag ("CAR") || col.CompareTag ("BOBER")) {
			StartCoroutine (Pause());
		}
	}

	IEnumerator Pause(){
		yield return new WaitForSeconds (Random.Range (0, 0.25f));
		tntClone.SetActive (true);
		Destroy (gameObject);
	}
}
