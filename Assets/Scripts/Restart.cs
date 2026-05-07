using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Restart : MonoBehaviour
{
	public void Reload(int lvl)
	{
		SceneManager.LoadScene(lvl);
		Time.fixedDeltaTime = DetectRagdoll.old;
		Time.timeScale = 1;
	}

	public void LoadMenu()
	{
		SceneManager.LoadScene("Menu");
	}
}
