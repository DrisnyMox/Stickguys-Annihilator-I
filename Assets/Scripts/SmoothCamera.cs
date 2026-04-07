using UnityEngine;
using System.Collections;

public class SmoothCamera : MonoBehaviour {
	public float dampTime = 0.15f;
	private Vector3 velocity = Vector3.zero;
	public Transform target;
	
	// Update is called once per frame
	void Start(){
		GetComponent<Camera> ().orthographicSize = Settings.distanceCamera;
		DamageDetect.car = gameObject;
	}

	void FixedUpdate () 
	{
		if (target)
		{
			//print (target.GetComponent<Rigidbody2D>().velocity.magnitude);
			dampTime = Mathf.Clamp((3.8f / target.GetComponent<Rigidbody2D>().velocity.magnitude), 0.018f, 0.119f);
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(new Vector3(target.position.x, target.position.y+0.75f,target.position.z));
			Vector3 delta = new Vector3(target.position.x, target.position.y+0.75f,target.position.z) - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;


			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
		}
		
	}
}
