using UnityEngine;    
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour {

	void Start (){
		SetupGooglePlay ();
		SignIn ();
	}
		
	static void SetupGooglePlay (){
		Debug.Log ("setup google play");
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();

		PlayGamesPlatform.InitializeInstance (config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate (); // Activate the Google Play Games platform
		Debug.Log ("setup google play done");
	}

	static void SignIn(){
		NetworkManager.Instance.SignIn ();
	}

}
