using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public static class Game {

	public static int currentCoins = 0;
	public static int countShowAdvertise = 0;
	public static string firstTNT  = string.Empty;
	public static string secondTNT = string.Empty;
	public static int gears = 0;
	// Use this for initialization

	public static void SaveCoins(){
		PlayerPrefs.DeleteKey ("coins");
		PlayerPrefs.SetInt ("coins", currentCoins);
		PlayerPrefs.Save ();
	}

	public static void LoadCoins(){
		if (!PlayerPrefs.HasKey ("coins"))
			return;
		currentCoins = PlayerPrefs.GetInt ("coins");
	}

	public static int GetNumberCurrentLevel(){
		return SceneManager.GetActiveScene ().buildIndex;
	}

	public static void LoadGears(){
		
		if (!PlayerPrefs.HasKey ("gears")) {
			gears = 88;
			SaveGears ();
			return;
		}
		gears = PlayerPrefs.GetInt ("gears");
	}

	public static void SaveGears() {
		//PlayerPrefs.DeleteKey ("gears");
		PlayerPrefs.SetInt ("gears", gears);
		PlayerPrefs.Save ();
	}

	public static void LoadCountTNT(){
		if (!PlayerPrefs.HasKey ("fTNT")) {
			firstTNT = "0";
			secondTNT = "3";
			PlayerPrefs.SetString ("fTNT", firstTNT);
			PlayerPrefs.SetString ("sTNT", secondTNT);
			PlayerPrefs.Save ();
			return;
		}
		firstTNT = PlayerPrefs.GetString ("fTNT");
		secondTNT = PlayerPrefs.GetString ("sTNT");
	}

	public static void SaveCountTNT(string count){
		if (count.Length > 1) {
			PlayerPrefs.SetString ("fTNT", count.Substring (0, 1));
			PlayerPrefs.SetString ("sTNT", count.Substring (1, count.Length-1));
			PlayerPrefs.Save ();
		} else {
			PlayerPrefs.SetString ("fTNT", "0");
			PlayerPrefs.SetString ("sTNT", count);
		}
		firstTNT = PlayerPrefs.GetString ("fTNT");
		secondTNT = PlayerPrefs.GetString ("sTNT");
	}
}
