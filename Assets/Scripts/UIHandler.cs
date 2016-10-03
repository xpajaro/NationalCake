using UnityEngine;    
using System.Collections;
using GooglePlayGames;
using System;

public class UIHandler : MonoBehaviour {


	public void InviteUser (){
		NetworkManager.InvitePlayer();
	}

	public void CheckInbox (){
		NetworkManager.CheckInbox();
	}


}
