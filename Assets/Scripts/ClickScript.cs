using UnityEngine;
using System.Collections;

public class ClickScript : MonoBehaviour {

	public bool clickedIs = false;
	// Use this for initialization
	public void OnMouseDown () {
		clickedIs = true;
        DebugLog.Add("click down");
	}
	
	// Update is called once per frame
	public void OnMouseUp () {
		clickedIs = false;
        DebugLog.Add("click up");
    }
}
