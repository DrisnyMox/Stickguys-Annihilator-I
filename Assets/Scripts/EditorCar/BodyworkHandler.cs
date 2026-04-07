using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class BodyworkHandler : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {

	static bool inHandler;
	
	public virtual void OnPointerDown (PointerEventData ped) {	}

	public virtual void OnPointerEnter (PointerEventData ped) {
		inHandler = true;
	}

	public virtual void OnPointerExit (PointerEventData ped) {
		inHandler = false;
	}

	public bool IsInHandler () {
		return inHandler;
	}
}
