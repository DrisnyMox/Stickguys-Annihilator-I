using UnityEngine;
using System.Collections;

public class PostCreatedDrag : MonoBehaviour {

	Vector3 offset = Vector3.zero;
	bool isDrag;
	BodyworkHandler bodyworkHandler;

	void Start () {
		bodyworkHandler = (BodyworkHandler)GameObject.FindObjectOfType (typeof(BodyworkHandler));
	}
	
	public void OnMouseEnter(){
		offset = transform.position - Camera.main.ScreenToWorldPoint (Input.mousePosition);
		if (transform.GetComponent<ItemEditorCar> ().itemType == ItemEditorCar.ItemType.wheel)
			offset += new Vector3 (0, 0, -0.15f);
		isDrag = true;
	}

	void Update(){
		if (isDrag) {
			transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) + offset;
		}
		if (Input.GetMouseButtonUp (0)) {
			if (bodyworkHandler.IsInHandler ()) {
				Destroy (gameObject);
				Game.gears++;
				Game.SaveGears ();
				EditorCar editor = (EditorCar)FindObjectOfType (typeof(EditorCar));
				editor.UpdateGears ();
				return;
			} 
			Destroy (this);
		}
	}
}
