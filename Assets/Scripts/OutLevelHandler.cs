using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutLevelHandler : MonoBehaviour
{

	public static CarScript car;


	void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag("CAR") && car.isPassed && this.enabled)
		{
			car.LevelComplete();
		}
	}
}
