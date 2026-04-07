using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class ItemWheel : MonoBehaviour {

	[SerializeField] GameObject itemEditorCar;
	EditorCar editor;

	void Start(){
		editor = (EditorCar)FindObjectOfType (typeof(EditorCar));
		int h = Screen.height / 5;
		GetComponent<LayoutElement> ().preferredHeight = h;
	}

	public void OnMouseDown () {
		if (Game.gears > 0) {
			GameObject item = (GameObject)Instantiate (
				                 itemEditorCar, 
				                 Vector3.Scale (transform.position, new Vector3 (1, 1, 0)), 
				                 Quaternion.identity
			                 );
			editor.itemEditorCar = item.GetComponent<ItemEditorCar> ();
			editor.ChangeSliderValue ();
			Game.gears--;
			EditorCar.currentUseGears++;
			editor.UpdateGears ();
		}
	}

}
