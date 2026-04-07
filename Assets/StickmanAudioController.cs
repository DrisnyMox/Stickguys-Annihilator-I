using UnityEngine;
using System.Collections;

public class StickmanAudioController : MonoBehaviour {

	public AudioClip[] screams;
	public AudioClip screamsShot;
	public AudioSource source { get; set;}
	bool mainIsPlayed = false;
	// Use this for initialization
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	public void PlayScream () {
		source.clip = screams[(int)Random.Range(0,screams.Length-1)];
		if(source.clip)
			source.Play ();
	}

	void FixedUpdate(){
		if (Time.timeScale >= 0.99f) {
			source.pitch = 1.25f;
		} else
			source.pitch = 0.45f;
		if (!mainIsPlayed && source.time > 0.2f)
			mainIsPlayed = true;
	}

	public void PlayShotScream(){
		if (mainIsPlayed && source.time <= 0.001f) {
			source.clip = screams[(int)Random.Range(0,screams.Length-1)];
			if(source.clip)
				source.Play ();
		}
	}
}
