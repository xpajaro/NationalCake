using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class MenuSceneManager : MonoBehaviour {
	public GameObject loginPanel;

	void Start () {
		LoginIfNewUser ();
		SoundManager.Instance.waterAmbienceSource.Play ();
	}


	void LoginIfNewUser () {

		if (AccessToken.CurrentAccessToken == null) {
			loginPanel.SetActive (true);
			FirebaseLogin.Instance.loginPanel = loginPanel;

		} else {
			loginPanel.SetActive (false);
		}
	}
}
