using UnityEngine;
using System.Collections;
using System;

public class Bullet : MonoBehaviour {

	[SerializeField] float distanceToDeactive = 38f;

	public static Transform car;// From CarScript

	Transform cam;

    private void Start()
    {
		cam = Camera.main.transform;
    }

	void LateUpdate()
	{
		var distanceToCar = Vector2.Distance(transform.position, car.position);
		var distanceToCam = Vector2.Distance(transform.position, cam.position);

		if (distanceToCam > distanceToDeactive && distanceToCar > distanceToDeactive)
		{
			Destroy(gameObject);
		}
	}
}
