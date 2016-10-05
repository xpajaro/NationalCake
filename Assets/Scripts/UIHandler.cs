using UnityEngine;    
using System.Collections;
using GooglePlayGames;
using System;

public class UIHandler : MonoBehaviour {

	NetworkManager networkManager;

	public UIHandler(){
		networkManager = new NetworkManager ();	
	}

	public void InviteUser (){
		networkManager.InvitePlayer();
	}

	public void CheckInbox (){
		networkManager.CheckInbox();
	}


}
