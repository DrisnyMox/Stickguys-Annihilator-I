using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelReward : MonoBehaviour {

	static PanelReward instance;
	[SerializeField] Text txt_CountTnT;
	[SerializeField] Text txt_CountGear;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		instance = this;
	}
	
	public static void Show(string countTnT, string countGears ){
		instance.transform.GetChild (0).gameObject.SetActive (true);
		instance.txt_CountTnT.text = "+"+countTnT;
		instance.txt_CountGear.text = "+"+countGears;
		GameObject.Find ("txt_inf").GetComponent<Text> ().text = Settings.lng.txt_DescrReward;
	}

	IEnumerator Hide(){
		yield return new WaitForSeconds (3f);
		instance.transform.GetChild (0).gameObject.SetActive (false);
	}
}
