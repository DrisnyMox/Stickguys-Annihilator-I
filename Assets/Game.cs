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
		SaveLoadSystem.SaveCoins(currentCoins);
	}

	public static void LoadCoins(){
		currentCoins = SaveLoadSystem.LoadCoins();
	}

	public static int GetNumberCurrentLevel(){
		return SceneManager.GetActiveScene ().buildIndex;
	}

	public static int GetPlacedPrice()
    {
		return (80 * GetNumberCurrentLevel()) - 80;
	}

	public static void LoadGears(){
		
		gears = SaveLoadSystem.LoadGears(88);
		if (!SaveLoadSystem.HasKey(SaveLoadSystem.KeyGears)) {
			SaveGears ();
		}
	}

	public static void SaveGears() {
		SaveLoadSystem.SaveGears(gears);
	}

	public static void LoadCountTNT(){
		SaveLoadSystem.LoadTNT(out firstTNT, out secondTNT, "0", "3");
		if (!SaveLoadSystem.HasKey(SaveLoadSystem.KeyFirstTNT)) {
			SaveLoadSystem.SaveTNT(firstTNT, secondTNT);
		}
	}

	public static void SaveCountTNT(string count){
		if (count.Length > 1) {
			SaveLoadSystem.SaveTNT(count.Substring (0, 1), count.Substring (1, count.Length-1));
		} else {
			SaveLoadSystem.SaveTNT("0", count);
		}
		SaveLoadSystem.LoadTNT(out firstTNT, out secondTNT, "0", "0");
	}
}
