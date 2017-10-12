using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;

public class MenuSceneManager : MonoBehaviour {
	public GameObject loginPanel;
	public Text txtCountdown;

	void Start () {
		LoginIfNewUser ();
		SoundManager.Instance.waterAmbienceSource.Play ();
	}

	void Update (){
		int hoursLeft = 24 - DateTime.Now.Hour;
		int minutesLeft = 60 - DateTime.Now.Minute;
		int secondsLeft = 60 - DateTime.Now.Second;
		txtCountdown.text = string.Format ("{0}:{1}:{2}", hoursLeft, minutesLeft, secondsLeft);
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
