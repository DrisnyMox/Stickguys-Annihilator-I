using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemColorBone : MonoBehaviour {

	public Text txtUsed;
	public GameObject price;
	public Image iconBone;
	[HideInInspector] public int index;

	void Start(){
		UpdateView ();
	}

	public void UpdateView(){
		Menu m = FindObjectOfType<Menu> ();
		int i = transform.GetSiblingIndex ();
		index = i;

		if (m.boneColor.currentColor == m.boneColor.colorsBone [i]) {
			txtUsed.text = Settings.lng.txt_Used;
		} else txtUsed.text = "";

		if (m.boneColor.unlockBones [i]) {
			price.SetActive (false);
		}

		iconBone.color = m.boneColor.colorsBone [i];
		
	}
}
