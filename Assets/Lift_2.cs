using UnityEngine;
using System.Collections;

public class Lift_2 : MonoBehaviour {

	public int direction = -1;
	public float pause = 0;
	public float returnPause = 5;
	Vector2 pos;
	Vector2 oldPos;
	// Use this for initialization
	void Start () {
		pos = transform.GetChild (0).position;
		oldPos = transform.position;
		Destroy (transform.GetChild (0).gameObject);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (direction < 0) {
			if (transform.position.x < pos.x) {
				GetComponent<Rigidbody2D> ().isKinematic = true;
				transform.position = oldPos;
			}
		} else if (direction > 0) {
			if (transform.position.x > pos.x) {
				GetComponent<Rigidbody2D> ().isKinematic = true;
				transform.position = oldPos;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col){
		
		if(col.CompareTag("CAR")){
			StartCoroutine (Kick ());
		}
	}

	void ReturnPos(){
		transform.position = oldPos;
		GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	IEnumerator Kick(){
		yield return new WaitForSeconds (pause);
		GetComponent<Rigidbody2D> ().isKinematic = false;
		GetComponent<Rigidbody2D> ().AddForce (new Vector2(800000*direction, 0));
		Invoke ("ReturnPos", returnPause);
	}
}
