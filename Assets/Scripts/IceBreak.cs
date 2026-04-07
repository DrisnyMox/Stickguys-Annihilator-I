using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBreak : MonoBehaviour {

	public static List<IceBreak> breaks = new List<IceBreak> ();
	[HideInInspector] public bool use;
	static Transform parent;
	AudioSource audioSource;
	// Use this for initialization
	void Start () {
		breaks.Add (this);
		parent = transform.parent;
		audioSource = GetComponent<AudioSource> ();
	}
	
	public void SetBreak(Transform parent){
		use = true;
		transform.parent = parent;
		audioSource.Play ();
		GetComponent<ParticleSystem> ().Play ();
		transform.localPosition = Vector3.zero;
		StartCoroutine (Reset ());
	}

	void OnBecameInvisible(){
		if(gameObject.activeInHierarchy)
			transform.parent = parent;
	}

	IEnumerator Reset(){
		yield return new WaitForSeconds (audioSource.clip.length);
		use = false;
		transform.parent = parent;
	}


}
