using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour {

	//List<int> intiy = new List<int> (new[] {1,2,3});
	List<Transform> stickmans = new List<Transform>();
	public List<StickForFreeze> stickmansIced = new List<StickForFreeze>();
	[SerializeField] AudioSource audio;
	[SerializeField] public AudioSource audioBreak;

	public class StickForFreeze {
		public Transform stickman;
		public bool iced;

		public StickForFreeze (Transform stickman){
			this.stickman = stickman;
			iced = false;
		}
	}

	void Start(){
		DamageDetect.fridge = this;
		DamageDetect.beFridge = true;
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (!col.transform.parent)
			return;
		if (col.transform.parent.tag == "MEAT") {
			if (stickmans.Find (s => s == col.transform.parent) == null) {
				StartCoroutine (Freeze (col.transform.parent, col.relativeVelocity));
				stickmans.Add (col.transform.parent);
			}
			StickForFreeze sff = stickmansIced.Find (s => s.stickman == col.transform.parent);
			if (stickmansIced.Find (s => s.stickman == col.transform.parent) == null) {
				StartCoroutine (Freeze (col.transform.parent));
				StartCoroutine (Blinking());
				stickmansIced.Add (new StickForFreeze (col.transform.parent));
			} else if (sff.iced){
				for (int i = 0; i < sff.stickman.childCount; i++) {
					if (sff.stickman.GetChild (i).GetComponent<HingeJoint2D> () ) {
						if (!audioBreak.isPlaying) {
							audioBreak.Play ();
							foreach (IceBreak ib in IceBreak.breaks) {
								if (!ib.use) {
									ib.SetBreak (sff.stickman.GetChild(2));
									break;
								}
							}
						}
						Destroy (sff.stickman.GetChild (i).GetComponent<HingeJoint2D> ());
					}
					sff.stickman.GetChild (i).GetComponent<Rigidbody2D> ().gravityScale = 1;
					sff.stickman.GetChild (i).GetComponent<Rigidbody2D> ().drag = 0;
				}

			}
		}
	}

	IEnumerator Freeze(Transform stickman, Vector3 vel = default(Vector3) ){
		yield return null;
		for (int j = 0; j < stickman.childCount; j++) {
			if (stickman.GetChild (j).childCount > 0) {
				stickman.GetChild (j).GetChild (0).gameObject.SetActive (true);
				stickman.GetChild (j).GetChild (0).GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 0);

				Vector2 dir = stickman.GetChild (j).GetComponent<Rigidbody2D> ().velocity;
				float force;
				force = 310 * stickman.GetChild (j).GetComponent<Rigidbody2D> ().mass;
				force /= Time.timeScale;

				stickman.GetChild (j).GetComponent<Rigidbody2D> ().AddForce (Vector2.up * force);


			}


		}
		yield return new WaitForSeconds (0.2f);
		audio.Play ();
		yield return new WaitForSeconds (0.1f);
		int iteration = 58;

		for (int i = 0; i < iteration; i++) {
			yield return null;
				for (int j = 0; j < stickman.childCount; j++) {
				stickman.GetChild (j).GetComponent<Rigidbody2D> ().drag += 0.08f;
				stickman.GetChild (j).GetChild (0).GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, (float)i / iteration);
			}
		}

		for (int j = 0; j < stickman.childCount; j++) {
			stickman.GetChild (j).GetComponent<Rigidbody2D> ().gravityScale = 0;
			stickman.GetChild (j).GetComponent<Rigidbody2D> ().angularDrag = 0;
		}
		stickmansIced.Find (s => s.stickman == stickman).iced = true;
	}

	IEnumerator Blinking(){
		int iter = 300;
		for (int i = 0; i < iter; i++) {
			GetComponent<SpriteRenderer> ().color -= new Color (0.001f,0.001f,0,0);
			yield return null;
		}
		for (int i = 0; i < iter; i++) {
			GetComponent<SpriteRenderer> ().color += new Color (0.001f,0.001f,0,0);
			yield return null;
		}
	}

	void OnDestroy(){
		IceBreak.breaks.Clear ();
		DamageDetect.beFridge = false;
	}
}
