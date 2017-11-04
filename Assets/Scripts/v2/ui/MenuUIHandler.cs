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
		PopupModalManager.Instance.Confirm (
			"Watch 2 video ads for $ reward", 
			AdManager.Instance.ShowAd);
	}

}

