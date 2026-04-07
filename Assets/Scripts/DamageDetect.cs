using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class DamageDetect : MonoBehaviour {

	Color oldColor;
	public GameObject blood;
	public int numberLevel;

	Transform ui;
	Transform PoolBlood;
	float timePause = 0;
	AudioSource source;
	SpriteRenderer sprRender;
	SpriteRenderer sprBone;
	bool corutinePlayed = false;
	StickmanAudioController stickAudioController;
	Text txtExp;
	public static GameObject car;
	Rigidbody2D rgdbody;
	int distanceOfDisabled;
	DetectRagdoll detectRagdoll;
	BoneColor boneColor;
	public static Fridge fridge;
	public static bool beFridge;

	bool isAudioSource;
	bool isStickmanAudionController;



	void Awake(){
		beFridge = false;
	}

	// Use this for initialization
	IEnumerator Start () {
		
		if (GameObject.Find ("Pool Blood")) {
			PoolBlood = GameObject.Find ("Pool Blood").transform;
		}

		if(GetComponent<SpriteRenderer> ()){
			oldColor = GetComponent<SpriteRenderer> ().color;
		}
		ui = GameObject.Find ("UI").transform;
		source = GetComponent<AudioSource> ();
		if (source)
			isAudioSource = true;
		sprRender = GetComponent<SpriteRenderer> ();
		stickAudioController = GetComponentInParent<StickmanAudioController> ();
		if (stickAudioController)
			isStickmanAudionController = true;
		txtExp = ui.GetChild (ui.childCount - 1).GetComponent<Text> ();
		if (transform.parent.name.IndexOf ("Xymus") >= 0) {
			HingeJoint2D hj2d = GetComponent<HingeJoint2D> ();
			if (hj2d && transform.parent.name.IndexOf("Pizdos") <= 0) {
				hj2d.breakForce = (UnityEngine.Random.Range (3f, 8f) * 10000f);
			}
		}
		rgdbody = GetComponent<Rigidbody2D> ();
		distanceOfDisabled = GetComponentInParent<ComponentMenager> ().distanceOfDisabled;
		detectRagdoll = GetComponent<DetectRagdoll> ();
		if (transform.childCount > 0) {
			transform.GetChild (0).GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
			transform.GetChild (0).gameObject.SetActive (false);
		}
		if (transform.Find ("Bone")) {
			sprBone = transform.Find ("Bone").GetComponent<SpriteRenderer> ();
		}
		boneColor = HUD.sBoneColor;
		yield return new WaitForSeconds (5.8f);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (source) {
			if (Time.timeScale >= 0.99f) {
				source.pitch = 1;
			} else {
				source.pitch = 0.15f;
			}
		}
		if (!detectRagdoll && rgdbody && rgdbody.isKinematic == false) {
			if (Vector3.Distance (transform.position, car.transform.position) > distanceOfDisabled) {
				gameObject.SetActive (false);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D col){

		float forceCollision = Mathf.Abs (col.relativeVelocity.x) + Mathf.Abs (col.relativeVelocity.y);
		if (forceCollision > 10) {
			if (col.transform.CompareTag ("CAR") || col.transform.CompareTag("BOBER") || col.transform.CompareTag("BULLET") ) {
				sprRender.color = new Color (0.58f, 0.153f, 0.153f);
				if (!corutinePlayed) {
					StartCoroutine (ColorDown ());
				}
				if(BloodActivator.enable)// TODO
				SpawnBlood ();
				if (isAudioSource) {
					if (source.time <= 0.001f) {
						source.Play ();
					}
				}

				if(isStickmanAudionController)
					stickAudioController.PlayShotScream ();
				if ( numberLevel <= 1 ) {
					Levels.currentExperience [numberLevel] += 10;
				} else {
					Levels.currentExperience [numberLevel] += (int)(10 * (numberLevel / 1.3f));
				}
				txtExp.text = "Exp:"+Levels.currentExperience[numberLevel].ToString ();

				if (sprBone) {
					//Color c = new Color (1,1,1,0);
					Color c = boneColor.currentColor;//new Color (0,0,0,0);
					c.a = Mathf.Clamp ((forceCollision / Time.timeScale / 300f), 0f, 0.88f);
					sprBone.color = c;
				}
			}
			if (col.transform.parent && col.transform.parent.CompareTag ("MEAT")) {
				if (transform.parent.name != col.transform.parent.name) {
					sprRender.color = new Color (0.54f, 0.164f, 0.164f);
					if (!corutinePlayed) {
						StartCoroutine (ColorDown ());
					}
					if(BloodActivator.enable)// TODO
					SpawnBlood ();
				
					if (numberLevel <= 1) {
						Levels.currentExperience [numberLevel] += 5;
					} else {
						Levels.currentExperience [numberLevel] += (int)(5*(numberLevel/1.3f));
					}
					//txtExp.text = "Exp: "+Levels.currentExperience[numberLevel].ToString ();
				}
			} 
		}
		if (beFridge) {
			Fridge.StickForFreeze stick = fridge.stickmansIced.Find (s => s.stickman == transform.parent);
			if (stick != null && stick.iced == true) {
				if (GetComponent<HingeJoint2D> ()) {
					GetComponent<Rigidbody2D> ().gravityScale = 1;
					GetComponent<Rigidbody2D> ().drag = 0;
					Destroy (GetComponent<HingeJoint2D> ());
					foreach (IceBreak ib in IceBreak.breaks) {
						if (!ib.use) {
							ib.SetBreak (transform);
							break;
						}
					}
				}
			}
		}
	}

	void SpawnBlood(){
		if (Blood.GetBlood (transform)) {
			timePause = Time.deltaTime;
		}
	}

	IEnumerator ColorDown(){
		corutinePlayed = true;
		yield return new WaitForSeconds (1.8f);
		sprRender.color = oldColor;
		if(sprBone)
			sprBone.color = new Color (1, 1, 1, 0);
		corutinePlayed = false;
	}
}
