using UnityEngine;
using System.Collections;

public class CheckFire : MonoBehaviour {

	static CheckFire checkFire;

	void Start(){
		checkFire = this;
		Check ();
	}

	public static void Check()
	{
		if (!checkFire)
			return;

		Gun cf = (Gun)FindObjectOfType(typeof(Gun));
		if (cf == null)
		{
			checkFire.gameObject.SetActive(false);
		}
		else
		{
			checkFire.gameObject.SetActive(true);
		}
	}
		
}
