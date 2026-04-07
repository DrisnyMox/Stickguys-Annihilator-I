using UnityEngine;
using System.Collections;

public class ComponentMenager : MonoBehaviour {
	
	public GameObject blood;// prefab
	public AudioClip crash;// from Inspector
	[SerializeField] GameObject christmasHat;
	[SerializeField] GameObject christmasBoots;
	[HideInInspector] public int numberLevel;
	[Range(50,300)] public int distanceOfDisabled = 88;
	// Use this for initialization
	void Awake(){
		numberLevel = Game.GetNumberCurrentLevel ();
	}

	void Start () {
		DetectRagdoll dr = GetComponentInChildren<DetectRagdoll> ();
		dr.distanceOfDisabled = distanceOfDisabled;
		DetectRagdoll.countActiveSlowMo = 0;
		DetectRagdoll.countActiveRigidbody = 0;

		for (int i = 0; i < transform.childCount; i++) {
			if (transform.GetChild (i).childCount > 0) {
				for (int j = 0; j < transform.GetChild(i).childCount; j++) {
					if (transform.GetChild (i).GetChild (j).name != "Bone") {
						transform.GetChild (i).GetChild (j).gameObject.AddComponent <DamageDetect> ();
						transform.GetChild (i).GetChild (j).gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.1f, 0.1f, 0.1f);
						if (blood) {
							transform.GetChild (i).GetChild (j).GetComponent<DamageDetect> ().blood = blood;
							transform.GetChild (i).GetChild (j).GetComponent<DamageDetect> ().numberLevel = numberLevel;
						}
					}
				}
			}
			//print (transform.GetChild (i).name);
			if (transform.GetChild (i).name != "Bone") {
				transform.GetChild (i).gameObject.AddComponent <DamageDetect> ();
				if (transform.GetChild (i).gameObject.GetComponent<SpriteRenderer> ()) {
					transform.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().color = new Color (0.1f, 0.1f, 0.1f);
					if (blood) {
						transform.GetChild (i).GetComponent<DamageDetect> ().blood = blood;
						transform.GetChild (i).GetComponent<DamageDetect> ().numberLevel = numberLevel;
					}
				}
			}
		}

		ChristmasEquip ();
	}

	public void Deactive(){
		GetComponentInChildren<DetectRagdoll> ().Deactive ();
	}



	void ChristmasEquip(){
		if (System.DateTime.Now.Month == 12 || System.DateTime.Now.Month == 1) {
			if (christmasHat) {
				Transform head = transform.GetChild (0);
				GameObject ch =	Instantiate (christmasHat, head);
				ch.transform.GetChild (1).GetComponent<HingeJoint2D> ().connectedBody = head.GetComponent<Rigidbody2D> ();
				ch.transform.GetChild (1).GetComponent<HingeJoint2D> ().breakForce = Random.Range (1, 8) * 8000;
			}

			if (christmasBoots && Random.Range (0, 8) >= 7) {
				Transform legRight = transform.GetChild (4);
				Transform legLeft = transform.GetChild (6);
				GameObject cb = Instantiate (christmasBoots, legRight);
				cb.GetComponent<FixedJoint2D> ().connectedBody = legRight.GetComponent<Rigidbody2D> ();
				cb.GetComponent<FixedJoint2D> ().breakForce = Random.Range (1, 8) * 7000;
				cb = Instantiate (christmasBoots, legLeft);
				cb.GetComponent<FixedJoint2D> ().connectedBody = legLeft.GetComponent<Rigidbody2D> ();
				cb.GetComponent<FixedJoint2D> ().breakForce = Random.Range (1, 8) * 7000;
			}
		}
	}
}
