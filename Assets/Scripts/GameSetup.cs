﻿using UnityEngine;    
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System;

public class GameSetup : MonoBehaviour {

	public static bool isHost;
	static int GAME_SCENE = 2;
	static int ENG_GAME_SCENE = 3;

	void Start (){
	}


	public static void setupGame(){
		SetupGooglePlay ();
		SignIn ();
	}

	static void SetupGooglePlay (){
		//Debug.Log ("setup google play");
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder ().Build ();

		PlayGamesPlatform.InitializeInstance (config);
		//PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate (); // Activate the Google Play Games platform
		//Debug.Log ("setup google play done");
	}

	static void SignIn(){
		NetworkManager.Instance.SignIn ();
	}

	public static void ChooseHost (string otherPlayerID){
		//Debug.Log ("choose host");
		if (String.Compare (PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId, otherPlayerID) < 0) {
			isHost = true;
		} 
		//Debug.Log ("choose host done");
	}

	public static void StartGame (){
		//Debug.Log ("start game");
		SceneManager.LoadScene(GAME_SCENE);
		SoundManager.instance.ToggleMusic (true);
		//Debug.Log ("start game done");
	}


	public static void LeaveGame(){
		PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		NextScene ();
	}

	public static void NextScene(){
		SceneManager.LoadScene (ENG_GAME_SCENE);
	}

}
