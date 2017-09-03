using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeUIHandler : MonoBehaviour {

	const float LOAD_MENU_COUNTDOWN = 2f;

	void Start () {
		Invoke ("LoadMenu", LOAD_MENU_COUNTDOWN);
	}

	void LoadMenu () {

		SceneManager.LoadScene (Constants.MENU_SCENE);
	}
}
