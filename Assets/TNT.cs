using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TNT : MonoBehaviour {

	AudioSource source;

	IEnumerator Start () {
		source = GetComponent<AudioSource> ();
		yield return new WaitForSeconds (0.35f);
		Destroy ( GetComponent<PointEffector2D>() );
		yield return new WaitForSeconds (5f);
		Destroy (gameObject);
	}

	public static void DecreaseTNT()
	{
		Game.SaveCountTNT((int.Parse(Game.firstTNT + Game.secondTNT) - 1).ToString());
		GameObject btnTNT = GameObject.Find("btn_TNT");
		if (btnTNT)
			btnTNT.transform.GetChild(1).GetComponent<Text>().text = (int.Parse(Game.firstTNT + Game.secondTNT)).ToString();
	}

	public static void IncreaseTNT(int count){
		Game.SaveCountTNT ( (int.Parse (Game.firstTNT + Game.secondTNT) + count).ToString() );
		if(GameObject.Find ("btn_TNT"))
			GameObject.Find ("btn_TNT").transform.GetChild (1).GetComponent<Text> ().text = (int.Parse (Game.firstTNT + Game.secondTNT)).ToString();
	}

	void FixedUpdate () {
		if (Time.timeScale >= 0.99f) {
			source.pitch = 1f;
		} else
			source.pitch = 0.5f;
	}

}
