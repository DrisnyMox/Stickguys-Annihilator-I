using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Text;

public class Serialization {

	const string CarsFolder = "/Cars/";
	const string JsonExtension = ".json";

	[System.Serializable]
	class CarSaveFileData {
		public int version = 1;
		public string name = string.Empty;
		public string createdUtc = string.Empty;
		public int gears = 0;
		public string rootName = string.Empty;
		public int previewWidth = 0;
		public int previewHeight = 0;
		public string binaryCarData = string.Empty;
		public string previewPngData = string.Empty;
	}

	[System.Serializable]
	class CarsCloudData {
		public int version = 1;
		public List<CarCloudEntry> cars = new List<CarCloudEntry>();
	}

	[System.Serializable]
	class CarCloudEntry {
		public string name = string.Empty;
		public string jsonBase64 = string.Empty;
	}

	public static void Save(SerializableCar car) {
		string carsDirectory = Application.persistentDataPath + CarsFolder;
		if (!Directory.Exists(carsDirectory)) {
			Directory.CreateDirectory(carsDirectory);
		}

		string name = car.name.Trim();
		byte[] payload = SerializeCarToBytes(car);

		CarSaveFileData saveData = new CarSaveFileData();
		saveData.name = name;
		saveData.createdUtc = System.DateTime.UtcNow.ToString("o");
		saveData.gears = car.gears;
		saveData.rootName = car.nameRoot;
		saveData.previewWidth = car.textureSize.width;
		saveData.previewHeight = car.textureSize.height;
		saveData.binaryCarData = System.Convert.ToBase64String(payload);
		saveData.previewPngData = string.Empty;

		string json = JsonUtility.ToJson(saveData, true);
		File.WriteAllText(carsDirectory + name + JsonExtension, json);
		SyncCarsDirectoryToCloud();
		Debug.Log(Application.persistentDataPath);
	}

	public static void Load() {
		RestoreCarsDirectoryFromCloud();
		string carsDirectory = Application.persistentDataPath + CarsFolder;
		if (!Directory.Exists(carsDirectory))
			return;

		string[] jsonFiles = Directory.GetFiles(carsDirectory, "*" + JsonExtension);
		for (int i = 0; i < jsonFiles.Length; i++) {
			LoadFromJson(jsonFiles[i]);
		}

	}

	static void LoadFromJson(string path) {
		if (!File.Exists(path))
			return;

		string json = File.ReadAllText(path);
		if (string.IsNullOrEmpty(json))
			return;

		CarSaveFileData saveData = JsonUtility.FromJson<CarSaveFileData>(json);
		if (saveData == null || string.IsNullOrEmpty(saveData.binaryCarData))
			return;

		byte[] payload = System.Convert.FromBase64String(saveData.binaryCarData);
		SerializableCar car = DeserializeCarFromBytes(payload);
		byte[] previewBytes = null;
		if (!string.IsNullOrEmpty(saveData.previewPngData)) {
			previewBytes = System.Convert.FromBase64String(saveData.previewPngData);
		}
		Deserialization.DeserializeCar(car, previewBytes);
	}


	public static void UpdatePreviewPng(string carName, byte[] pngBytes) {
		if (string.IsNullOrEmpty(carName) || pngBytes == null || pngBytes.Length == 0)
			return;

		string path = Application.persistentDataPath + CarsFolder + carName.Trim() + JsonExtension;
		if (!File.Exists(path))
			return;

		string json = File.ReadAllText(path);
		if (string.IsNullOrEmpty(json))
			return;

		CarSaveFileData saveData = JsonUtility.FromJson<CarSaveFileData>(json);
		if (saveData == null)
			return;

		saveData.previewPngData = System.Convert.ToBase64String(pngBytes);
		File.WriteAllText(path, JsonUtility.ToJson(saveData, true));
		SyncCarsDirectoryToCloud();
	}

	static byte[] SerializeCarToBytes(SerializableCar car) {
		BinaryFormatter bf = CreateFormatter();
		MemoryStream stream = new MemoryStream();
		bf.Serialize(stream, car);
		byte[] payload = stream.ToArray();
		stream.Close();
		return payload;
	}

	static SerializableCar DeserializeCarFromBytes(byte[] payload) {
		BinaryFormatter bf = CreateFormatter();
		MemoryStream stream = new MemoryStream(payload);
		SerializableCar car = (SerializableCar)bf.Deserialize(stream);
		stream.Close();
		return car;
	}

	static BinaryFormatter CreateFormatter() {
		BinaryFormatter bf = new BinaryFormatter();
		SurrogateSelector ss = new SurrogateSelector();
		Vector3Surrogate vector3Surrogate = new Vector3Surrogate();
		ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
		bf.SurrogateSelector = ss;
		return bf;
	}

	public static void DeleteCar(int id){
		File.Delete(Application.persistentDataPath + CarsFolder + HUD.titleCarsCustom[id].Trim() + JsonExtension);
		SyncCarsDirectoryToCloud();
		GameObject g = HUD.carsCustom [id];
		HUD.carsCustom.Remove (g);
		string t = HUD.titleCarsCustom [id];
		HUD.titleCarsCustom.Remove (t);
		Sprite s = HUD.imagesCarsCustom [id];
		HUD.imagesCarsCustom.Remove (s);
	}

	static void SyncCarsDirectoryToCloud() {
		string carsDirectory = Application.persistentDataPath + CarsFolder;
		if (!Directory.Exists(carsDirectory)) {
			SaveLoadSystem.SaveEditorCarsData(string.Empty);
			return;
		}

		string[] jsonFiles = Directory.GetFiles(carsDirectory, "*" + JsonExtension);
		CarsCloudData cloudData = new CarsCloudData();
		for (int i = 0; i < jsonFiles.Length; i++) {
			string path = jsonFiles[i];
			if (!File.Exists(path))
				continue;

			string fileName = Path.GetFileNameWithoutExtension(path);
			string json = File.ReadAllText(path);
			if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(json))
				continue;

			CarCloudEntry entry = new CarCloudEntry();
			entry.name = fileName;
			entry.jsonBase64 = System.Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
			cloudData.cars.Add(entry);
		}

		string payload = cloudData.cars.Count > 0 ? JsonUtility.ToJson(cloudData) : string.Empty;
		SaveLoadSystem.SaveEditorCarsData(payload);
	}

	static void RestoreCarsDirectoryFromCloud() {
		string payload = SaveLoadSystem.LoadEditorCarsData();
		if (string.IsNullOrEmpty(payload))
			return;

		CarsCloudData cloudData = JsonUtility.FromJson<CarsCloudData>(payload);
		if (cloudData == null || cloudData.cars == null || cloudData.cars.Count == 0)
			return;

		string carsDirectory = Application.persistentDataPath + CarsFolder;
		if (!Directory.Exists(carsDirectory))
			Directory.CreateDirectory(carsDirectory);

		string[] oldFiles = Directory.GetFiles(carsDirectory, "*" + JsonExtension);
		for (int i = 0; i < oldFiles.Length; i++) {
			File.Delete(oldFiles[i]);
		}

		for (int i = 0; i < cloudData.cars.Count; i++) {
			CarCloudEntry entry = cloudData.cars[i];
			if (entry == null || string.IsNullOrEmpty(entry.name) || string.IsNullOrEmpty(entry.jsonBase64))
				continue;

			byte[] jsonBytes = System.Convert.FromBase64String(entry.jsonBase64);
			string carJson = Encoding.UTF8.GetString(jsonBytes);
			if (string.IsNullOrEmpty(carJson))
				continue;

			File.WriteAllText(carsDirectory + entry.name.Trim() + JsonExtension, carJson);
		}
	}
}

[System.Serializable]
public class SerializableCar {

	public Vector3 positionRoot;
	public string nameRoot;
	public Vector3 scaleRoot;
	public List<Bodywork> bodyworks = new List<Bodywork> ();
	public List<Wheel> wheels = new List<Wheel>();
	public string name = string.Empty;
	public TextureSize textureSize;
	public EmptyForTrigger emptyForTrigger;
	public int gears;

	[System.Serializable]
	public struct Bodywork {
		public Vector3 positionChild;
		public Vector3 scaleChild;
		public string nameChild;
	}
	[System.Serializable]
	public struct Wheel {
		public Vector3 positionWheel;
		public Vector3 scaleWheel;
		public string nameWheel;
	}
	[System.Serializable]
	public struct EmptyForTrigger {
		public Vector3 positionEmpty;
		public Vector3 scaleEmpty;
		public float colliderX;
		public float colliderY;
	}
	[System.Serializable]
	public struct TextureSize {
		public int width;
		public int height;
	}
		
	public SerializableCar (Vector3 positionRoot, string nameRoot, Vector3 scaleRoot, List<Bodywork> bodyworks, List<Wheel> wheels, string name ){
		this.positionRoot = positionRoot;
		this.nameRoot = nameRoot;
		this.scaleRoot = scaleRoot;
		this.bodyworks = bodyworks;
		this.wheels = wheels;
		this.name = name;
	}
}

sealed class Vector3Surrogate : ISerializationSurrogate {

	public void GetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context) {

		Vector3 v3 = (Vector3) obj;
		info.AddValue("x", v3.x);
		info.AddValue("y", v3.y);
		info.AddValue("z", v3.z);
		//Debug.Log(v3);
	}
		
	public System.Object SetObjectData(System.Object obj,
		SerializationInfo info, StreamingContext context,
		ISurrogateSelector selector) {

		Vector3 v3 = (Vector3) obj;
		v3.x = (float)info.GetValue("x", typeof(float));
		v3.y = (float)info.GetValue("y", typeof(float));
		v3.z = (float)info.GetValue("z", typeof(float));
		obj = v3;
		return obj;   // Formatters ignore this return value //Seems to have been fixed!
	}
}
