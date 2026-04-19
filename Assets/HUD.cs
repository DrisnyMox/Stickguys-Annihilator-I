using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;
using mixpanel;

public class HUD : MonoBehaviour {

	public GameObject p_Autos;// from Inspector
	public GameObject p_BuyDialog;//from Inspector
	[SerializeField] GameObject btnRestart;

	public GameObject[] cars;// prefab
	public static List<GameObject> carsCustom = new List<GameObject>();
	public static List<Sprite> imagesCarsCustom = new List<Sprite>();
	public static List<string> titleCarsCustom = new List<string>();

	public ClickScript crClickScript1;
	public ClickScript crClickScript2;
	public GameObject p_LevelComplite;
	public Transform p_Pause;
	public GameObject p_HUD;
	[HideInInspector] public GenerateButtonsCars generateButtonsCars;
	[Header("Ссылки на объекты")]
	[SerializeField] GameObject tnt;
	[SerializeField] Transform gridCars;
	[SerializeField] GameObject p_Confirmation;
	[SerializeField] GameObject p_ConfirmEditor;
	[SerializeField] Text txtPricePlacedCar;
	[SerializeField] BoneColor boneColor;
	[SerializeField] GameObject btnSwitchBackground;
	[Space]
	[SerializeField] Button btnOpenEditor;

	GameObject car;
	BackgroundsKeeper backKeeper;


	public static int idAuto = 0;
	public static int idAutoCustom = 0;
	public static bool tntSelected;
	public static bool currentCarIsCustom = false;
	public static Coroutine cor = null;
	public static BoneColor sBoneColor;

	int numberLevel;
	int selectedAuto = -1;
	int SelectedAutoCustom = -1;

	bool enablePlaced = true;

	void Awake(){
        try
        {
            Levels.LoadData();
            Game.LoadCoins();
            sBoneColor = boneColor;
            //DebugLog.Add("MOOOOOX.... SIIIIIIDRRR");
        } catch(System.Exception e)
        {
            DebugLog.Add(e.ToString());
        }

		btnOpenEditor.onClick.AddListener(OpenEditorCar_Clicked);
	}

    IEnumerator Start()
	{
		tntSelected = false;
		Game.LoadGears();
		Levels.LoadEditor();


		p_Confirmation.SetActive(false);
		p_ConfirmEditor.SetActive(false);
		p_BuyDialog.SetActive(false);
		p_Pause.gameObject.SetActive(false);

		GameObject.Find("txt Gas").GetComponent<Text>().text = Settings.lng.txt_Gas;
		GameObject.Find("txt Break").GetComponent<Text>().text = Settings.lng.txt_Break;
		var txtBlood = GameObject.Find("txt Blood")?.GetComponent<Text>();
		if (txtBlood)
		{
			txtBlood.text = Settings.lng.txt_Blood;
		}

		numberLevel = Game.GetNumberCurrentLevel();//GameObject.Find ("Ragdoll Pafos").GetComponent<ComponentMenager> ().numberLevel;
		car = GameObject.Find("Car");
		if (currentCarIsCustom)
		{
			Destroy(car);
			car = Instantiate(carsCustom[idAutoCustom]);
			car.SetActive(true);
			car.transform.GetChild(0).GetComponent<CarScript>().ControlCar[0] = crClickScript1;
			car.transform.GetChild(0).GetComponent<CarScript>().ControlCar[1] = crClickScript2;
			car.transform.GetChild(0).GetComponent<CarScript>().p_HUD = p_HUD;
			car.transform.GetChild(0).GetComponent<CarScript>().p_LevelComplete = p_LevelComplite;
			car.transform.GetChild(0).GetComponent<CarScript>().customCar = true;

			Camera.main.GetComponent<SmoothCamera>().target = car.transform.GetChild(0);
		}
		else if (idAuto != 0)
		{
			Destroy(car);
			car = Instantiate(cars[idAuto]);
			car.GetComponent<CarScript>().ControlCar[0] = crClickScript1;
			car.GetComponent<CarScript>().ControlCar[1] = crClickScript2;
			car.GetComponent<CarScript>().p_HUD = p_HUD;
			car.GetComponent<CarScript>().p_LevelComplete = p_LevelComplite;

			Camera.main.GetComponent<SmoothCamera>().target = car.transform;
			DebugLog.Add("iffffffffffff");
		}

		HelperShop();
		CheckAuto();
		UpdateTNTView();

		var ui = GameObject.Find("UI").transform;

		if (!SaveLoadSystem.HasKey(SaveLoadSystem.KeyTooltipTNT))
		{
			//ui.GetChild(ui.childCount - 2).gameObject.SetActive(true);
			var txtTooltipTNT = GameObject.Find("txt_TooltipTNT")?.GetComponent<Text>();
			if (txtTooltipTNT)
			{
				txtTooltipTNT.text = Settings.lng.txt_TooltipTNT;
			}
			SaveLoadSystem.SaveString(SaveLoadSystem.KeyTooltipTNT, "showed", true);
		}
		if (SaveLoadSystem.HasKey(SaveLoadSystem.KeyChmos))
		{
			SaveLoadSystem.DeleteKey(SaveLoadSystem.KeyChmos);
		}
		GameObject txtSlow = GameObject.Find("txt_Slow");
		if (txtSlow)
			txtSlow.GetComponent<Text>().text = Settings.lng.txt_Slow;
		GameObject txtActivate = GameObject.Find("txt_Activate");
		if (txtActivate)
			txtActivate.GetComponent<Text>().text = Settings.lng.txt_Activate;
		txtPricePlacedCar.text = $"{Game.GetPlacedPrice()} {Settings.lng.txt_PlacedCar}";
		if (Game.GetPlacedPrice() == 0)
		{
			txtPricePlacedCar.text = "Вниз колесами";
		}
		backKeeper = FindObjectOfType<BackgroundsKeeper>();
		if (backKeeper)
		{
			backKeeper.GetComponentInChildren<Image>().sprite = backKeeper.backgrounds[0];
			btnSwitchBackground.SetActive(true);
		}
		else
			btnSwitchBackground.SetActive(false);

		
		var txtExp = ui.GetChild(ui.childCount - 1).GetComponent<Text>();
		txtExp.text = $"{Settings.lng.txt_ExpShort} {Levels.currentExperience[numberLevel]}";

		yield return null;

		p_Autos.SetActive(false);
		CheckFire.Check();
		Mixpanel.Track($"StartLevel:{numberLevel}");
		yield return null;
		Mixpanel.Track($"SelectCar:{idAuto}");
		
	}//_________________________________________________________________________________

	void UpdateTNTView()
	{
        GameObject.Find("btn_TNT").transform.GetChild(1).GetComponent<Text>().text = $"{(int.Parse(Game.firstTNT + Game.secondTNT))}";
    }

    public void xyishe(){
		SaveLoadSystem.DeleteKey (SaveLoadSystem.KeyEditor);
	}
	public void xyishe2(){
		Game.currentCoins += 8888888;
		Game.SaveCoins ();
		UpdateCoins ();
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.L)) {
			SaveLoadSystem.DeleteKey (SaveLoadSystem.KeyTooltipTNT);
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			Game.gears += 88;
			Game.SaveGears ();
		}
		if (Input.GetKeyDown (KeyCode.U)) {
			SaveLoadSystem.DeleteKey (SaveLoadSystem.KeyGears);
		}
		if (Input.GetKeyDown (KeyCode.F9)) {
			SaveLoadSystem.DeleteKey (SaveLoadSystem.KeyGameAuto);
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			SaveLoadSystem.DeleteKey (SaveLoadSystem.KeyEditor);
		}
		if(Input.GetKeyDown(KeyCode.Alpha0)){
			Game.currentCoins += 888888;
			Game.SaveCoins ();
			UpdateCoins ();
		}
		if (Input.GetMouseButtonDown (0) && tntSelected && AvailableTNT()) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);

			if (!IsPointerOverUIObject () ) {
				Vector3 posTNT = Camera.main.ScreenToWorldPoint (Input.mousePosition) + new Vector3(0,0,10);
				Instantiate (tnt, posTNT, Quaternion.identity);	
				TNT.DecreaseTNT ();
			}
		}
		if (Input.GetKeyDown (KeyCode.F)) {
			Fire ();
		}
	}//_________________________________________________________________________________

	public void SelectAuto(int id)
	{
		selectedAuto = id;
		if (Levels.isOpenAuto[id])
		{
			SpawnAuto(id);
		}
		else
		{
			OpenBuyDialog();
		}
	}

	public void SelectAutoCustom(int id){
		SelectedAutoCustom = id;
		SpawnAutoCustom (id);
	}

	void SpawnAuto(int id)
	{
        Destroy(car);

        GameObject spawnedCar = Instantiate(cars[id]);
		DebugLog.Add(spawnedCar.ToString() + " -- spawn");

		spawnedCar.GetComponent<CarScript>().ControlCar[0] = crClickScript1;
		spawnedCar.GetComponent<CarScript>().ControlCar[1] = crClickScript2;
		spawnedCar.GetComponent<CarScript>().p_HUD = p_HUD;
		spawnedCar.GetComponent<CarScript>().p_LevelComplete = p_LevelComplite;

		Camera.main.GetComponent<SmoothCamera>().target = spawnedCar.transform;
		CloseAutos();
		//DebugLog.Add(spawnedCar.name + "-------");
		//DebugLog.Add(car.name + "+++++++++");
		
		car = spawnedCar;
		DebugLog.Add(car.name);
		idAuto = id;
		currentCarIsCustom = false;
		CheckFire.Check();

	}

	void SpawnAutoCustom(int id){
		GameObject spawnedCar = Instantiate (carsCustom [id]);
		spawnedCar.SetActive (true);
		spawnedCar.GetComponentInChildren<CarScript> ().ControlCar [0] = crClickScript1;
		spawnedCar.GetComponentInChildren<CarScript> ().ControlCar [1] = crClickScript2;
		spawnedCar.GetComponentInChildren<CarScript> ().p_HUD = p_HUD;
		spawnedCar.GetComponentInChildren<CarScript> ().p_LevelComplete = p_LevelComplite;
		if (spawnedCar.GetComponentInChildren<CarScript> ().customCar) {
			Camera.main.GetComponent<SmoothCamera> ().target = spawnedCar.transform.GetChild (0);
		} else {
			Camera.main.GetComponent<SmoothCamera> ().target = spawnedCar.transform;
		}
		CloseAutos ();
		Destroy (car);
		car = spawnedCar;
		idAutoCustom = id;
		currentCarIsCustom = true;
		CheckFire.Check ();
	}

	public void OpenAutos() {
		p_Autos.SetActive (true);

		var scrollbar = p_Autos.transform.Find("Scrollbar");
		scrollbar.GetComponent<Scrollbar>().value = 1;

		AdvertiseService.ShowAdmobBottom ();
		UpdateCoins ();
		ChangeLanguage ();
	}

	public void CloseAutos() {
		selectedAuto = -1;
		p_Autos.SetActive (false);
		btnRestart.SetActive (true);
        DebugLog.Add("pered hide admob");
        AdvertiseService.HideAdmobBottom ();
        DebugLog.Add("posle hide admob");
    }

	public void OpenBuyDialog(){
		p_BuyDialog.SetActive (true);
        GameObject.Find("txt-AreYouSure").GetComponent<Text>().text = Settings.lng.txt_AreYouSure;
        GameObject.Find("Text No").GetComponent<Text>().text = Settings.lng.txt_No;
        GameObject.Find("Text Yes").GetComponent<Text>().text = Settings.lng.txt_Yes;
    }

	public void CloseByuDialog(){
		selectedAuto = -1;
		p_BuyDialog.SetActive (false);
	}

	public void BuyAuto(){
		Levels.isOpenAuto [selectedAuto] = true;
		Game.currentCoins -= Levels.pricesAuto [selectedAuto];
		p_Autos.transform.GetChild(0).GetChild(0).GetChild (selectedAuto).GetChild (1).gameObject.SetActive (false);
		Levels.SaveData ();
		Game.SaveCoins ();
		UpdateCoins ();
		CloseByuDialog ();
		CheckAuto ();
		HelperShop ();
	}

	void UpdateCoins(){
		int id = p_Autos.transform.childCount - 1;
		p_Autos.transform.GetChild(id).GetChild(0).GetComponent<Text>().text = $"{Settings.lng.txt_Coins} {Game.currentCoins}";
	}

	//=====================================================================================
	public void OpenPause(){
		CarScript crScrpt = (CarScript)FindObjectOfType (typeof(CarScript));
		crScrpt.currentTimeScale = Time.timeScale;
		Time.timeScale = 0;
		p_HUD.SetActive (false);
		p_Pause.gameObject.SetActive (true);
		GameObject.Find ("txt_Pause").GetComponent<Text> ().text = Settings.lng.txt_Pause;
		GameObject.Find ("txt_Pause (1)").GetComponent<Text> ().text = Settings.lng.txt_Pause;
		GameObject.Find ("txt_Resume").GetComponent<Text> ().text = Settings.lng.txt_Resume;
		GameObject.Find ("txt_Menu").GetComponent<Text> ().text = Settings.lng.txt_Menu;
	}


	private void OpenEditorCar_Clicked()
	{
		OpenEditorCar();
	}

	public void OpenEditorCar(){

		if (SaveLoadSystem.HasKey (SaveLoadSystem.KeySkidko)) {
			SaveLoadSystem.DeleteKey (SaveLoadSystem.KeySkidko);
		}
			
		//if (Levels.editorIsOpen) {
		EditorCar.numberLevel = Game.GetNumberCurrentLevel ();
		AdvertiseService.HideAdmobBottom ();
		SceneManager.LoadScene ("Editor Car");
		Time.timeScale = 0;

		/*} else if(Levels.priceEditor <= Game.currentCoins) {
			cor = StartCoroutine ( WTF.WaitAction( cor ) );
			yield return cor;
			Game.currentCoins -= 1500000;
			Game.SaveCoins ();
			UpdateCoins ();
			Levels.editorIsOpen = true;
			Levels.SaveEditor ();
			GameObject.Find ("btn_Add Car").transform.GetChild (1).gameObject.SetActive (false);
		}/**/

		//yield return null;
	}

	/*IEnumerator RunCor(){
		yield return cor = StartCoroutine (WTF.WaitAction ());
	}/**/

	public void ClosePause(){
		CarScript crScrpt = (CarScript)FindObjectOfType (typeof(CarScript));
		Time.timeScale = crScrpt.currentTimeScale;
		p_HUD.SetActive (true);
		p_Pause.gameObject.SetActive (false);
	}
	public void LoadMenu(){
		
		Menu.mode = "levels";
		Levels.currentExperience[numberLevel] = 0;
		Camera.main.GetComponent<Restart> ().LoadMenu ();
		Game.countShowAdvertise++;
		if (Game.countShowAdvertise >= 3) {
			CarScript crScrpt = (CarScript)FindObjectOfType (typeof(CarScript));
			crScrpt.ShowAdvertise ();
			Game.countShowAdvertise = 0;
		}
	}

	public void Restart()
	{
		var outLevelHandler = FindObjectOfType<OutLevelHandler>();
		if (outLevelHandler)
		{
			outLevelHandler.enabled = false;
		}

        numberLevel = Game.GetNumberCurrentLevel();
		Levels.currentExperience[numberLevel] = 0;
		Camera.main.GetComponent<Restart>().Reload(numberLevel);
		CarScript crScrpt;
		crScrpt = (CarScript)FindObjectOfType(typeof(CarScript));
		crScrpt.steps = 0;
		Game.countShowAdvertise++;
		if (Game.countShowAdvertise >= 3)
		{
			crScrpt.ShowAdvertise();
			Game.countShowAdvertise = 0;
		}
	}

	void CheckAuto(){
		for (int i = 0; i < Levels.countAutos; i++) {
			if (gridCars.GetChild (i).childCount > 1) {
				if (!Levels.isOpenAuto [i]) {
					if (Game.currentCoins <= Levels.pricesAuto [i]) {
						gridCars.GetChild (i).GetComponent<Button> ().interactable = false;
						gridCars.GetChild (i).GetChild (1).GetComponent<Image> ().color = new Color (0.54f, 0.54f, 0.54f, 0.8f);
					} else {
						gridCars.GetChild (i).GetComponent<Button> ().interactable = true;
						gridCars.GetChild (i).GetChild (1).GetComponent<Image> ().color = new Color (0.936f, 0.716f, 0, 0.8f);
					}
				} else {
					gridCars.GetChild (i).GetChild (1).gameObject.SetActive (false);
				}
			}
		}
	}//___________________________________________________

	bool IsPointerOverUIObject(){
		PointerEventData eventDataCurrentPosition = new PointerEventData (EventSystem.current);
		eventDataCurrentPosition.position = Input.mousePosition;
		List<RaycastResult> results = new List<RaycastResult> ();
		EventSystem.current.RaycastAll (eventDataCurrentPosition, results);

		foreach (var r in results) {
			//print (r.gameObject);
			if (r.gameObject.layer == 5) {
				return true;
			}
		}
		return false;
	}

	void HelperShop(){
		Transform btnAuto = GameObject.Find ("btn_SelecTS").transform;
		int countAvailable = 0;
		for (int i = 0; i < Levels.countAutos; i++) {
			if (Levels.isOpenAuto [i] == false && Levels.pricesAuto [i] <= Game.currentCoins) 
				countAvailable++;
		}
		if (countAvailable > 0) {
			btnAuto.GetChild (0).GetChild (0).GetComponent<Text> ().text = countAvailable.ToString ();
		} else
			btnAuto.GetChild(0).gameObject.SetActive (false);
	}

	public static void HelperLevelShop(){
		Transform btnMenu = GameObject.Find ("btn_Menu").transform;
		int countAvailable = 0;
		for (int i = 0; i < 11; i++) {//=================================================== TODO количество уровней
			if (Levels.isOpen [i] == false && Levels.prices[i] <= Game.currentCoins) 
				countAvailable++;
		}
		if (countAvailable > 0) {
			btnMenu.GetChild (0).GetChild (0).GetComponent<Text> ().text = countAvailable.ToString ();
		} else
			btnMenu.GetChild(0).gameObject.SetActive (false);
	}

	void Initialize(){
		p_Autos = GameObject.Find ("p_Autos");
	}

	public void PlacedCar(){
		int pricePlaced = Game.GetPlacedPrice();
		if (Game.currentCoins > pricePlaced) {
			Game.currentCoins -= pricePlaced;
			Game.SaveCoins ();
			if (enablePlaced) {
				CarScript cs = FindObjectOfType<CarScript>();
				cs.Placed ();
				StartCoroutine (PausePres ());
			}
		}
	}

	IEnumerator PausePres(){
		enablePlaced = false;
		p_HUD.transform.GetChild (3).gameObject.SetActive (false);
		yield return new WaitForSeconds (3f);
		enablePlaced = true;
		p_HUD.transform.GetChild (3).gameObject.SetActive (true);
	}

	bool AvailableTNT(){
		if (int.Parse (Game.firstTNT + Game.secondTNT) > 0)
			return true;
		else {
			tntSelected = false;
			GetComponent<UseTNT> ().ControlDisplay ();
			return false;
		}
	}

	public void BtnDeleteCar(int id){
		p_Confirmation.SetActive (true);
		GameObject.Find ("txt_YouSure").GetComponent<Text> ().text = Settings.lng.txt_AreYouSure;
		GameObject.Find ("txt_Yes").GetComponent<Text> ().text = Settings.lng.txt_Yes;
		GameObject.Find ("txt_No").GetComponent<Text> ().text = Settings.lng.txt_No;
		p_Confirmation.transform.GetChild (1).GetComponent<Button> ().onClick.RemoveAllListeners ();
		p_Confirmation.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (delegate() {
			DeleteCar (id);
		});
	}

	public void DeleteCar(int id){
		//print (Game.gears);
		Game.gears += HUD.carsCustom [id].transform.GetChild (0).GetComponent<CarScript> ().usedGears / 2;
		Game.SaveGears ();
		Serialization.DeleteCar (id);
		GameObject b = generateButtonsCars.btnsCustomCars [id];
		generateButtonsCars.btnsCustomCars.Remove (b);
		Destroy (b);
		if (idAutoCustom >= carsCustom.Count-1) {
			idAutoCustom = carsCustom.Count - 1;
		}
		float countCars = HUD.carsCustom.Count;
		//print ("количество машин "+countCars);
		if (countCars > 0) {
			for (int i = 0; i < countCars; i++) {
				int n = i;
				generateButtonsCars.btnsCustomCars [i].GetComponent<Button> ().onClick.RemoveAllListeners ();
				generateButtonsCars.btnsCustomCars[i].GetComponent<Button> ().onClick.AddListener (delegate() {
					SelectAutoCustom(n);
				} );
				generateButtonsCars.btnsCustomCars [i].transform.GetChild (1).GetComponent<Button> ().onClick.RemoveAllListeners ();
				generateButtonsCars.btnsCustomCars[i].transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (delegate() {
					BtnDeleteCar (n);
				} );
			}
		}
		p_Confirmation.SetActive (false);
	}

	#region Переклюения ЗАДника
	public void SwitchBackground(){
		int i;
		for (i = 0; i < backKeeper.backgrounds.Length; i++) {
			if (backKeeper.backgrounds [i] == backKeeper.GetComponentInChildren<Image> ().sprite) {
				i++;
				if (i == backKeeper.backgrounds.Length)
					i = 0;
				backKeeper.GetComponentInChildren<Image> ().sprite = backKeeper.backgrounds [i];
			}
		}
	}
	#endregion

	#region Огонь нах
	public void Fire(){
		Gun[] g = (Gun[])FindObjectsOfType (typeof(Gun));
		foreach (Gun gun in g) {
			StartCoroutine (gun.Fire ());
		}
	}
	#endregion

	void ChangeLanguage(){
		GameObject.Find ("txt_Log").GetComponent<Text> ().text = Settings.lng.car_Log;
		GameObject.Find ("txt_Locker").GetComponent<Text> ().text = Settings.lng.car_Locker;
		GameObject.Find ("txt_SportCar").GetComponent<Text> ().text = Settings.lng.car_SpCar;
		GameObject.Find ("txt_Crusher").GetComponent<Text> ().text = Settings.lng.car_Crusher;
		GameObject.Find ("txt_Tank").GetComponent<Text> ().text = Settings.lng.car_Tank;
		GameObject.Find ("txt_Sartir").GetComponent<Text> ().text = Settings.lng.car_Sartir;
		GameObject.Find ("txt_Mocik").GetComponent<Text> ().text = Settings.lng.car_Mocik;
		GameObject.Find ("txt_Belaz").GetComponent<Text> ().text = Settings.lng.car_Belaz;
		GameObject.Find ("txt_BunkBed").GetComponent<Text> ().text = Settings.lng.car_BunkBed;
		GameObject.Find ("txt_Fridge").GetComponent<Text> ().text = Settings.lng.car_Fridge;
		GameObject.Find ("txt_Editor").GetComponent<Text> ().text = Settings.lng.txt_Editor;
	}
		
}
