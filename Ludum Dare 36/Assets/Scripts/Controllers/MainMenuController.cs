using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {


	public void OnPlay() {
		//SceneManager.LoadSceneAsync ("Main");
		SceneManager.LoadSceneAsync(1);
	}


	public void OnExit() {
		Application.Quit ();
	}


}
