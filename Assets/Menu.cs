using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject p_MainMenu;// from Inspector
	public GameObject p_Levels;// from Inspector
	public GameObject p_Settings;
	public GameObject p_BuyDialog;// from Inspector
	public GameObject p_BuyTNT;
	public GameObject p_ColorsBones;
	public GameObject p_BuyColorsBoneDialog;

	public GameObject p_Coins;// from Inspector
	public BoneColor boneColor;

	[SerializeField] Button[] btnLevels;
	[SerializeField] Text[] txtCoins = new Text[2];
	[SerializeField] Text txtCountTNT;
	[SerializeField] Text txtVehicleEditor;
	[SerializeField] ItemColorBone[] itemsBoneColor;
	[SerializeField] Animation glowTNTHolder;

	[Space]

	[SerializeField] Button btnQuit;

	public AudioClip[] Motors;
	public static AudioClip[] motors;

	public static string mode = "menu";
	bool firstRunning = true;
	int selectedLevel = 0;
	ItemColorBone currentItemBoneColor;

	void Awake() {
		motors = Motors;
		p_MainMenu.SetActive(true);
		p_Levels.SetActive(false);
		p_BuyDialog.SetActive(false);
		p_Settings.SetActive(false);
		p_BuyTNT.SetActive(false);
		p_ColorsBones.SetActive(false);
		p_BuyColorsBoneDialog.SetActive(false);


		for (int i = 0; i < Levels.countLevels; i++) {
			Levels.isOpen[i] = false;
		}
		Levels.isOpen[0] = true;
		Levels.isOpen[1] = true;
		Levels.prices[1] = 0;
		Levels.prices[2] = 800;
		Levels.prices[3] = 1900;
		Levels.prices[4] = 6000;
		Levels.prices[5] = 8000;
		Levels.prices[6] = 18000;
		Levels.prices[7] = 88000;
		Levels.prices[8] = 98000;
		Levels.prices[9] = 333333;
		Levels.prices[10] = 888888;
		Levels.prices[11] = 1000000;

		//------------------------
		Levels.isOpenAuto[0] = true;
		Levels.pricesAuto[0] = 0;
		Levels.pricesAuto[1] = 300;
		Levels.pricesAuto[2] = 3500;
		Levels.pricesAuto[3] = 7000;
		Levels.pricesAuto[4] = 18000;
		Levels.pricesAuto[5] = 38000;
		Levels.pricesAuto[6] = 100000;
		Levels.pricesAuto[7] = 300000;
		Levels.pricesAuto[8] = 500000;
		Levels.pricesAuto[9] = 700000;
	}

	void OnEnable() {
		if (Application.genuine) {
			if (mode == "menu") {
				p_MainMenu.SetActive(true);
				p_Levels.SetActive(false);
				p_Settings.SetActive(false);
			} else if (mode == "levels") {
				p_MainMenu.SetActive(false);
				p_Levels.SetActive(true);
				p_Settings.SetActive(false);
				ChangeLanguage();
			}
		}
	}

	void Start() {
		Game.LoadCoins();
		Game.LoadCountTNT();
		Levels.LoadData();
		UpdateCoins();
		CheckLevels();
		Settings.SetLan();
		//Settings.current.moreGames = GameObject.Find ("txt_MoreGames").GetComponent<Text> ();
		Settings.Texto();

		if (!SaveLoadSystem.HasKey(SaveLoadSystem.KeyTNTBonus)) {
			TNT.IncreaseTNT(330);
			SaveLoadSystem.SaveInt(SaveLoadSystem.KeyTNTBonus, 1, true);
		}

#if !UNITY_ANDROID
		btnQuit.gameObject.SetActive(false);
#endif
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			Game.currentCoins += 5000;
			UpdateCoins();
			SaveLoadSystem.SaveCoins(Game.currentCoins);
			//SaveLoadSystem.SaveGameData(string.Empty);
			//SaveLoadSystem.SaveAutosData(string.Empty);
			//SaveLoadSystem.Save();
		}

		if (Input.GetKeyDown(KeyCode.D))
		{
			SaveLoadSystem.DeleteAll();
		}
	}

	public void OpenLevels()
	{
		p_Levels.SetActive(true);
		p_MainMenu.SetActive(false);
		p_Settings.SetActive(false);
		ChangeLanguage();
		UpdateCoins();
	}

	public void OpenMainMenu() {
		p_Levels.SetActive(false);
		p_MainMenu.SetActive(true);
		p_Settings.SetActive(false);
		p_BuyTNT.SetActive(false);
	}

	public void OpenSettings() {
		p_Settings.SetActive(true);
		p_Levels.SetActive(false);
		p_MainMenu.SetActive(false);
		p_BuyTNT.SetActive(false);
		Handheld.Vibrate();
	}

	public void OpenShopTNT() {
		txtCountTNT.text = "x" + (int.Parse(Game.firstTNT + Game.secondTNT)).ToString();
		p_BuyTNT.SetActive(true);
		GameObject.Find("txt TNT Shop").GetComponent<Text>().text = Settings.lng.txt_tntShop;
	}

	public void OpenEditorVehicle() {
		SceneManager.LoadScene("Editor Car");
	}

	public void BuyTNT(int countTNT) {
		switch (countTNT) {
			case 1:
				{
					if (Game.currentCoins >= 1000) {
						Game.currentCoins -= 1000;
						TNT.IncreaseTNT(countTNT);
						glowTNTHolder.Stop();
						glowTNTHolder.Play();
					}
					break;
				}
			case 5:
				{
					if (Game.currentCoins >= 5000) {
						Game.currentCoins -= 5000;
						TNT.IncreaseTNT(countTNT);
						glowTNTHolder.Stop();
						glowTNTHolder.Play();
					}
					break;
				}
			case 10:
				{
					if (Game.currentCoins >= 10000) {
						Game.currentCoins -= 10000;
						TNT.IncreaseTNT(countTNT);
						glowTNTHolder.Stop();
						glowTNTHolder.Play();
					}
					break;
				}
		}
		txtCountTNT.text = "x" + (int.Parse(Game.firstTNT + Game.secondTNT)).ToString();
		UpdateCoins();
		Game.SaveCoins();
	}


	public void LoadLevel(int lvl) {
		Levels.isOpen[0] = true;
		Levels.isOpen[1] = true;
		Levels.isOpenAuto[0] = true;
		Time.timeScale = 1;
		if (Levels.isOpen[lvl]) {
			SceneManager.LoadSceneAsync(lvl);
			Time.fixedDeltaTime = 0.025f;//0.03f;
		} else if (Levels.prices[lvl] <= Game.currentCoins) {
			OpenBuyDialog();
			selectedLevel = lvl;
		}
	}

	public void OpenBuyDialog() {
		p_BuyDialog.SetActive(true);
		GameObject.Find("txt-AreYouSure").GetComponent<Text>().text = Settings.lng.txt_AreYouSure;
		GameObject.Find("Text No").GetComponent<Text>().text = Settings.lng.txt_No;
		GameObject.Find("Text Yes").GetComponent<Text>().text = Settings.lng.txt_Yes;
	}

	public void CloseBuyDialog() {
		p_BuyDialog.SetActive(false);
		selectedLevel = 0;
	}

	public void BuyLevel() {
		Levels.isOpen[selectedLevel] = true;
		Game.currentCoins -= Levels.prices[selectedLevel];
		btnLevels[selectedLevel].transform.GetChild(1).gameObject.SetActive(false);
		Levels.SaveData();
		Game.SaveCoins();
		CloseBuyDialog();
		UpdateCoins();
		CheckLevels();
	}

	void CheckLevels() {
		for (int i = 1; i < btnLevels.Length; i++) {
			if (btnLevels[i].transform.childCount > 1) {
				if (!Levels.isOpen[i]) {
					if (Levels.prices[i] <= Game.currentCoins) {
						btnLevels[i].transform.GetChild(1).GetComponent<Image>().color = new Color(0.936f, 0.716f, 0, 0.8f);
						btnLevels[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
						btnLevels[i].GetComponent<Button>().interactable = true;
					} else {
						btnLevels[i].transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);
						btnLevels[i].transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.4f);
						btnLevels[i].GetComponent<Button>().interactable = false;
					}
				} else {
					btnLevels[i].interactable = true;
					btnLevels[i].transform.GetChild(1).gameObject.SetActive(false);
				}
			}
		}
	}

	public void OpenPrivacyPolicy() {
		Application.OpenURL("https://wogergames.github.io/StickmanAnnihilation/");
	}

	public void OrbitJump()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.WogerGames.OrbitJump");
	}

	public void StickmanReaper()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.WogerGames.StickmanReaper");
	}

	public void StickmanAnnihilationII()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.WogerGames.StickmanAnnihilationII");
	}

	public void StickmanPunch()
    {
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.OtherStickmanGames.StickmanPunch");
    }

	#region Color Bones
	public void OpenColorsBone(){
		p_ColorsBones.SetActive (true);
		GameObject.Find ("txt Title Color Bones").GetComponent<Text> ().text = Settings.lng.txt_ColorBones;
		foreach (ItemColorBone item in itemsBoneColor) {
			item.UpdateView ();
		}
	}
		
	public void SelectItemBone(ItemColorBone icb){
		if (boneColor.unlockBones [icb.index]) {
			boneColor.currentColor = boneColor.colorsBone [icb.index];
			foreach (ItemColorBone item in itemsBoneColor) {
				item.UpdateView ();
			}
		} else if (Game.currentCoins >= 10000){
			p_BuyColorsBoneDialog.SetActive (true);
			currentItemBoneColor = icb;
			GameObject.Find ("txt_Are You Sure").GetComponent<Text> ().text = Settings.lng.txt_AreYouSure;
			GameObject.Find ("txt_Cancel").GetComponent<Text> ().text = Settings.lng.txt_cancelEditor;
		}
		boneColor.SaveColor ();
	}

	public void BuyColorBone(){
		boneColor.unlockBones [currentItemBoneColor.index] = true;
		boneColor.currentColor = boneColor.colorsBone [currentItemBoneColor.index];
		foreach (ItemColorBone item in itemsBoneColor) {
			item.UpdateView ();
		}
		p_BuyColorsBoneDialog.SetActive (false);
		Game.currentCoins -= 10000;
		Game.SaveCoins ();
		UpdateCoins ();
		boneColor.SaveColor ();
	}

	public void CancelBuyColorBone(){
		p_BuyColorsBoneDialog.SetActive (false);
	}
	#endregion

	void UpdateCoins()
	{
		var coinsStr = $"{Settings.lng.txt_Coins}{Game.currentCoins}";
		txtCoins[0].text = coinsStr;
		txtCoins[1].text = coinsStr;
	}

	public void StickmanCarnage(){
		Application.OpenURL ("https://play.google.com/store/apps/details?id=com.OtherStickmanGames.StickmanSlice");
	}


	void ChangeLanguage(){
		GameObject.Find ("txt_Straight").GetComponent<Text> ().text = Settings.lng.map_Straight;
		GameObject.Find ("txt_HiStk").GetComponent<Text> ().text = Settings.lng.map_HiStk;
		GameObject.Find ("txt_UpDown").GetComponent<Text> ().text = Settings.lng.map_UpDown;
		GameObject.Find ("txt_Wave").GetComponent<Text> ().text = Settings.lng.map_Wave;
		GameObject.Find ("txt_SecretRace").GetComponent<Text> ().text = Settings.lng.map_SecretRace;
		GameObject.Find ("txt_Marionetka").GetComponent<Text> ().text = Settings.lng.map_Marionetka;
		GameObject.Find ("txt_Petla").GetComponent<Text> ().text = Settings.lng.map_Petla;
		GameObject.Find ("txt_Jevalo").GetComponent<Text> ().text = Settings.lng.map_Jevalo;
		GameObject.Find ("txt_ArcDead").GetComponent<Text> ().text = Settings.lng.map_ArcDead;
		GameObject.Find ("txt_ThreePaths").GetComponent<Text> ().text = Settings.lng.map_ThreePaths;
		GameObject.Find ("txt_HardRoad").GetComponent<Text> ().text = Settings.lng.map_HardRoad;
		txtVehicleEditor.text = Settings.lng.txt_VehicleEditor;

	}

	public void Quit(){
		Application.Quit ();
	}
}
