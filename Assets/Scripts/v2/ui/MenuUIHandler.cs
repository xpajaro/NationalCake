using UnityEngine;    
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuUIHandler : MonoBehaviour {

	public void JoinGame(){
		SceneManager.LoadScene (Constants.STAGING_SCENE);
	}

	public void ShowRewardedAds(){
		int adsLeft = AdManager.ADS_BEFORE_REWARD - AdManager.Instance.adWatchCount;

		PopupModalManager.Instance.Confirm (
			String.Format("Watch {0} video ads for $2bn reward?", adsLeft), 
			AdManager.Instance.ShowAd);
	}

	public void BuyBailoutPackage(){
		PopupModalManager.Instance.Confirm (
			"Buy $10 billion bailout package?", 
			IAPManager.Instance.BuyProduct);
	}

}

