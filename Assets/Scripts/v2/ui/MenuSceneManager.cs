using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Facebook.Unity;
using System;

public class MenuSceneManager : MonoBehaviour {
	public GameObject loginPanel;
	public Text txtCountdown, txtRank, txtRevenue;
	public Image meterFiller;

	public static MenuSceneManager Instance;

	Player player;

	void Awake() {
		if (Instance == null) {
			Instance = this;

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}

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
			UpdateAchievements ();
		}
	}

	public void UpdateAchievements(){
		player = LocalStorage.Instance.Load ();

		if (player != null) {
			if (!player.LastLogin.Date.Equals (DateTime.Now.Date)) {
				player.currentScore = new Score ();

				LocalStorage.Instance.Save (player);
			}

			InvokeRepeating("UpdateMeter", 0, 2f);


			txtRank.text = player.currentScore.Rank == null ? 
				Constants.NO_DATA : player.currentScore.Rank;

			txtRevenue.text = player.currentScore.Revenue == 0 ? 
				Constants.NO_DATA : player.currentScore.Revenue.ToString();
			
		}
	}

	void UpdateMeter (){
		FirebaseDB.Instance.GetHighscore (FillMeter);
	}

	void FillMeter (int serverHighscore){
		int score = player != null ? player.currentScore.Revenue : 0;
		meterFiller.fillAmount = (float) score / serverHighscore ;
	}
}
