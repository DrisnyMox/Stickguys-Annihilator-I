using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AreaEffector2D))]
public class FitterForceEffector : MonoBehaviour {

	[SerializeField] float divider = 1f;

	public void FitForce () {
		AreaEffector2D ae2d = GetComponent<AreaEffector2D> ();
		CarScript car = FindObjectOfType<CarScript> ();
		float massCar = car.GetTotalMass ();
		ae2d.forceMagnitude = (massCar * 8) / divider;
	}

}
