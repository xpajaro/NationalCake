using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Match;

public class NetworkHelper : NetworkManager {

	const string DISCONNECTION_MESSAGE = "connection failed";


	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {	
		base.OnServerAddPlayer(conn, playerControllerId);

		Debug.Log ("players added");

		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME)) {
			if (numPlayers == 2) {
				ServerChangeScene (Constants.MAIN_SCENE_NAME);
			}
		}
	}

	public override void OnClientError(NetworkConnection conn, int errorCode) {
		base.OnClientError (conn, errorCode);
		Debug.Log ("client connection error - " + errorCode);
	}


	public override void OnClientDisconnect (NetworkConnection conn) {
		base.OnClientDisconnect (conn);

		Debug.Log ("client disconnected");

		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME)) {
			UIHandler.ReturnToMenu ();
		}

	}

	public override void OnServerDisconnect (NetworkConnection conn) {
		base.OnServerDisconnect (conn);

		Debug.Log ("server disconnected");

		PopupModalManager.Instance.Show (DISCONNECTION_MESSAGE, ReturnToMenu, "okay");
	}

	private void ReturnToMenu(){
		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME)) {
			UIHandler.ReturnToMenu ();
		} else {
			if (GameState.gameWon) {
				UIHandler.GoToWinnerScreen ();
			} else {
				UIHandler.GoToLoserScreen ();
			}
		}
	}

}
