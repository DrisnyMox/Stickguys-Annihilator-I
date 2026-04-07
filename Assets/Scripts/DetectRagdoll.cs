using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DetectRagdoll : MonoBehaviour {

	Rigidbody2D[] rgs = null;
	public static float old;
	public bool enableFollow = false;
	public int percentEnabled = 70;
	public float timeFollowing = 1.19f;
	[SerializeField] BoxCollider2D triggerBox;
	bool slowMoIsplayed = true;
	[HideInInspector] public int distanceOfDisabled = 88;

	public static float timeSlowDivider = 1.189f;
	public static int countActiveRigidbody;
	public static int countActiveSlowMo;

	Rigidbody2D rigidbody2d;
	SmoothCamera camera;
	public static Transform car;
	Transform cameraTarget;

	StickmanAudioController stickmanAudioController;

	bool enbl = false;

	void Start () {
		rgs = transform.parent.GetComponentsInChildren<Rigidbody2D> ();
		for (int i = 0; i < rgs.Length; i++) {
			rgs [i].isKinematic = true;
			//rgs [i].isKinematic = false;
			//rgs [i].Sleep ();
		} 
		old = Time.fixedDeltaTime;
		rigidbody2d = GetComponent<Rigidbody2D> ();
		camera = Camera.main.GetComponent<SmoothCamera> ();
		/*if (GameObject.FindGameObjectWithTag ("CAR").transform) {
			car = GameObject.FindGameObjectWithTag ("CAR").transform;
			print (car);
		}/**/
		stickmanAudioController = transform.parent.GetComponent<StickmanAudioController> ();
		var colliders = GetComponents<BoxCollider2D> ();
		foreach (BoxCollider2D bc2d in colliders) {
			if (bc2d.isTrigger) {
				triggerBox = bc2d;
			}
		}
	}//_____________________________________________

	void FixedUpdate(){
		if (rigidbody2d.isKinematic == false ) {
			//if (camera.target) {
			float distance = Vector2.Distance (camera.target.position, transform.position);
			if ( slowMoIsplayed || (countActiveRigidbody > 8 && countActiveSlowMo > 1)) {
				if (distance > distanceOfDisabled) {
					transform.parent.gameObject.SetActive (false);
					countActiveRigidbody--;
					countActiveSlowMo--;
				}
			}
			//}
			if (countActiveRigidbody > 18 && countActiveSlowMo > 1 && distance > distanceOfDisabled/2) {
				transform.parent.gameObject.SetActive (false);
				countActiveRigidbody--;
				countActiveSlowMo--;
			}
		}/**/
	}//_____________________________________________

	void OnTriggerEnter2D (Collider2D col) {
		if (!enbl) {
			if (col.CompareTag ("CAR") || col.CompareTag ("MEAT") || col.CompareTag ("BOBER") || col.CompareTag ("BULLET")) {
				for (int i = 0; i < rgs.Length; i++) {
					//if (rgs [i].isKinematic == true) {
						rgs [i].isKinematic = false;
					//rgs [i].WakeUp();
						if (Settings.slowMo) {
							if (col.CompareTag ("CAR") || col.CompareTag ("BOBER") ) {
								if (Time.timeScale > 0.08f) {
									StartCoroutine (SlowMo ());
									//enbl = true;
								}
								if (i == rgs.Length - 1) {
									if (enableFollow || col.CompareTag ("BULLET")) { 
										StartCoroutine (FollowCamera ());
										//enbl = true;
									} 
								}
							}
						}
						if (i == rgs.Length - 1 && stickmanAudioController) {
							stickmanAudioController.PlayScream ();
							countActiveRigidbody++;
						}
					//} else {
					//	break;
					//}
				}
				enbl = true;
				//triggerBox.isTrigger = false;
				//triggerBox.enabled = false;
			}
		}
	}//________________________________________________________

	public void Deactive(){
		if (countActiveRigidbody > 1 && countActiveSlowMo > 1) {
			if (!slowMoIsplayed)
				countActiveSlowMo--;
			transform.parent.gameObject.SetActive (false);
			countActiveRigidbody--;
		}
	}

	IEnumerator SlowMo(){
		Time.timeScale /= timeSlowDivider;
		Time.fixedDeltaTime /= timeSlowDivider;
		slowMoIsplayed = false;
		countActiveSlowMo++;

		yield return new WaitForSeconds (0.85f);
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.028f;//0.03f;
		slowMoIsplayed = true;
		countActiveSlowMo--;
	}

	IEnumerator FollowCamera(){
		camera.target = transform;
		yield return new WaitForSeconds (timeFollowing);
		camera.target = car;
	}
}
