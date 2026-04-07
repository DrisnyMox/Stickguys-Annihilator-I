using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodActivator : MonoBehaviour {

	[SerializeField] Color clrEnable;
	[SerializeField] Color clrDisable;

	public static bool enable { 
		get {
			if (!PlayerPrefs.HasKey ("bloodActive"))
				return false;
			return bool.Parse(PlayerPrefs.GetString ("bloodActive"));
		}
		set { 
			PlayerPrefs.SetString ("bloodActive", value.ToString ());
			PlayerPrefs.Save ();
		}
	}

	void Start(){
		ChangeColorBtn ();
	}

	public void OnClick(){
		if (enable) {
			enable = false;
		} else {
			enable = true;
		}
		ChangeColorBtn ();
	}


	void ChangeColorBtn(){
		if (enable) {
			GetComponent<Image> ().color = clrEnable;
		} else {
			GetComponent<Image> ().color = clrDisable;
		}
	}
}
