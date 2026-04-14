using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour {

	public bool ClickedIs { get; set; } = false;
	public bool ClikedOnUI { get; set; } = false;
	// Use this for initialization
	public void OnMouseDown () {
		ClickedIs = true;
		ClikedOnUI = true;

		DebugLog.Add("click down");
	}
	
	// Update is called once per frame
	public void OnMouseUp () {
		ClickedIs = false;
		ClikedOnUI = false;
		DebugLog.Add("click up");
    }
}
