//using AppodealAds.Unity.Api;
//using AppodealAds.Unity.Common;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class CarScriptWithAutoDrive : MonoBehaviour//, IRewardedVideoAdListener 
{

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

	// НОВАЯ ПЕРЕМЕННАЯ: Включает автоматическое движение вправо
	[Header("Auto Drive Settings")]
	public bool autoDriveRight = true;

	Vector2 oldPos = Vector2.zero;
	public int steps = 0;
	int generalDistances = 0;
	public float currentTimeScale;
	public bool isPassed = false;
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

	void Awake()
	{
		Bullet.car = transform;
		Sliver.car = transform;
		//OutLevelHandler.car = this;
	}

	IEnumerator Start()
	{
		//Appodeal.setRewardedVideoCallbacks(this);
		/*if (p_HUD != null) {
			p_HUD.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener ( delegate() {
				this.Moxol();			
			} );
			p_HUD.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener ( () => Brokoli () );
		}/**/

		if (p_HUD != null)
			p_HUD.SetActive(true);

		if (p_LevelComplete != null)
			p_LevelComplete.SetActive(false);


		if (p_HUD)
		{
			var stickman = FindObjectOfType<ComponentMenager>();
			if (stickman)
				numberLevel = stickman.numberLevel;

			Levels.currentExperience[numberLevel] = 0;
		}

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		wheelJoints = gameObject.GetComponents<WheelJoint2D>();
		backWheel = wheelJoints[1].motor;
		frontWheel = wheelJoints[0].motor;
		isGas = false;
		//var myLoadedAssetBundle = AssetBundle.LoadFromFile ("brokol");
		var prefabs = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		for (int i = 0; i < prefabs.Length; i++)
		{
			if (prefabs[i].name == "Blood")
			{
				//print(prefabs[i].name);
			}
		}
		if (customCar)
		{

		}
		else
		{
			posYWheel = transform.GetChild(0).transform.localPosition.y;
			posYWheel2 = transform.GetChild(1).transform.localPosition.y;
			mojno = true;
		}


		yield return null;
		DetectRagdoll.car = transform;
		var fittersEffector = FindObjectsOfType<FitterForceEffector>();
		foreach (FitterForceEffector ffe in fittersEffector)
		{
			ffe.FitForce();
		}
		btnActiveBlood = GameObject.Find("Btn Enable Blood");
	}

	void FixedUpdate()
	{
		float maxDist = transform.name.IndexOf("Tank") > -1 ? 0.39f : 1.1f;

		if (mojno)
		{
			if (transform.GetChild(0).transform.localPosition.y - posYWheel > maxDist || transform.GetChild(0).transform.localPosition.y - posYWheel < -0.5f)
			{
				transform.GetChild(0).transform.localPosition = new Vector3(transform.GetChild(0).transform.localPosition.x, posYWheel, -0.05f);
			}
			if (transform.GetChild(1).transform.localPosition.y - posYWheel2 > maxDist || transform.GetChild(1).transform.localPosition.y - posYWheel2 < -0.5f)
			{
				transform.GetChild(1).transform.localPosition = new Vector3(transform.GetChild(1).transform.localPosition.x, posYWheel2, -0.05f);
			}
		}

		frontWheel.motorSpeed = backWheel.motorSpeed;

		angleCar = transform.localEulerAngles.z;

		if (angleCar >= 180)
		{
			angleCar = angleCar - 360;
		}

		// БЕЗОПАСНАЯ ПРОВЕРКА КНОПОК
		bool btnGasClicked = (ControlCar != null && ControlCar.Length > 0 && ControlCar[0] != null) && ControlCar[0].ClickedIs;
		bool btnBrakeClicked = (ControlCar != null && ControlCar.Length > 1 && ControlCar[1] != null) && ControlCar[1].ClickedIs;

		// Проверяем, нажата ли кнопка газа, ИЛИ включен ли автопилот
		bool gasPressed = autoDriveRight || btnGasClicked;

		if (gasPressed == true)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (acceleration - gravity * Mathf.PI * (angleCar / 180) * 80) * Time.deltaTime, maxSpeed, maxBackSpeed);
		}
		if ((gasPressed == false && backWheel.motorSpeed < 0) || (gasPressed == false && backWheel.motorSpeed == 0 && angleCar < 0))
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (deacceleration - gravity * Mathf.PI * (angleCar / 180) * 80) * Time.deltaTime, maxSpeed, 0);
		}
		else if ((gasPressed == false && backWheel.motorSpeed > 0) || (gasPressed == false && backWheel.motorSpeed == 0 && angleCar > 0))
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - (-deacceleration - gravity * Mathf.PI * (angleCar / 180) * 80) * Time.deltaTime, 0, maxBackSpeed);
		}

		if (btnBrakeClicked == true && backWheel.motorSpeed > 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed - brakeForce * Time.deltaTime, 0, maxBackSpeed);
		}
		else if (btnBrakeClicked == true && backWheel.motorSpeed < 0)
		{
			backWheel.motorSpeed = Mathf.Clamp(backWheel.motorSpeed + brakeForce * Time.deltaTime, maxSpeed, 0);
		}

		wheelJoints[1].motor = backWheel;
		wheelJoints[0].motor = frontWheel;
		if (wheelJoints.Length > 2)
		{
			for (int i = 2; i < wheelJoints.Length; i++)
			{
				wheelJoints[i].motor = backWheel;
			}
		}

		if (gasPressed == true)
		{
			if (isGas == false && Game.currentCoins >= Game.GetPlacedPrice())
			{
				if (p_HUD != null) p_HUD.transform.GetChild(3).gameObject.SetActive(true);
				CheckFire.Check();
			}
			if (!isGas)
			{
				if (p_HUD != null)
				{
					Transform bgSwitch = p_HUD.transform.Find("btn_Switch Background");
					if (bgSwitch != null) bgSwitch.gameObject.SetActive(false);
				}
			}
			isGas = true;

			if (p_HUD != null) p_HUD.transform.GetChild(1).gameObject.SetActive(false);
			if (btnActiveBlood != null) btnActiveBlood.SetActive(false);
		}
	}//________________________________________________________

	IEnumerator checkMove()
	{
		int csoom = 6;
		if (numberLevel == 7)
			csoom = 8;
		yield return new WaitForSeconds(0.5f);
		if (Vector2.Distance(oldPos, transform.position) < 0.15f)
		{
			steps++;
			generalDistances += (int)Vector2.Distance(oldPos, transform.position);
			if (steps >= csoom)
			{
				LevelComplete();
			}
		}
		else
		{
			steps = 0;
		}

		oldPos = transform.position;

		if (Camera.main.GetComponent<SmoothCamera>().target == null)
		{
			Camera.main.GetComponent<SmoothCamera>().target = transform;
		}
		if (steps < csoom)
		{
			StartCoroutine(checkMove());
		}

		if (transform.position.magnitude > 8000)
		{
			LevelComplete();
		}
	}

	public void LevelComplete()
	{
		HUD.tntSelected = false;
		GetComponent<Rigidbody2D>().isKinematic = true;
		isPassed = false;
		numberLevel = Game.GetNumberCurrentLevel();

		// Проверка на наличие p_HUD перед отключением
		if (p_HUD != null)
			p_HUD.SetActive(false);

		// Проверка на наличие p_LevelComplete перед включением
		if (p_LevelComplete != null)
		{
			p_LevelComplete.SetActive(true);

			// Чтобы GameObject.Find не выдавал ошибку, ищем тексты только если панель активна
			GameObject txtAgain = GameObject.Find("txt_Again");
			if (txtAgain != null) txtAgain.GetComponent<Text>().text = Settings.lng.txt_Again;

			GameObject txtMenu = GameObject.Find("txt_Menu");
			if (txtMenu != null) txtMenu.GetComponent<Text>().text = Settings.lng.txt_Menu;

			GameObject txtCompleted = GameObject.Find("txt_Completed");
			if (txtCompleted != null) txtCompleted.GetComponent<Text>().text = Settings.lng.txt_Completed;

			GameObject txtExp = GameObject.Find("txt_Experience");
			if (txtExp != null) txtExp.GetComponent<Text>().text = Settings.lng.txt_Experience;

			GameObject txtMaxExp = GameObject.Find("txt_MaxExperience ");
			if (txtMaxExp != null) txtMaxExp.GetComponent<Text>().text = Settings.lng.txt_Record;

			GameObject txtCoins = GameObject.Find("txt_Coins");
			if (txtCoins != null) txtCoins.GetComponent<Text>().text = Settings.lng.txt_CoinsEarned;

			GameObject txtAllCoins = GameObject.Find("txt_AllCoins");
			if (txtAllCoins != null) txtAllCoins.GetComponent<Text>().text = Settings.lng.txt_Coins;

			if (Levels.currentExperience[numberLevel] > Levels.recordExperience[numberLevel])
			{
				Levels.recordExperience[numberLevel] = Levels.currentExperience[numberLevel];
				if (txtCompleted != null)
				{
					txtCompleted.GetComponent<Text>().text = Settings.lng.txt_NewRecord;
					txtCompleted.GetComponent<Text>().color = new Color(0.1f, 1, 0.1f);
				}
			}

			GameObject txtOutExp = GameObject.Find("txt_OutExperience");
			if (txtOutExp != null) txtOutExp.GetComponent<Text>().text = Levels.currentExperience[numberLevel].ToString();

			GameObject txtOutMaxExp = GameObject.Find("txt_OutMaxExperience");
			if (txtOutMaxExp != null) txtOutMaxExp.GetComponent<Text>().text = Levels.recordExperience[numberLevel].ToString();

			int coins = (int)(Levels.currentExperience[numberLevel] / (38.2f / 10f));
			Game.currentCoins += coins;
			Game.SaveCoins();

			GameObject txtOutCoins = GameObject.Find("txt_OutCoins");
			if (txtOutCoins != null) txtOutCoins.GetComponent<Text>().text = coins.ToString();

			if (txtAllCoins != null) txtAllCoins.GetComponent<Text>().text = Settings.lng.txt_Coins + Game.currentCoins.ToString();
		}
		else
		{
			// Если панели нет, мы всё равно должны начислить монеты и сохранить прогресс
			if (Levels.currentExperience[numberLevel] > Levels.recordExperience[numberLevel])
			{
				Levels.recordExperience[numberLevel] = Levels.currentExperience[numberLevel];
			}
			int coins = (int)(Levels.currentExperience[numberLevel] / (38.2f / 10f));
			Game.currentCoins += coins;
			Game.SaveCoins();
		}

		Levels.SaveData();
		HUD.HelperLevelShop();
	}//________________________________________________________

	public void RunCheck()
	{
		isPassed = true;
		oldPos = transform.position;
		StartCoroutine(checkMove());
	}//________________________________________________________


	public void ShowAdvertise()
	{
		//if(Appodeal.isLoaded(Appodeal.REWARDED_VIDEO)){
		//	Appodeal.show (Appodeal.REWARDED_VIDEO);
		//} else if (Appodeal.isLoaded (Appodeal.INTERSTITIAL)) {
		//	Appodeal.show (Appodeal.INTERSTITIAL);
		//}
		//Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);
		//Appodeal.initialize(AdvertiseService.appKey, Appodeal.REWARDED_VIDEO);
		//Appodeal.cache(Appodeal.REWARDED_VIDEO);
	}


	private void Update()
	{
		// Защита от null для элемента массива в Update
		if (ControlCar != null && ControlCar.Length > 0 && ControlCar[0] != null)
		{
			if (!ControlCar[0].ClikedOnUI)
			{
				ControlCar[0].ClickedIs = Input.GetKey(KeyCode.Space);
			}
		}
	}


	public float GetTotalMass()
	{
		float massTotal = 0;
		if (customCar)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).GetComponent<Rigidbody2D>())
				{
					massTotal += transform.GetChild(i).GetComponent<Rigidbody2D>().mass;
				}
			}
			for (int i = 0; i < transform.parent.childCount; i++)
			{
				massTotal += transform.parent.GetChild(i).GetComponent<Rigidbody2D>().mass;
			}
		}
		else
		{
			massTotal = GetComponent<Rigidbody2D>().mass;
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).GetComponent<Rigidbody2D>())
				{
					massTotal += transform.GetChild(i).GetComponent<Rigidbody2D>().mass;
				}
			}
		}
		return massTotal;
	}

	public void Placed()
	{
		transform.localRotation = Quaternion.Euler(0, 0, 0);

		float massTotal = GetTotalMass();

		if (transform.name.IndexOf("Locker") > -1)
		{
			massTotal *= 3;
		}

		massTotal /= Time.timeScale;

		GetComponent<Rigidbody2D>().AddForce(Vector2.up * massTotal * 300);
		GetComponent<Rigidbody2D>().angularVelocity = 0;
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

	}
}