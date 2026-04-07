using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemEditorCar : MonoBehaviour {

	Vector3 offset = Vector3.zero;

	BodyworkHandler bodyworkHandler;
	EditorCar editorCar;
	GameObject panelSettingsBodywork;
	GameObject panelSettingsWheel;
	public float multiplier = 1;
	[SerializeField] public BoxCollider2D tempCollider;
	public static GameObject carBase;
	public static List<GameObject> wheels = new List<GameObject>();

	public static List<ItemEditorCar> itemsEditor = new List<ItemEditorCar> ();
	[HideInInspector] public Color oldColor;

	[Space(18)]
	public ItemType itemType;


	void Start () {
		bodyworkHandler = FindObjectOfType<BodyworkHandler> ();
		editorCar = FindObjectOfType<EditorCar> ();
		panelSettingsBodywork = ObjectKeeper.panelSettingsBodywork;//GameObject.Find ("Panel Settings Bodywork");
		panelSettingsWheel = ObjectKeeper.panelSettingsWheel;

		if (itemType == ItemType.Bodywork) {
			if (!carBase) {
				carBase = gameObject;
			}
		} else if (itemType == ItemType.wheel) {
			wheels.Add (gameObject);
		}
		itemsEditor.Add (this);
		oldColor = GetComponent<SpriteRenderer> ().color;
		//----------------------------------------
		transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0,0,Camera.main.transform.position.z);

		editorCar.itemEditorCar = this;
		editorCar.ChangeSliderValue ();
		foreach (ItemEditorCar iec in itemsEditor) {
			iec.GetComponent<SpriteRenderer> ().color = iec.oldColor;
		}
		GetComponent<SpriteRenderer> ().color = new Color (0.7f, 1, 1);
	}

	void OnMouseDown () {
		offset = transform.position - Camera.main.ScreenToWorldPoint (Input.mousePosition);
		if (ItemType.Bodywork == itemType) {
			panelSettingsBodywork.SetActive (true);
			panelSettingsWheel.SetActive (false);
		} else {
			panelSettingsBodywork.SetActive (false);
			panelSettingsWheel.SetActive (true);
			offset += new Vector3 (0, 0, -0.15f);
		}
		editorCar.itemEditorCar = this;
		editorCar.ChangeSliderValue ();
		foreach (ItemEditorCar iec in itemsEditor) {
			iec.GetComponent<SpriteRenderer> ().color = iec.oldColor;
		}
		GetComponent<SpriteRenderer> ().color = new Color (0.7f, 1, 1);

	}
		
	void OnMouseDrag () {
		transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition) + offset;
	}

	void OnMouseUp () {
		if (bodyworkHandler.IsInHandler () /*&& !IsItemOverUI()/**/) {
			Destroy (gameObject);
			Game.gears++;
			EditorCar.currentUseGears--;
			Game.SaveGears ();
			editorCar.UpdateGears ();
		}

	}

	void OnMouseExit(){
		DeleteTempCollider ();
	}
		
	void OnDestroy(){
		DeleteTempCollider ();
		if (panelSettingsBodywork) panelSettingsBodywork.SetActive (false);
		if ( panelSettingsWheel) panelSettingsWheel.SetActive (false);
		if (ItemType.Bodywork == itemType) {
			if (carBase == gameObject) {
				carBase = GetCarBase ();
			}
		} else if (ItemType.wheel == itemType) {
			wheels.Remove (gameObject);
		}
		itemsEditor.Remove (this);
	}

	GameObject GetCarBase() {
		foreach (ItemEditorCar iec in FindObjectsOfType<ItemEditorCar>()) {
			if (iec.itemType == ItemType.Bodywork && iec.gameObject != gameObject) {
				return iec.gameObject;
			}
		}
		return null;
	}
		

	public void DeleteTempCollider(){
		if (tempCollider)
			Destroy (tempCollider);
	}

	//void SampleUnknowParameter (Vector3? ofset = null ) {/**/}
	//void SampleDefaultParameter (Vector3 ofset = default(Vector3) ) {/**/}

	public enum ItemType {
		Bodywork,
		wheel
	}
}
