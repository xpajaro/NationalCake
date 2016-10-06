using UnityEngine;    
using System.Collections;
using GooglePlayGames;
using System;

public class UIHandler : MonoBehaviour {

	NetworkManager networkManager = new NetworkManager ();	

	public void InviteUser (){
		Debug.Log ("invite player");
		networkManager.InvitePlayer();
		Debug.Log ("invite player done");
	}

	public void CheckInbox (){
		Debug.Log ("check inbox");
		networkManager.CheckInbox();
		Debug.Log ("check inbox done");
	}


}
