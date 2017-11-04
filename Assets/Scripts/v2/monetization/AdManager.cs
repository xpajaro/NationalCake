using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour{

	#if UNITY_IOS
	private string gameId = "1597503";
	#elif UNITY_ANDROID
	private string gameId = "1597504";
	#endif

	private string placementId = "rewardedVideo";

	public static AdManager Instance;

	private int adWatchCount;

	void Awake (){
		if (Instance == null) {
			Instance = this;
		}
	}

	void Start () {    
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

			if (adWatchCount == 2) {
				RewardUser ();
				adWatchCount = 0;

			} else {
				ShowAd ();
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
