using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class MenuSceneManager : MonoBehaviour {
	GameObject loginPanel;

	void Start () {
		LoginIfNewUser ();
	}
	
	// Update is called once per frame
	void LoginIfNewUser () {

		if (AccessToken.CurrentAccessToken == null) {
			loginPanel.SetActive (true);
		} else {
			loginPanel.SetActive (false);
		}
	}
}
