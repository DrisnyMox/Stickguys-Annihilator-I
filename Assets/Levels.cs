using UnityEngine;
using System.Collections;

public static class Levels {

	public const int countLevels = 13;
	public const int countAutos = 10;
	public static int[] currentExperience = new int[countLevels];
	public static int[] recordExperience  = new int[countLevels];
	public static bool[] isOpen = new bool[countLevels];
	public static int[]  prices = new int[countLevels];

	public static int[] pricesAuto = new int[countAutos];
	public static bool[] isOpenAuto = new bool[countAutos];
	public static int priceEditor = 1500000;
	public static bool editorIsOpen = false;

	public static void SaveData(){
		PlayerPrefs.DeleteKey ("gmdata");
		PlayerPrefs.DeleteKey ("gmauto");
		string saveStr = "";
		for (int i = 0; i < countLevels; i++) {
			saveStr += string.Format ("{0}*{1};", recordExperience [i], isOpen [i]);
		}
		PlayerPrefs.SetString ("gmdata", saveStr);
		string saveAutos = "";
		for (int i = 0; i < countAutos; i++) {
			saveAutos += string.Format ("{0};", isOpenAuto [i]); 
		}
		PlayerPrefs.SetString ("gmauto", saveAutos);


		PlayerPrefs.Save ();
	}

	public static void LoadData(){
		if (!PlayerPrefs.HasKey ("gmdata"))
			return;
		string strData = PlayerPrefs.GetString ("gmdata");
		if (strData.Length == 0)
			return;
		string[] itemsData = strData.Split (new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < itemsData.Length/*countLevels/**/; i++) {
			
				string[] itemsLevel = itemsData [i].Split (new char[] { '*' }, System.StringSplitOptions.RemoveEmptyEntries);
				recordExperience [i] = int.Parse (itemsLevel [0]);
				isOpen [i] = bool.Parse (itemsLevel [1]);

		}
		if (!PlayerPrefs.HasKey ("gmauto"))
			return;
		string strAutos = PlayerPrefs.GetString ("gmauto");
		if (strAutos.Length == 0)
			return;
		string[] strAuto = strAutos.Split (new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < strAuto.Length; i++) {
			
			isOpenAuto [i] = bool.Parse (strAuto [i]);
		}
	}

	public static void SaveEditor(){
		PlayerPrefs.SetString ("editor", editorIsOpen.ToString());
		PlayerPrefs.Save ();
	}

	public static void LoadEditor(){
		if (!PlayerPrefs.HasKey ("editor")) {
			return;
		}
		string str = PlayerPrefs.GetString ("editor");
		editorIsOpen = bool.Parse (str);
	}
		
}
