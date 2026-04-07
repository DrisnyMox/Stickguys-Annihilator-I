using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EditorCar : MonoBehaviour {

	[HideInInspector] public ItemEditorCar itemEditorCar;
	[SerializeField] Slider[] slide;
	delegate void Deleg(ItemEditorCar iec);
	delegate Vector3 GetPointCollider(Vector3 bodywork, Vector3 border);
	[SerializeField] GameObject panelApproval;
	string titleCar = string.Empty;
	public static int numberLevel;
	[SerializeField] Text txt_Gears;
	public static int currentUseGears = 0;

	void Start () {
		Time.timeScale = 0;
		Game.LoadGears ();
		print("Loaded gears-"+ Game.gears);
		UpdateGears (); 
		ChangeLanguage ();
		panelApproval.SetActive (false);
		ItemEditorCar.itemsEditor.Clear ();
	}

	public void SaveCar () {
		
		Game.LoadGears ();
		int countWheel = 0;
		foreach (ItemEditorCar iec in FindObjectsOfType<ItemEditorCar>())
			if (iec.itemType == ItemEditorCar.ItemType.wheel) 
				countWheel++;
		if (countWheel > 1) {
			panelApproval.SetActive (true);
			GameObject.Find ("txt_Ok").GetComponent<Text> ().text = Settings.lng.txt_YesEditor;
			GameObject.Find ("txt_Cancel").GetComponent<Text> ().text = Settings.lng.txt_cancelEditor;
		}
	}

	public void BeginSaveCar(){
		if (titleCar.Length > 0) {
			panelApproval.SetActive (false);
			if (ItemEditorCar.carBase) {
				BuildCar ();
				foreach (ItemEditorCar iec in (ItemEditorCar[])FindObjectsOfType(typeof(ItemEditorCar))) {
					Destroy (iec);
				}
			}
		}
	}

	public void UpdateGears() {
		txt_Gears.text = "x" + Game.gears.ToString ();
		Game.SaveGears ();
	}

	public void SetTitleCar(string title){
		if (title.Length > 0) {
			HUD.titleCarsCustom.Add (title);
			titleCar = title.Trim ();
		}
	}

	void BuildCar(){

		foreach (ItemEditorCar iec in ItemEditorCar.itemsEditor) {
			iec.GetComponent<SpriteRenderer> ().color = iec.oldColor;
		}

		ItemEditorCar.carBase.transform.parent = ObjectKeeper.car.transform;
		foreach (GameObject wheel in ItemEditorCar.wheels) {
			wheel.transform.parent = ObjectKeeper.car.transform;
		}

		ItemEditorCar.carBase.AddComponent<Rigidbody2D> ().useAutoMass = true;
		for (int i = 0; i < ItemEditorCar.wheels.Count; i++) {
			ItemEditorCar.wheels [i].tag = "BOBER";
			ItemEditorCar.wheels [i].transform.position -= new Vector3 (0, 0, 0.1f);
			WheelJoint2D wj2d = ItemEditorCar.carBase.AddComponent<WheelJoint2D> ();
			wj2d.connectedBody = ItemEditorCar.wheels [i].GetComponent<Rigidbody2D> ();
			Vector3 offsetAnchor = wj2d.connectedBody.transform.position - ItemEditorCar.carBase.transform.position;
			offsetAnchor.x /= ItemEditorCar.carBase.transform.localScale.x;
			offsetAnchor.y /= ItemEditorCar.carBase.transform.localScale.y;
			wj2d.anchor = offsetAnchor;
			JointSuspension2D js2d = wj2d.suspension;
			js2d.frequency = 3.8f;
			wj2d.suspension = js2d;
		}

		ItemEditorCar leftmostBodywork  = ItemEditorCar.carBase.GetComponent<ItemEditorCar>();
		ItemEditorCar rightmostBodywork = ItemEditorCar.carBase.GetComponent<ItemEditorCar>();
		Vector3 left = new Vector3(99,0,0);
		Vector3 right = new Vector3(-99,0,0);
		Vector3 up = new Vector3 (0, -99, 0);
		Vector3 down = new Vector3 (0, 99, 0);
		foreach (ItemEditorCar iec in (ItemEditorCar[])FindObjectsOfType(typeof(ItemEditorCar))) {
			if (iec.itemType == ItemEditorCar.ItemType.Bodywork) {
				if (!iec.GetComponent<BoxCollider2D> ()) {
					iec.gameObject.AddComponent<BoxCollider2D> ();
				}
				Vector3 borderPoint = iec.GetComponent<BoxCollider2D> ().size / 2;
				borderPoint.x *= iec.transform.localScale.x;
				borderPoint.y *= iec.transform.localScale.y;
				if (left.x > (iec.transform.position - borderPoint).x) {
					left = iec.transform.position - borderPoint;
					leftmostBodywork = iec;
				} 
				if (right.x < (iec.transform.position + borderPoint).x) {
					right = iec.transform.position + borderPoint;
					rightmostBodywork = iec;
				}
				if (up.y < (iec.transform.position + borderPoint).y) {
					up = iec.transform.position + borderPoint;
				}
				if (down.y > (iec.transform.position - borderPoint).y) {
					down = iec.transform.position - borderPoint;
				} 
			} else if (iec.itemType == ItemEditorCar.ItemType.wheel) {
				float radius = iec.GetComponent<CircleCollider2D> ().radius;
				radius *= iec.transform.localScale.x;
				if (down.y > (iec.transform.position.y - radius)) {
					down = iec.transform.position - new Vector3(0,radius,0);
				}
				if (left.x > (iec.transform.position.x - radius)) {
					left = iec.transform.position - new Vector3 (radius, 0, 0);
				}
				if (right.x < (iec.transform.position.x + radius)) {
					right = iec.transform.position + new Vector3 (radius, 0, 0);
				}
			}
		}
	
		GameObject emptyForTrigger = new GameObject ();
		emptyForTrigger.name = "Empty For Trigger";
		emptyForTrigger.transform.parent = ObjectKeeper.car.transform;/**/
		BoxCollider2D boxTrigget = emptyForTrigger.AddComponent<BoxCollider2D>();
		boxTrigget.isTrigger = true;
		Vector3 leftmostPoint = SearchEdgeMostPoint (leftmostBodywork, delegate(Vector3 bodywork, Vector3 border) {
			return bodywork - border;
		});
		Vector3 rightmostPoint = SearchEdgeMostPoint (rightmostBodywork, delegate(Vector3 bodywork, Vector3 border) {
			return bodywork + border;
		});

		boxTrigget.size = new Vector2 (rightmostPoint.x - leftmostPoint.x, ItemEditorCar.carBase.GetComponent<BoxCollider2D>().size.y);
		//boxTrigget.size /= ItemEditorCar.carBase.transform.localScale.x;
		Vector3 offsetTriggerBox = new Vector3 ();
		offsetTriggerBox.x = ((rightmostPoint.x-leftmostPoint.x)/2 ) + leftmostPoint.x;
		emptyForTrigger.transform.position = offsetTriggerBox;
		emptyForTrigger.transform.localScale += new Vector3 (0.1f,3,0);
		emptyForTrigger.transform.parent = ItemEditorCar.carBase.transform;
		emptyForTrigger.tag = "CAR";

		//emptyForTrigger.transform.parent = ItemEditorCar.carBase.transform;
		/*if (ItemEditorCar.carBase.GetComponent<ItemEditorCar> ().Equals (rightmostBodywork)) {
			offsetTriggerBox = (rightmostBodywork.transform.position - leftmostBodywork.transform.position ) / 2;
			float scaleX = rightmostBodywork.transform.localScale.x;
			float sizeX = rightmostBodywork.GetComponent<BoxCollider2D> ().size.x;
			//print ( sizeX - (sizeX - (scaleX * sizeX)) );
			//offsetTriggerBox.x -= sizeX - (sizeX - (scaleX * sizeX));
			emptyForTrigger.transform.position = offsetTriggerBox + leftmostBodywork.transform.position;
			//if(rightmostBodywork.GetComponent<BoxCollider2D>().size.x * 
		}
		if (ItemEditorCar.carBase.GetComponent<ItemEditorCar> ().Equals (rightmostBodywork)) {
			offsetTriggerBox = (leftmostBodywork.transform.position - rightmostBodywork.transform.position) / 2;
			float sizeX = (rightmostBodywork.GetComponent<BoxCollider2D> ().size.x - boxTrigget.size.x) / 2;
			print (offsetTriggerBox);
			offsetTriggerBox.x = sizeX;
		} else if (ItemEditorCar.carBase.GetComponent<ItemEditorCar> ().Equals (leftmostBodywork)) {
			offsetTriggerBox = (rightmostBodywork.transform.position - leftmostBodywork.transform.position) / 2;
			float sizeX = ( boxTrigget.size.x - leftmostBodywork.GetComponent<BoxCollider2D> ().size.x) / 2;
			offsetTriggerBox.x = sizeX;
			//offsetTriggerBox /= leftmostBodywork.transform.localScale.x;
		} else {
			float leftmost = ItemEditorCar.carBase.transform.position.x - leftmostBodywork.transform.position.x;
			float rightmost = rightmostBodywork.transform.position.x -	ItemEditorCar.carBase.transform.position.x;
			offsetTriggerBox.x = (rightmost - leftmost ) / 2;
			offsetTriggerBox.x /= ItemEditorCar.carBase.transform.localScale.x;
		}/**/
		//boxTrigget.offset = offsetTriggerBox;

		ForeachItemEditorCar (SetCarbaseAsParent);
		ForeachItemEditorCar (AddTagToBodywork);
		ItemEditorCar.carBase.AddComponent<CarScript> ().customCar = true;
		ItemEditorCar.carBase.GetComponent<CarScript> ().usedGears = currentUseGears;
		HUD.carsCustom.Add (ObjectKeeper.car);

		Vector3 positionRoot = ObjectKeeper.car.transform.GetChild (0).localPosition;
		string nameRoot = ObjectKeeper.car.transform.GetChild (0).name.Replace ("(Clone)", "");
		Vector3 scaleRoot = ObjectKeeper.car.transform.GetChild (0).localScale;
		List<SerializableCar.Bodywork> bodyworks = new List<SerializableCar.Bodywork> ();
		for (int i = 1; i < ObjectKeeper.car.transform.GetChild(0).childCount; i++) {// Нулевой элемент Пустышка для триггера
			SerializableCar.Bodywork b = new SerializableCar.Bodywork();
			b.positionChild = ObjectKeeper.car.transform.GetChild(0).GetChild(i).localPosition;
			b.scaleChild = ObjectKeeper.car.transform.GetChild (0).GetChild (i).localScale;
			b.nameChild = ObjectKeeper.car.transform.GetChild (0).GetChild (i).name.Replace ("(Clone)", "");
			bodyworks.Add (b);
		}
		List<SerializableCar.Wheel> wheels = new List<SerializableCar.Wheel> ();
		for (int i = 1; i < ObjectKeeper.car.transform.childCount; i++) {//нулевой элемент корневая асть кузова 
			SerializableCar.Wheel w = new SerializableCar.Wheel();
			w.positionWheel = ObjectKeeper.car.transform.GetChild (i).localPosition;
			w.scaleWheel = ObjectKeeper.car.transform.GetChild (i).localScale;
			w.nameWheel = ObjectKeeper.car.transform.GetChild (i).name.Replace ("(Clone)","");
			wheels.Add (w);
		}
		string name = titleCar;

		SerializableCar car = new SerializableCar (positionRoot, nameRoot, scaleRoot, bodyworks, wheels, name);
		car.textureSize.width = (int)(right.x - left.x + 16);
		car.textureSize.height = (int)(up.y-down.y+16);
		car.emptyForTrigger.positionEmpty = emptyForTrigger.transform.localPosition;
		car.emptyForTrigger.scaleEmpty = emptyForTrigger.transform.localScale;
		car.emptyForTrigger.colliderX = boxTrigget.size.x;
		car.emptyForTrigger.colliderY = boxTrigget.size.y;
		car.gears = currentUseGears;
		currentUseGears = 0;
		Serialization.Save (car);

		StartCoroutine(CreateImage(left, right, up, down));

	}


	Vector3 SearchEdgeMostPoint (ItemEditorCar edgemostBodywork, GetPointCollider getCollider) {
		Vector3 borderPoint = (edgemostBodywork.GetComponent<BoxCollider2D> ().size / 2);
		borderPoint.x *= edgemostBodywork.transform.localScale.x;
		borderPoint = getCollider (edgemostBodywork.transform.position, borderPoint);
		return borderPoint;
	}//__________________________________________________________________________

	void ForeachItemEditorCar(Deleg deleg){
		foreach (ItemEditorCar iec in (ItemEditorCar[])FindObjectsOfType(typeof(ItemEditorCar))) {
			if(iec.itemType == ItemEditorCar.ItemType.Bodywork)
				deleg (iec);
		}
	}

	void SetCarbaseAsParent(ItemEditorCar iec){
		if (iec.gameObject != gameObject) {
			iec.transform.parent = ItemEditorCar.carBase.transform;
		}
	}
	
	void AddTagToBodywork(ItemEditorCar iec){
		iec.tag = "CAR";
	}//__________________________________________________________ 9 13:10 _______

	IEnumerator CreateImage(Vector3 leftPoint, Vector3 rightPoint, Vector3 upPoint, Vector3 downPoint ){
		
		if (!Directory.Exists (Application.persistentDataPath + "/Images/")) {
			Directory.CreateDirectory (Application.persistentDataPath + "/Images/");
		}

		yield return new WaitForEndOfFrame(); 
		leftPoint = Camera.main.WorldToScreenPoint (leftPoint);
		rightPoint = Camera.main.WorldToScreenPoint (rightPoint);
		upPoint = Camera.main.WorldToScreenPoint (upPoint);
		downPoint = Camera.main.WorldToScreenPoint (downPoint);

		Texture2D imageCar = new Texture2D((int)(rightPoint.x-leftPoint.x+16), (int)(upPoint.y-downPoint.y+16));
		// кропаем в свежесозданную текстуру нужный участок экрана
		imageCar.ReadPixels(new Rect(leftPoint.x-8, downPoint.y-8, rightPoint.x-leftPoint.x+16, upPoint.y-downPoint.y+16), 0, 0, true);
		imageCar.Apply();
		int w = imageCar.width / 3;
		int h = imageCar.height / 3;
		TextureScale.Bilinear (imageCar, w, h);

		HUD.imagesCarsCustom.Add (Sprite.Create (imageCar, new Rect (0, 0, imageCar.width, imageCar.height), new Vector2 (0.5f, 0.5f)));
		FileStream fs = System.IO.File.Open(Application.persistentDataPath + "/Images/" + titleCar.Trim() + ".png", FileMode.Create); 
		BinaryWriter binary = new BinaryWriter(fs);
		binary.Write(imageCar.EncodeToPNG());	// и на диск
		fs.Close();

		ObjectKeeper.car.SetActive (false);
		ObjectKeeper.car.transform.position = new Vector3 (-36.3f, 2.8f, 0);
		DontDestroyOnLoad (ObjectKeeper.car);
		SceneManager.LoadSceneAsync (numberLevel);
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.025f;
	}

	public void SetWidth(float amount){
		Vector3 scale = itemEditorCar.transform.localScale;
		scale.x = amount;
		itemEditorCar.transform.localScale = scale;
	}

	public void BackToLevel(){
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.025f;
		Game.gears += currentUseGears;
		Game.SaveGears ();
		currentUseGears = 0;
		SceneManager.LoadScene (numberLevel);
	}

	public void SetHeight(float amount){
		Vector3 scale = itemEditorCar.transform.localScale;
		scale.y = amount;
		itemEditorCar.transform.localScale = scale;
	}

	public void SetRadius(float amount){
		Vector3 scale = itemEditorCar.transform.localScale;
		amount *= itemEditorCar.multiplier;
		scale = new Vector3 (amount,amount,amount);
		itemEditorCar.transform.localScale = scale;
	}//___________________________________________________________________________

	public void ChangeSliderValue(){
		if (itemEditorCar.itemType == ItemEditorCar.ItemType.Bodywork) {
			slide [0].value = itemEditorCar.transform.localScale.x;
			slide [1].value = itemEditorCar.transform.localScale.y;
		} else {
			slide [2].value = itemEditorCar.transform.localScale.x/itemEditorCar.multiplier;
		}
	}

	public void RemoveItem(){
		if (itemEditorCar) {
			Destroy (itemEditorCar.gameObject);
			currentUseGears--;
			Game.gears++;
			Game.SaveGears ();
			UpdateGears ();
		}
	}

	void ChangeLanguage(){
		GameObject.Find ("txt_Save").GetComponent<Text> ().text = Settings.lng.txt_saveEditor;
		GameObject.Find ("txt_Back").GetComponent<Text> ().text = Settings.lng.txt_BackEditor;
	}
}
