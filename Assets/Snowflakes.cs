using UnityEngine;
using System.Collections;

public class Snowflakes : MonoBehaviour
{
	[SerializeField] GameObject[] Ground;

    int countExits = 0;

    void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag("CAR"))
		{
			countExits++;
			if (countExits >= 2 && gameObject.activeInHierarchy)
			{
				StartCoroutine(Dis());
			}
		}
	}

	IEnumerator Dis()
	{
		yield return new WaitForSeconds(1f);
		Destroy(gameObject);
		foreach (GameObject gm in Ground)
		{
			Destroy(gm);
		}
	}
}
