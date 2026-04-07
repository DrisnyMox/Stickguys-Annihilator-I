using UnityEngine;
using System.Collections;

public class Deserialization : MonoBehaviour {

	public static void DeserializeCar(SerializableCar car, byte[] previewPngBytes = null){

		GameObject customCar = new GameObject ();
		customCar.name = "Moxyebina";
		customCar.transform.position = new Vector3 (-36.3f, 2.8f, 0);
		GameObject root = (GameObject)Resources.Load (car.nameRoot);
		root = Instantiate (root);
		//root.GetComponent<ItemEditorCar> ().DeleteTempCollider ();
		root.transform.parent = customCar.transform;
		root.transform.localPosition = car.positionRoot;
		root.transform.localScale = car.scaleRoot;
		root.tag = "CAR";
		ClearScripts (root);
		GameObject emptyForTrigger = new GameObject ();
		emptyForTrigger.name = "Empty For Trigger";
		BoxCollider2D box =	emptyForTrigger.AddComponent<BoxCollider2D> ();
		emptyForTrigger.transform.parent = root.transform;
		emptyForTrigger.transform.localPosition = car.emptyForTrigger.positionEmpty;
		emptyForTrigger.transform.localScale = car.emptyForTrigger.scaleEmpty;
		box.size = new Vector2 (car.emptyForTrigger.colliderX, car.emptyForTrigger.colliderY);
		emptyForTrigger.GetComponent<BoxCollider2D> ().size = box.size;
		box.isTrigger = true;
		emptyForTrigger.tag = "CAR";
		for (int i = 0; i < car.bodyworks.Count; i++) { 
			GameObject child =	(GameObject)Instantiate(Resources.Load (car.bodyworks[i].nameChild));
			//child.GetComponent<ItemEditorCar> ().DeleteTempCollider ();
			child.name = car.bodyworks[i].nameChild;
			child.transform.parent = root.transform;
			child.transform.localPosition = car.bodyworks [i].positionChild - new Vector3(0,0,0.1f);
			child.transform.localScale = car.bodyworks [i].scaleChild;
			child.tag = "CAR";

			ClearScripts (child);
		}
		for (int i = 0; i < car.wheels.Count; i++) {
			GameObject wheel = (GameObject)Instantiate (Resources.Load (car.wheels [i].nameWheel));
			//wheel.GetComponent<ItemEditorCar> ().DeleteTempCollider ();
			wheel.transform.parent = customCar.transform;
			wheel.transform.localPosition = car.wheels [i].positionWheel-new Vector3(0,0,0.01f);
			wheel.transform.localScale = car.wheels [i].scaleWheel;
			wheel.tag = "BOBER";
			ClearScripts (wheel);
			WheelJoint2D wj2d = root.AddComponent<WheelJoint2D> ();
			wj2d.connectedBody = wheel.GetComponent<Rigidbody2D> ();
			Vector3 offsetAnchor = wj2d.connectedBody.transform.position - root.transform.position;
			offsetAnchor.x /= root.transform.localScale.x;
			offsetAnchor.y /= root.transform.localScale.y;
			wj2d.anchor = offsetAnchor;
			JointSuspension2D js2d = wj2d.suspension;
			js2d.frequency = 3.8f;
			wj2d.suspension = js2d;
		}
		root.GetComponent<Rigidbody2D> ().drag = 0f;
		root.GetComponent<Rigidbody2D> ().mass *= 5;
		root.AddComponent<CarScript> ().customCar = true;
		root.GetComponent<CarScript> ().usedGears = car.gears;
		root.AddComponent<AudioSource> ().spatialBlend = 1;
		root.GetComponent<AudioSource> ().loop = true;
		root.GetComponent<AudioSource> ().playOnAwake = false;
		root.GetComponent<AudioSource> ().volume = 0.9f;
		root.AddComponent<AudioContrCar> ();//.s_motor = ObjectKeeper.motors[Random.Range(0,2)];
		root.GetComponent<AudioContrCar> ().s_motor = Menu.motors[Random.Range(0,Menu.motors.Length)];

		Texture2D img = new Texture2D (car.textureSize.width, car.textureSize.height);
		if (previewPngBytes != null && previewPngBytes.Length > 0) {
			img.LoadImage(previewPngBytes);
		} else {
			string previewPath = Application.persistentDataPath + "/Images/" + car.name + ".png";
			if (System.IO.File.Exists(previewPath)) {
				img.LoadImage(System.IO.File.ReadAllBytes(previewPath));
			}
		}
		img.Apply ();


		HUD.imagesCarsCustom.Add (Sprite.Create (img, new Rect (0, 0, img.width, img.height), new Vector2 (0.5f, 0.5f)));
		HUD.carsCustom.Add (customCar);
		HUD.titleCarsCustom.Add (car.name);
		DontDestroyOnLoad (customCar);
		customCar.SetActive (false);
	}

	static void ClearScripts(GameObject go){
		Destroy (go.GetComponent<PostCreatedDrag> ());
		Destroy (go.GetComponent<ItemEditorCar>());
	}
}
