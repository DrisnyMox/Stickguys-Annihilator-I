using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WTF : MonoBehaviour {

	static WTF wtf;
	//public static Coroutine cor;

	void Awake(){
		wtf = this;
	}

	void Start(){
		wtf = this;
	}

	void Stope(Coroutine cor){
		StopCoroutine (HUD.cor);
	}

	public static IEnumerator WaitAction( Coroutine corut ){
		yield return null;

		bool agree = false;

		wtf.transform.GetChild (1).GetComponent<Button> ().onClick.RemoveAllListeners ();

		wtf.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (delegate() {
			agree = true;
		});

		wtf.transform.GetChild (0).GetComponent<Button> ().onClick.RemoveAllListeners ();
		wtf.transform.GetChild (0).GetComponent<Button> ().onClick.AddListener (delegate() {
			wtf.Stope(corut);
			wtf.gameObject.SetActive(false);
		});
		wtf.gameObject.SetActive (true);

		GameObject.Find ("txt_YouSureEditor").GetComponent<Text> ().text = Settings.lng.txt_AreYouSure;
		GameObject.Find ("txt_YesEditor").GetComponent<Text> ().text = Settings.lng.txt_Yes;
		GameObject.Find ("txt_NoEditor").GetComponent<Text> ().text = Settings.lng.txt_No;

		while(!agree){
			print ("Wait");
			yield return new WaitForSeconds (0.1f);
		}

		wtf.gameObject.SetActive (false);
	}

}
