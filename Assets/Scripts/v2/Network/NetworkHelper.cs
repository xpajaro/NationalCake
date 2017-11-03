using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Match;

public class NetworkHelper : NetworkManager {

	const string DISCONNECTION_MESSAGE = "Opponent left the game.";


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

		if (GameState.gameEnded == false) {
			SessionManager.Instance.UpdateReserves (true);
			PopupModalManager.Instance.Show (DISCONNECTION_MESSAGE, ReturnToMenu, "okay");
		}

		GameState.gameEnded = true;
	}

	public override void OnServerDisconnect (NetworkConnection conn) {
		base.OnServerDisconnect (conn);

		Debug.Log ("server disconnected");

		if (GameState.gameEnded == false) {
			SessionManager.Instance.UpdateReserves (true);
			PopupModalManager.Instance.Show (DISCONNECTION_MESSAGE, ReturnToMenu, "okay");
		}

		GameState.gameEnded = true;
	}

	private void ReturnToMenu(){
		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME)) {
			UIHandler.ReturnToMenu ();

		} else {
			UIHandler.GoToWinnerScreen ();
		}
	}

}
