using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class Serialization {

	public static void Save(SerializableCar car) {
		
		if (!Directory.Exists (Application.persistentDataPath + "/Cars/")) {
			Directory.CreateDirectory (Application.persistentDataPath + "/Cars/");
		}
		var files = Directory.GetFiles (Application.persistentDataPath + "/Cars/");
		string name = car.name.Trim();//(files.Length).ToString ();
		//SaveLoad.cube = Cube.cube;
		BinaryFormatter bf = new BinaryFormatter();
		SurrogateSelector ss = new SurrogateSelector ();
		Vector3Surrogate Vector3_SS = new Vector3Surrogate();
		ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), Vector3_SS);

		bf.SurrogateSelector = ss;

		FileStream file = File.Create (Application.persistentDataPath + "/Cars/"+name+".stk");
		//bf.Serialize(file, SaveLoad.cube);
		bf.Serialize (file, car);
		file.Close();
		Debug.Log (Application.persistentDataPath);
	}   

	public static void Load() {
		if (!Directory.Exists (Application.persistentDataPath + "/Cars/"))
			return;
		var files = Directory.GetFiles (Application.persistentDataPath + "/Cars/");
		for (int i = 0; i < files.Length; i++) {
			if(File.Exists(files[i]/*Application.persistentDataPath + "/Cars/"+i.ToString()+".stk"/**/)) {
				BinaryFormatter bf = new BinaryFormatter();
				SurrogateSelector ss = new SurrogateSelector ();
				//Application.persistentDataPath это строка; выведите ее в логах и вы увидите расположение файла сохранений
				Vector3Surrogate Vector3_SS = new Vector3Surrogate();
				ss.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), Vector3_SS);

				bf.SurrogateSelector = ss;
				FileStream file = File.Open(files[i]/*Application.persistentDataPath + "/Cars/"+i.ToString()+".stk"/**/, FileMode.Open);
				//SaveLoad.cube = (Cube)bf.Deserialize(file);
				SerializableCar car  = (SerializableCar)bf.Deserialize (file);
				file.Close();
				//------------------------------
				Deserialization.DeserializeCar(car);
			}	
		}
	}

	public static void DeleteCar(int id){
		File.Delete(Application.persistentDataPath + "/Cars/"+HUD.titleCarsCustom[id].Trim()+".stk");
		File.Delete(Application.persistentDataPath + "/Images/"+HUD.titleCarsCustom[id].Trim()+".png");
		GameObject g = HUD.carsCustom [id];
		HUD.carsCustom.Remove (g);
		string t = HUD.titleCarsCustom [id];
		HUD.titleCarsCustom.Remove (t);
		Sprite s = HUD.imagesCarsCustom [id];
		HUD.imagesCarsCustom.Remove (s);
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
