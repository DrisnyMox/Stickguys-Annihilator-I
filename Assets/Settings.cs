using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Settings : MonoBehaviour {

	public static bool slowMo = true;
	public static float amountBlood = 88;
	public static float distanceCamera = 7f;
	public static int language = 1;
	public string texto = "";
	public Text moreGames;
	public static Language lng;
	Menu menu;
	public Sprite[] btnLanguage;
	public static Settings current;

	public GameObject[] textos;

	void Awake(){
		current = this;
		LoadDistnace ();
		LoadBlood ();
		LoadLanguage ();
		LoadSlowMo ();
		PoolManager.amountBlood = (int)amountBlood;
	}
	// Use this for initialization
	void Start () {
		menu = Camera.main.GetComponent<Menu> ();
		menu.p_Settings.transform.Find ("Slider Blood").GetComponent<Slider> ().value = PoolManager.amountBlood;
		menu.p_Settings.transform.Find ("Toggle SlowMo").GetComponent<Toggle> ().isOn = slowMo;
		menu.p_Settings.transform.Find ("Slider Distance").GetComponent<Slider> ().value = distanceCamera;
		menu.p_Settings.transform.Find ("Btn Language").GetComponent<Image> ().sprite = btnLanguage [language];
		ChangeLanguage ();

	}

	void ChangeLanguageM(){
		if (gameObject.activeInHierarchy) {
			GameObject.Find ("txt_Slowmo").GetComponent<Text> ().text = lng.txt_SlowMo;
			GameObject.Find ("txt_Blood").GetComponent<Text> ().text = lng.txt_CountBlood;
			GameObject.Find ("txt_CamDist").GetComponent<Text> ().text = lng.txt_DistanceCam;
			GameObject.Find ("txt_Language").GetComponent<Text> ().text = lng.txt_Language;
		}
	}
	
	public static void SaveSlowMo(){
		SaveLoadSystem.SaveSlowMo(slowMo);
	}
	public static void SaveDistance(){
		SaveLoadSystem.SaveDistance(distanceCamera);
	}
	public static void SaveBlood(){
		SaveLoadSystem.SaveBlood(PoolManager.amountBlood);
	}
	public static void SaveLanguage(){
		SaveLoadSystem.SaveLanguage(language);
	}
	public static void LoadLanguage(){
		language = SaveLoadSystem.LoadLanguage(language);
	}
	public static void LoadBlood(){
		amountBlood = SaveLoadSystem.LoadBlood(amountBlood);
	}
	public static void LoadDistnace(){
		distanceCamera = SaveLoadSystem.LoadDistance(distanceCamera);
	}
	public static void LoadSlowMo(){
		slowMo = SaveLoadSystem.LoadSlowMo(slowMo);
	}

	public static void SetLan () {
		current.ChangeLanguage ();
		SaveLanguage ();
	}

	public void SetDistanceCamera(float dist){
		distanceCamera = dist;
		SaveDistance ();
	}

	public void SetSlowMo(bool enable){
		slowMo = enable;
		SaveSlowMo ();
	}

	public void SetAmountBlood(float amount){
		PoolManager.amountBlood = (int)amount;
		PoolManager.current.GenerateBlood ();
		SaveBlood ();
	}

	public void SetLanguage(){
		if (language == 0) {
			language = 1;
		} else if(language == 1){
			language = 0;
		}
		GameObject.Find ("Btn Language").GetComponent<Image> ().sprite = btnLanguage [language];
		ChangeLanguage ();
		SaveLanguage ();
	}

	void ChangeLanguage(){
		if (language == 0) {
			lng.txt_Language = "Язык";
			lng.txt_DistanceCam = "Дальность обзора";
			lng.txt_CountBlood = "Кол-во крови";
			lng.txt_SlowMo = "Замедление";
			lng.txt_Gas = "Газ";
			lng.txt_Break = "Тормоз";
			lng.txt_Menu = "МЕНЮ";
			lng.txt_Again = "ЗАНОВО";
			lng.txt_Pause = "Пауза...";
			lng.txt_Resume = "Продолжить";
			lng.txt_VehicleEditor = "Редактор Транспорта";
			lng.txt_Used = "ИСПОЛЬЗУЕТСЯ";
			lng.txt_ColorBones = "Цвет Костей";
			lng.txt_MoreGames = "Больше Игр";
			// ну пох, КАРТЫ
			lng.map_Straight = "Прямая";
			lng.map_HiStk = "Чернослив";
			lng.map_UpDown = "Вниз и вверх";
			lng.map_Wave = "Песочные волны";
			lng.map_SecretRace = "Тайный проезд";
			lng.map_Marionetka = "Марионетка";
			lng.map_Petla = "Петля";
			lng.map_Jevalo = "Жевало";
			lng.map_ArcDead = "Дуга Смерти";
			lng.map_ThreePaths = "Три Пути";
			lng.map_HardRoad = "Трудный Путь";
			// завершение уровня
			lng.txt_Completed = "-= ЗАВЕРШЕНО =-";
			lng.txt_Coins = "Монеты: ";
			lng.txt_Experience = "Опыт";
			lng.txt_Record = "Рекорд опыта";
			lng.txt_CoinsEarned = "Монет заработано";
			lng.txt_PlacedCar = "Монет";
			lng.txt_Slow = "Медленнее";
			lng.txt_Activate = "Активируй Рычаг";
			lng.txt_TooltipTNT = "Добавлен TNT !\nЧтобы им воспользоваться нажмите сначала на иконку, а потом нажмите на экране по месту, которое хотите взорвать :) TNT пополняется после просмотров рекламы.";
			lng.txt_DescrReward = "Поздравляем! Вы получили награду:";
			//тачки ёба
			lng.car_Log = "Бревно";
			lng.car_Locker = "Тумбочка";
			lng.car_SpCar = "Сп.Машина";
			lng.car_Crusher = "Дробилка";
			lng.car_Tank = "Танк";
			lng.car_Sartir = "Толкан";
			lng.car_Mocik = "Мотик";
			lng.car_Belaz = "БЕЛАЗ";
			lng.car_BunkBed = "Кровать";
			lng.car_Fridge = "Холодильник";
			// Эдитор такоёбов
			lng.txt_cancelEditor = "Отмена";
			lng.txt_saveEditor = "Сохранить";
			lng.txt_NoEditor = "Нет";
			lng.txt_YesEditor = "Да";
			lng.txt_SureEditor = "Вы уверены ?";
			lng.txt_BackEditor = "Назад";
			lng.txt_Editor = "Редактор";

		}

		if (language == 1) {
			lng.txt_Language = "Language";
			lng.txt_DistanceCam = "Camera Distance";
			lng.txt_CountBlood = "Blood Amount";
			lng.txt_SlowMo = "Slow Motion";
			lng.txt_Gas = "Gas";
			lng.txt_Break = "Brake";
			lng.txt_Menu = "MENU";
			lng.txt_Again = "AGAIN";
			lng.txt_Pause = "Pause...";
			lng.txt_Resume = "Resume";
			lng.txt_VehicleEditor = "Vehicle Editor";
			lng.txt_Used = "USED";
			lng.txt_ColorBones = "Color Bones";
			lng.txt_MoreGames = "More Games";
			// ну пох, КАРТЫ
			lng.map_Straight = "Straight";
			lng.map_HiStk = "Hi Stickman";
			lng.map_UpDown = "Down & Up";
			lng.map_Wave = "Sendy Wave";
			lng.map_SecretRace = "Secret Way";
			lng.map_Marionetka = "Marionette";
			lng.map_Petla = "Loop";
			lng.map_Jevalo = "Chewed";
			lng.map_ArcDead = "Arc of Dead";
			lng.map_ThreePaths = "Three Paths";
			lng.map_HardRoad = "Hard Road";
			// завершение уровня
			lng.txt_Completed = "-= COMPLETED =-";
			lng.txt_Coins = "Coins: ";
			lng.txt_Experience = "Experience";
			lng.txt_Record = "Max Experience";
			lng.txt_CoinsEarned = "Coins Earned";
			lng.txt_PlacedCar = "Coins";
			lng.txt_Slow = "Slow";
			lng.txt_Activate = "Activate Lever";
			lng.txt_TooltipTNT = "Added TNT !\nClick first on the icon below to use, and then click on the screen at the place you want to blow up :) You get more TNT after viewing the advertisement.";
			lng.txt_DescrReward = "Congratulations! You received the award:";
			//тачки ёба
			lng.car_Log = "Log";
			lng.car_Locker = "Locker";
			lng.car_SpCar = "Sp.Car";
			lng.car_Crusher = "Crusher";
			lng.car_Tank = "Tank";
			lng.car_Sartir = "Crazy Toilet";
			lng.car_Mocik = "Bike";
			lng.car_Belaz = "BelAZ";
			lng.car_BunkBed = "Bunk Bed";
			lng.car_Fridge = "Fridge";
			// Эдитор такоёбов
			lng.txt_cancelEditor = "Cancel";
			lng.txt_saveEditor = "Save";
			lng.txt_NoEditor = "No";
			lng.txt_YesEditor = "Yes";
			lng.txt_SureEditor = "Are you sure ?";
			lng.txt_BackEditor = "Back";
			lng.txt_Editor = "Editor";
		}
		ChangeLanguageM ();
		if (GameObject.Find ("txt_MoreGames")) {
			//Settings.current.moreGames = GameObject.Find ("txt_MoreGames").GetComponent<Text> ();
		}
		Texto ();

	}

	public static void Texto(){
		if (language == 0) {
			current.texto = "Это бета версия 0.9.8.8 Мы любим цифру 8 поэтому в этом обновлении было сделано очень много исправлений и добавлений. Кроме новой карты, мы так же сделали редактор бесплатным, TNT можно покупать за монетки, добавлен Рентген и разный цвет костей, стоимость выравнивания машины теперь зависит от карты. Огромное вам спасибо за то что были с нами.";
		} else {
			current.texto = "This is beta version 0.9.8.8 We love figure 8 so a lot of fixes and additions have been made in this update. In addition to the new map, we also made the editor free, TNT can be bought for coins, added x-Rays and different color of the bones, the cost of leveling the vehicle now depends on the map. Thank you so much for being with us.";
		}
		current.textos [0].GetComponent<Text>().text = current.texto;
		//current.moreGames.text = Settings.lng.txt_MoreGames;
	}
}

public struct Language {
	public string txt_Language;
	public string txt_DistanceCam;
	public string txt_CountBlood;
	public string txt_SlowMo;
	public string txt_Gas;
	public string txt_Break;
	public string txt_Menu;
	public string txt_Again;
	public string txt_Pause;
	public string txt_Resume;
	public string txt_VehicleEditor;
	public string txt_Used;
	public string txt_ColorBones;
	public string txt_MoreGames;
	// Краты, похуй
	public string map_Straight;
	public string map_HiStk;
	public string map_UpDown;
	public string map_Wave;
	public string map_SecretRace;
	public string map_Marionetka;
	public string map_Petla;
	public string map_Jevalo;
	public string map_ArcDead;
	public string map_ThreePaths;
	public string map_HardRoad;
	// Броколи
	public string txt_Completed;
	public string txt_Coins;
	public string txt_Experience;
	public string txt_Record;
	public string txt_CoinsEarned;
	public string txt_PlacedCar;
	public string txt_TooltipTNT;
	public string txt_Slow;
	public string txt_Activate;
	public string txt_DescrReward;
	// мхи
	public string car_Log;
	public string car_Locker;
	public string car_SpCar;
	public string car_Crusher;
	public string car_Tank;
	public string car_Sartir;
	public string car_Mocik;
	public string car_Belaz;
	public string car_BunkBed;
	public string car_Fridge;
	// еблососы
	public string txt_saveEditor;
	public string txt_cancelEditor;
	public string txt_SureEditor;
	public string txt_YesEditor;
	public string txt_NoEditor;
	public string txt_BackEditor;
	public string txt_Editor;
}
