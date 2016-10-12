using UnityEngine;    
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour {

	public static bool isHost;

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

	public static void ChooseHost (string otherPlayerID){
		Debug.Log ("choose host");
		if (String.Compare (PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId, otherPlayerID) < 0) {
			isHost = true;
			Debug.Log ("host chosen - me");
		} else {
			Debug.Log ("host chosen - not me");
		}
		Debug.Log ("choose host done");
	}

	public static void StartGame (){
		Debug.Log ("start game");
		SceneManager.LoadScene(1);
		Debug.Log ("start game done");
	}

}
