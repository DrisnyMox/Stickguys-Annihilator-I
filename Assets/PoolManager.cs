using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	public GameObject blood;
	public static int amountBlood = 50;
	List<GameObject> bloods = new List<GameObject>();
	public static PoolManager current;

	void Awake(){
		current = this;	
	}

	void Start () {
		GenerateBlood ();
	}
	
	// Update is called once per frame
	public void GenerateBlood(){
		int countBloods = bloods.Count;
		for (int i = 0; i < countBloods; i++) {
			GameObject g = bloods [0];
			bloods.Remove (g);
			Destroy (g);
		}
		for (int i = 0; i < amountBlood; i++) {
				GameObject bloodClone = (GameObject)Instantiate (this.blood);
				bloodClone.SetActive (false);
				bloods.Add (bloodClone);
		}
		Blood.bloods = bloods.ToArray ();
	}

	public static void StaticGenerate()
	{
		List<GameObject> sBloods = new List<GameObject>();
		GameObject blood = (GameObject)Resources.Load ("Blood", typeof(GameObject));
		for (int i = 0; i < amountBlood; i++) {
			GameObject bloodClone = (GameObject)Instantiate (blood);
			bloodClone.SetActive (false);
			sBloods.Add (bloodClone);
		}
		Blood.bloods = sBloods.ToArray ();

	}
}
