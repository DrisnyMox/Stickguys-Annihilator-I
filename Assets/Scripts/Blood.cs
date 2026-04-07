using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Blood : MonoBehaviour {

	public Transform pos;
	public static int countAll = 0;
	public static GameObject[] bloods;
	// Use this for initialization
	void Awake(){
		DontDestroyOnLoad (gameObject);
	}

	IEnumerator Start () {
		yield return new WaitForSeconds (2f);
		gameObject.SetActive (false);
	}

	void OnEnable(){
		StartCoroutine (Start ());
	}

	void Update () {
		if(pos)
			transform.position = pos.position - new Vector3(0,0,-0.1f);
	}

	public static bool GetBlood(Transform t){
		for (int i = 0; i < bloods.Length; i++) {
			if (!bloods [i].activeInHierarchy) {
				bloods [i].GetComponent<Blood> ().pos = t;
				bloods [i].SetActive (true);
				return true;
			}
		}
		return false;
	}

	void OnBecameInvisible(){
		gameObject.SetActive (false);
		StopCoroutine (Start ());
	}
		
}
