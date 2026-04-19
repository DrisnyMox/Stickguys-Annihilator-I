using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour {

	[SerializeField] UIButtonClickEffect clickEffect;

	public bool ClickedIs { get; set; } = false;
	public bool ClikedOnUI { get; set; } = false;
	// Use this for initialization
	public void OnMouseDown () {
		ClickedIs = true;
		ClikedOnUI = true;

		clickEffect?.PlayEffect();

		DebugLog.Add("click down");
	}
	
	// Update is called once per frame
	public void OnMouseUp () {
		ClickedIs = false;
		ClikedOnUI = false;
		DebugLog.Add("click up");
    }
}
