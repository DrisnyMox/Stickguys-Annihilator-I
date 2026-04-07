using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bone Color", menuName = "Великое Мясо/Bone Color")]
public class BoneColor : ScriptableObject {

	[SerializeField] public Color currentColor = Color.black;
	[Header("Набор цветов")]
	[SerializeField] public Color[] colorsBone;
	[SerializeField] public bool[] unlockBones;

	void OnEnable(){
		LoadColor ();
	}

	public void SaveColor(){
		int index;
		for(index=0; index<unlockBones.Length; index++){
			if (colorsBone [index] == currentColor) {
				break;
			}
		}
		PlayerPrefs.SetInt ("boneColor", index);

		string saveUnlocks = "";
		for (int i = 0; i < unlockBones.Length; i++) {
			saveUnlocks += string.Format ("{0};", unlockBones[i].ToString());
		}
		PlayerPrefs.SetString ("unlocksColor", saveUnlocks);
		PlayerPrefs.Save ();
	}
		
	public void LoadColor(){
		if (!PlayerPrefs.HasKey ("boneColor"))
			return;
		currentColor = colorsBone [PlayerPrefs.GetInt ("boneColor")];

		string loadUnlocks = PlayerPrefs.GetString ("unlocksColor");
		string[] itemsData = loadUnlocks.Split (new char[] { ';' }, System.StringSplitOptions.RemoveEmptyEntries);
		for (int i = 0; i < itemsData.Length; i++) {
			unlockBones[i] = bool.Parse(itemsData [i]);
		}
	}
}
