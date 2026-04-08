using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UseTNT : MonoBehaviour {

	[SerializeField] Color color1;
	[SerializeField] Color color2;

	GameObject glow;

	bool presed {
		get { 
			return HUD.tntSelected;
		}
		set { 
			HUD.tntSelected = value;
		}
	}
	
	public void OnPress() {
		if (int.Parse (Game.firstTNT + Game.secondTNT) > 0)
			presed = presed ? false : true;
		else {
			Transform ui = GameObject.Find ("UI").transform;
			//ui.GetChild (ui.childCount - 2).gameObject.SetActive (true);
			GameObject.Find("txt_TooltipTNT").GetComponent<Text> ().text = Settings.lng.txt_TooltipTNT;
		}
	}

	public void ControlDisplay(GameObject btn = null) {
		if (btn == null)
			btn = glow;
		else
			glow = btn;
		btn.GetComponent<Image> ().enabled = presed;
		if (presed) {
			StartCoroutine (ColorUp (btn));
		}
	}

	IEnumerator ColorUp(GameObject btn){
		for (int i = 0; i < 100; i++) {
			btn.GetComponent<Image> ().color = Color.Lerp (btn.GetComponent<Image> ().color, color2, Time.deltaTime*1.3f);
			yield return null;
		}
		if(presed)
			StartCoroutine (ColorDown (btn));
	}
	IEnumerator ColorDown(GameObject btn){
		for (int i = 0; i < 100; i++) {
			btn.GetComponent<Image> ().color = Color.Lerp (btn.GetComponent<Image> ().color, color1, Time.deltaTime*1.3f);
			yield return null;
		}
		if(presed)
			StartCoroutine (ColorUp (btn));
	}

#if UNITY_EDITOR
	void Update(){
		if (Input.GetKeyDown (KeyCode.N)) {
			TNT.IncreaseTNT (5);
		}
	}
#endif


}
