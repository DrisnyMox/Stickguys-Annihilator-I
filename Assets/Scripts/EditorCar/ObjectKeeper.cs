using UnityEngine;
using System.Collections;

public class ObjectKeeper : MonoBehaviour {

	[SerializeField] GameObject PanelSettingsBodywork;
	public static GameObject panelSettingsBodywork;
	[SerializeField] GameObject PanelSettingsWheel;
	public static GameObject panelSettingsWheel;
	[SerializeField] GameObject Car;
	public static GameObject car;


	void Start(){
		panelSettingsBodywork = PanelSettingsBodywork;
		panelSettingsWheel    = PanelSettingsWheel;
		car = Car;
		//***************************************
		panelSettingsBodywork.SetActive (false);
		panelSettingsWheel.SetActive (false);
	}

}
