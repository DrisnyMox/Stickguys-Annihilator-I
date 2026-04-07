using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitterSurfaceEffector : MonoBehaviour {

	[SerializeField] float sign = 1;

	public void FitForce () {
		SurfaceEffector2D se2d = GetComponent<SurfaceEffector2D> ();
		Transform car = FindObjectOfType<CarScript> ().transform;
		float massCar = car.GetComponent<Rigidbody2D> ().mass;
		for (int i = 0; i < car.childCount; i++) {
			if (car.GetChild (i).GetComponent<Rigidbody2D> ()) {
				massCar += car.GetChild (i).GetComponent<Rigidbody2D> ().mass;
			}
		}
		se2d.forceScale = sign * massCar * 8;
	}
}
