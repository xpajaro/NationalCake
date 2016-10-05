using UnityEngine;    
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour {

	NetworkListener networkListener ;

	public NetworkManager(){
		networkListener = new NetworkListener();
	}

	public void InvitePlayer (){
		Debug.Log ("invite player");
		Social.localUser.Authenticate ((bool success) => {
			const int MinOpponents = 1 , MaxOpponents= 1;
			const int GameVariant = 0;

			PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen (MinOpponents, MaxOpponents,
				GameVariant, networkListener);
		});
		Debug.Log ("invite player done");
	}

	public void CheckInbox (){

		Social.localUser.Authenticate ((bool success) => {
			PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(networkListener);
		});
	}


	public void SendMessage (byte[] msgBytes){
		Debug.Log ("send message");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, msgBytes);
		Debug.Log ("send message done");
	}

	//-------------------------------------------
	// Utilities
	//-------------------------------------------


}
