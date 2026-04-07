using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour {

	public GameObject[] gms;


	void OnTriggerEnter2D(Collider2D col){
		if(col.CompareTag("CAR")){
			foreach (GameObject go in gms) {
				Destroy (go);
			}
		}
			
	}
}
