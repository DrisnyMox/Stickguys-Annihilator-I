using AppodealAds.Unity.Api;
using AppodealAds.Unity.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;


public class CarScript : MonoBehaviour, IRewardedVideoAdListener {

	WheelJoint2D[] wheelJoints;
	JointMotor2D frontWheel;
	JointMotor2D backWheel;

	public float maxSpeed = -12000f;
	public float brakeForce = 12000f;
	//==========================
	private float maxBackSpeed = 50000f;
	private float acceleration = 1500f;
	private float deacceleration = -1000f;
	private float gravity = 9.8f;
	private float angleCar = 0;

	Vector2 oldPos = Vector2.zero;
	public int steps = 0;
	int generalDistances = 0;
	public float currentTimeScale;
	public bool isPassed  = false;
	public bool customCar = false;
	bool isGas = false;

	public ClickScript[] ControlCar = new ClickScript[2];
	public GameObject p_LevelComplete;// from inspector
	public GameObject p_HUD;// from inspector
	GameObject btnActiveBlood;
	public int usedGears;
	int numberLevel;

	float posYWheel;
	float posYWheel2;
	bool mojno = false;

	public void Brokoli(){
		print ("ta ta da da brokolli");
	}
	public void Moxol(){
		print ("pizda, pizda");
	}

	void Awake(){
		Bullet.car = transform;
		Sliver.car = transform;
		OutLevelHandler.car = this;
	}

	IEnumerator Start () {
		Appodeal.setRewardedVideoCallbacks(this);
		/*p_HUD.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener ( delegate() {
			this.Moxol();			
		} );
		p_HUD.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener ( () => Brokoli () );/**/

		if(p_HUD)
			p_HUD.SetActive (true);
		if(p_LevelComplete)
			p_LevelComplete.SetActive (false);

		var stickman = (ComponentMenager)FindObjectOfType (typeof(ComponentMenager));
		if(stickman)
			numberLevel = stickman.numberLevel;

		Levels.currentExperience[numberLevel] = 0;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		wheelJoints = gameObject.GetComponents<WheelJoint2D>();
		backWheel = wheelJoints[1].motor;
		frontWheel = wheelJoints[0].motor;
		isGas = false;
		//var myLoadedAssetBundle = AssetBundle.LoadFromFile ("brokol");
		var prefabs = Resources.FindObjectsOfTypeAll (typeof(GameObject));
		for (int i = 0; i < prefabs.Length; i++) {
			if (prefabs [i].name == "Blood") {
				//print(prefabs[i].name);
			}
		}
		if (customCar) {
			
		} else {
			posYWheel = transform.GetChild (0).transform.localPosition.y;
			posYWheel2 = transform.GetChild (1).transform.localPosition.y;
			mojno = true;
		}


		yield return null;
		DetectRagdoll.car = transform;
		var fittersEffector = FindObjectsOfType<FitterForceEffector> ();
		foreach (FitterForceEffector ffe in fittersEffector) {
			ffe.FitForce ();
		}
		btnActiveBlood = GameObject.Find ("Btn Enable Blood");
	}

	void FixedUpdate () {
		float maxDist = transform.name.IndexOf ("Tank") > -1 ? 0.39f : 1.1f;

		if (mojno) {
			if (transform.GetChild (0).transform.localPosition.y - posYWheel > maxDist || transform.GetChild (0).transform.localPosition.y - posYWheel < -0.5f) {
				transform.GetChild (0).transform.localPosition = new Vector3 (transform.GetChild (0).transform.localPosition.x, posYWheel, -0.05f);
			}
			if (transform.GetChild (1).transform.localPosition.y - posYWheel2 > maxDist || transform.GetChild (1).transform.localPosition.y - posYWheel2 < -0.5f) {
				transform.GetChild (1).transform.localPosition = new Vector3 (transform.GetChild (1).transform.localPosition.x, posYWheel2, -0.05f);
			}
		}

		frontWheel.motorSpeed = backWheel.motorSpeed;

		angleCar = transform.localEulerAngles.z;

		if (angleCar >= 180)
		{
			angleCar = angleCar - 360;
		}

		if (ControlCar[0].clickedIs == true)
		{
            backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (acceleration - gravity * Mathf.PI * (angleCar / 180) * 80) * Time.deltaTime, maxSpeed, maxBackSpeed);
        }
		if ((ControlCar[0].clickedIs == false && backWheel.motorSpeed < 0) || (ControlCar[0].clickedIs == false && backWheel.motorSpeed == 0 && angleCar < 0))
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (deacceleration - gravity * Mathf.PI * (angleCar / 180) * 80) * Time.deltaTime, maxSpeed, 0);
		}
		else if ((ControlCar[0].clickedIs == false && backWheel.motorSpeed > 0) || (ControlCar[0].clickedIs == false && backWheel.motorSpeed == 0 && angleCar > 0))
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (-deacceleration - gravity * Mathf.PI * (angleCar / 180) * 80) * Time.deltaTime, 0, maxBackSpeed);
		}

		if (ControlCar[1].clickedIs == true && backWheel.motorSpeed > 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - brakeForce * Time.deltaTime, 0, maxBackSpeed);
		}
		else if (ControlCar[1].clickedIs == true && backWheel.motorSpeed < 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed + brakeForce * Time.deltaTime, maxSpeed, 0);
		}

		wheelJoints[1].motor = backWheel;
		wheelJoints[0].motor = frontWheel;
		if (wheelJoints.Length > 2) {
			for (int i = 2; i < wheelJoints.Length; i++) {
				wheelJoints [i].motor = backWheel;
			}
		}

		if (ControlCar [0].clickedIs == true) {
			if (isGas == false && Game.currentCoins > 800 ) {
				p_HUD.transform.GetChild (3).gameObject.SetActive (true);
				CheckFire.Check ();
			}
			if(!isGas)
				p_HUD.transform.Find ("btn_Switch Background").gameObject.SetActive (false);
			isGas = true;
			p_HUD.transform.GetChild (1).gameObject.SetActive (false);
			if(btnActiveBlood) btnActiveBlood.SetActive(false);
		}
	}//________________________________________________________

	IEnumerator checkMove(){
		int csoom = 6;
		if (numberLevel == 7)
			csoom = 8;
		yield return new WaitForSeconds (0.5f);
		if (Vector2.Distance (oldPos, transform.position) < 0.15f) {
			steps++;
			generalDistances += (int)Vector2.Distance (oldPos, transform.position);
			if (steps >= csoom) {
				LevelComplete ();
			}
		} else {
			steps = 0;
		}

		oldPos = transform.position;

		if(Camera.main.GetComponent<SmoothCamera> ().target == null ){
			Camera.main.GetComponent<SmoothCamera> ().target = transform;
		}
		if (steps < csoom) {
			StartCoroutine (checkMove ());
		}
			
		if (transform.position.magnitude > 8000) {
			LevelComplete ();
		}
	}

	public void LevelComplete(){
		HUD.tntSelected = false;
		GetComponent<Rigidbody2D> ().isKinematic = true;
		isPassed = false;
		numberLevel = Game.GetNumberCurrentLevel ();

		p_HUD.SetActive (false);
		p_LevelComplete.SetActive (true);

		GameObject.Find ("txt_Again").GetComponent<Text> ().text = Settings.lng.txt_Again;
		GameObject.Find ("txt_Menu").GetComponent<Text> ().text = Settings.lng.txt_Menu;

		GameObject.Find ("txt_Completed").GetComponent<Text> ().text = Settings.lng.txt_Completed;
		GameObject.Find ("txt_Experience").GetComponent<Text> ().text = Settings.lng.txt_Experience;
		GameObject.Find ("txt_MaxExperience ").GetComponent<Text> ().text = Settings.lng.txt_Record;
		GameObject.Find ("txt_Coins").GetComponent<Text> ().text = Settings.lng.txt_CoinsEarned;
		GameObject.Find ("txt_AllCoins").GetComponent<Text> ().text = Settings.lng.txt_Coins;

		if (Levels.currentExperience [numberLevel] > Levels.recordExperience [numberLevel]) {
			Levels.recordExperience[numberLevel] = Levels.currentExperience [numberLevel];
			GameObject.Find ("txt_Completed").GetComponent<Text> ().text = "-= NEW RECORD !!! =-";
			GameObject.Find ("txt_Completed").GetComponent<Text> ().color = new Color (0.1f, 1, 0.1f);
		}
		GameObject.Find ("txt_OutExperience").GetComponent<Text> ().text = Levels.currentExperience [numberLevel].ToString ();
		GameObject.Find ("txt_OutMaxExperience").GetComponent<Text> ().text = Levels.recordExperience [numberLevel].ToString ();

		int coins = (int)(Levels.currentExperience[numberLevel] / (38.2f / 10f));
		Game.currentCoins += coins;
		Game.SaveCoins ();
		GameObject.Find ("txt_OutCoins").GetComponent<Text> ().text = coins.ToString ();
		GameObject.Find ("txt_AllCoins").GetComponent<Text> ().text = Settings.lng.txt_Coins + Game.currentCoins.ToString ();
		Levels.SaveData ();
		HUD.HelperLevelShop ();
	}//________________________________________________________

	public void RunCheck () {
		isPassed = true;
		oldPos = transform.position;
		StartCoroutine (checkMove());
	}//________________________________________________________
		

	public void ShowAdvertise(){
		if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)){
			Appodeal.show (Appodeal.REWARDED_VIDEO);
		} else if (Appodeal.isLoaded (Appodeal.INTERSTITIAL)) {
			Appodeal.show (Appodeal.INTERSTITIAL);
		}
		Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);
		Appodeal.initialize(AdvertiseService.appKey, Appodeal.REWARDED_VIDEO);
		Appodeal.cache(Appodeal.REWARDED_VIDEO);
	}

	

	

	public float GetTotalMass(){
		float massTotal = 0;
		if (customCar) {
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).GetComponent<Rigidbody2D> ()) {
					massTotal += transform.GetChild (i).GetComponent<Rigidbody2D> ().mass;
				}
			}
			for (int i = 0; i < transform.parent.childCount; i++) {
				massTotal += transform.parent.GetChild (i).GetComponent<Rigidbody2D> ().mass;
			}
		} else {
			massTotal = GetComponent<Rigidbody2D> ().mass;
			for (int i = 0; i < transform.childCount; i++) {
				if (transform.GetChild (i).GetComponent<Rigidbody2D> ()) {
					massTotal += transform.GetChild (i).GetComponent<Rigidbody2D> ().mass;
				}
			}
		}
		return massTotal;
	}

	public void Placed(){
		transform.localRotation = Quaternion.Euler (0, 0, 0);

		float massTotal = GetTotalMass ();

		if (transform.name.IndexOf ("Locker") > -1) {
			massTotal *= 3;
		}

		massTotal /= Time.timeScale;

		GetComponent<Rigidbody2D> ().AddForce (Vector2.up * massTotal * 300);
		GetComponent<Rigidbody2D> ().angularVelocity = 0;
		//GetComponentInChildren<Rigidbody2D> ().AddForce (Vector2.up * massTotal * 30);
	}

    #region Rewarded Video callback handlers
    public void onRewardedVideoLoaded(bool precache)
    {
        
    }

    public void onRewardedVideoFinished(double amount, string name)
    {
        int tnt = Random.Range(8, 11);
        TNT.IncreaseTNT(tnt);
        Game.gears += 1;
        Game.SaveGears();
        PanelReward.Show(tnt.ToString(), "1");
    }

    public void onRewardedVideoExpired()
    {
        
    }
    public void onRewardedVideoClicked()
    {
        
    }
    public void onRewardedVideoClosed(bool finished)
    {
        
    }
    #endregion

    

    public void onRewardedVideoFailedToLoad()
    {
        
    }

    public void onRewardedVideoShown()
    {
        
    }

	public void onRewardedVideoShowFailed()
	{
		throw new System.NotImplementedException();
	}
}
