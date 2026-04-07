using UnityEngine;
using System.Collections;

public class Lift : MonoBehaviour {

	Vector3 downPos, upPos;
	public float force = 8000000;
	// Use this for initialization
	void Start () {
		downPos = transform.GetChild (0).transform.position;
		upPos = transform.position;
		Destroy(transform.GetChild(0).gameObject);
		GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -force));
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (transform.position.y < downPos.y) {
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, force));
			//force = Random.Range (50000, 300000);
		} else if (transform.position.y > upPos.y) {
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -force));
			//force = Random.Range (50000, 300000);
		}/**/
	}
}
