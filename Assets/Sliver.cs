using UnityEngine;
using System.Collections;
using System;

public class Sliver : MonoBehaviour {

	public static Transform car;

	Action CheckDistance = ()=>{};

	void Start(){
		if (car) {
			CheckDistance = () => {
				if(Vector2.Distance(transform.position, car.position) > 33f){
					gameObject.SetActive (false);
				}
			};
		}
	}

	void LateUpdate(){
		CheckDistance ();
	}

	void OnBecameInvisible(){
		gameObject.SetActive (false);
	}

}
