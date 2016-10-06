using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public static bool isHost;

	public static void StartGame (){
		Debug.Log ("start game");
		SceneManager.LoadScene(1);
		Debug.Log ("start game done");
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

}
