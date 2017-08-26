using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkHelper : NetworkManager {

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {	
		base.OnServerAddPlayer(conn, playerControllerId);

		Debug.Log ("players added");

		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME)) {
			if (numPlayers == 2) {
				ServerChangeScene (Constants.MAIN_SCENE_NAME);
			}
		}
	}

}
