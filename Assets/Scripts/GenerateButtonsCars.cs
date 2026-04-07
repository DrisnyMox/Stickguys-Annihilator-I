using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GenerateButtonsCars : MonoBehaviour {

	[SerializeField] GameObject btnCustomCar;
	public static bool carsIsLoad = false;
	public List<GameObject> btnsCustomCars = new List<GameObject>();


	public void Start(){
		Generate ();
	}

	void Generate(){
		if (!carsIsLoad) {
			Serialization.Load ();
			carsIsLoad = true;
		}

		HUD hud = (HUD)FindObjectOfType (typeof(HUD));
		hud.generateButtonsCars = this;
		float countCars = HUD.carsCustom.Count;
		if (countCars > 0) {
			for (int i = 0; i < countCars; i++) {
				GameObject btn = (GameObject)Instantiate (btnCustomCar);
				btn.transform.parent = transform;
				int id = i;
				btn.GetComponent<Button> ().onClick.AddListener (delegate() {
					hud.SelectAutoCustom(id);
				} );
				btn.transform.GetChild (1).GetComponent<Button> ().onClick.AddListener (delegate() {
					hud.BtnDeleteCar (id);
				} );
				btn.transform.GetChild (0).GetComponent<Image> ().sprite = HUD.imagesCarsCustom [i];
				btn.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = HUD.titleCarsCustom [i];
				btn.GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 1);
				btnsCustomCars.Add (btn);
			}
		}
	}

	/*void Start () {
		float countCars = HUD.carsCustom.Count;
		if (countCars > 0) {
			int a = (int)Mathf.Ceil(countCars / 4f);
			for (int i = 0; i < a; i++) {
				for (int j = 0; j < 4; j++) {
					if ((i*4) + j >= countCars) break;

					GameObject btn = (GameObject)Instantiate (btnCustomCar);
					btn.transform.parent = transform;
					btn.transform.localScale = Vector3.one;
					btn.transform.SetSiblingIndex (transform.childCount - 4);
					btn.GetComponent<RectTransform> ().offsetMin = new Vector2 (5, 5);
					btn.GetComponent<RectTransform> ().offsetMax = new Vector2 (-5, 0);
					float anchorX = j * 0.2f;
					float anchorY = i * 0.25f;
					btn.GetComponent<RectTransform> ().anchorMin = new Vector2 (0.1f+anchorX, -0.15f-anchorY);
					btn.GetComponent<RectTransform> ().anchorMax = new Vector2 (0.3f+anchorX, 0.1f-anchorY);
					HUD hud = (HUD)FindObjectOfType (typeof(HUD));
					int id = (i * 4) + j;
					btn.GetComponent<Button> ().onClick.AddListener (delegate() {
						hud.SelectAutoCustom(id);

					} );
				}
			}
		}
	}/**/
}
