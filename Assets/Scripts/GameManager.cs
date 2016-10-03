using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameManager : MonoBehaviour {

	public static bool isHost;

	public static void StartGame (){
		SceneManager.LoadScene(1);
	}

	public static void ChooseHost (string otherPlayerID){
		if ( String.Compare(Social.localUser.id, otherPlayerID) < 0) {
			isHost = true;
		}
	}

}
