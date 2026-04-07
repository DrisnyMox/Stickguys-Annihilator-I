using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {

	[SerializeField] float distanceToDeactive = 38f;

	public static Transform car;// From CarScript

	void LateUpdate(){
		if(Vector2.Distance(transform.position, car.position) > distanceToDeactive){
			gameObject.SetActive (false);
			Destroy (gameObject, 1.8f);
		}
	}
}
