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
	}
		
	static void SetupGooglePlay (){
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();

		PlayGamesPlatform.InitializeInstance (config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate (); // Activate the Google Play Games platform
	}

}
