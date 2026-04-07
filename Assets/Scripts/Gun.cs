using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {

	[SerializeField] GameObject core;

	public IEnumerator Fire(){
		GameObject c = Instantiate (core, transform.position+new Vector3(0,0,1), Quaternion.identity);
		c.transform.parent = transform;
		c.transform.localPosition += new Vector3 (3, 0, 0);
		c.transform.localScale *= transform.localScale.y;
		float force = 1000 / Time.timeScale;
		c.GetComponent<Rigidbody2D> ().AddForce (transform.right * force, ForceMode2D.Impulse);
		if (GetComponent<AudioSource> ())
			GetComponent<AudioSource> ().Play ();
		yield return null;

		c.transform.parent = null;
	}
}
