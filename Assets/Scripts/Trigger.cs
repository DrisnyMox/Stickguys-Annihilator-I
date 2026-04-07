using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col){
		//var car = GameObject.Find ("CAR");
		if (col.CompareTag("CAR")) {
			CarScript cs = col.GetComponent<CarScript> ();
			if (gameObject.name.IndexOf ("End") == -1 ) {
				if (cs && col.GetComponent<CarScript> ().isPassed == false) {
					cs.RunCheck ();
				}
			} else {
				//cs.EnableLevelComplite ();
				cs.LevelComplete();
			}
		}
	}

}
