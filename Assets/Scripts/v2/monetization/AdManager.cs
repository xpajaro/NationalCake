using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour{

	private string gameId = "";


	private string placementId = "rewardedVideo";


	public int adWatchCount;
	public const int ADS_BEFORE_REWARD = 5;

	public static AdManager Instance;

	void Awake (){
		if (Instance == null) {
			Instance = this;
		}
	}

	void Start () {   

		#if UNITY_IOS
		gameId = "1597503";
		#elif UNITY_ANDROID
		gameId = "1597504";
		#endif

		if (Advertisement.isSupported) {
			Advertisement.Initialize (gameId, true);
		}
	}

	public void ShowAd () {
		ShowOptions options = new ShowOptions();
		options.resultCallback = HandleShowResult;

		Advertisement.Show(placementId, options);
	}

	void HandleShowResult (ShowResult result){
		if(result == ShowResult.Finished) {
			Debug.Log("Video completed - Offer a reward to the player");

			adWatchCount++;

			if (adWatchCount == ADS_BEFORE_REWARD) {
				RewardUser ();
				adWatchCount = 0;

			} 

		}else if(result == ShowResult.Skipped) {
			Debug.LogWarning("Video was skipped - Do NOT reward the player");

		}else if(result == ShowResult.Failed) {
			Debug.LogError("Video failed to show");
		}
	}

	void RewardUser (){
		SessionManager.Instance.playerData.Revenue++;
		SessionManager.Instance.Save ();
	}
}
