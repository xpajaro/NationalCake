using UnityEngine;    
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

public class NetworkManager  {

	//make singleton
	public static NetworkManager _instance;
	public static NetworkManager Instance {
		get {
			if (_instance == null) {
				_instance = new NetworkManager();
			}
			return _instance;
		}
	}

	NetworkListener networkListener = new NetworkListener();

	public void SignIn (){
		//Debug.Log ("sign in");

		if (! Social.localUser.authenticated) {
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					//Debug.Log ("We're signed in! Welcome " + Social.localUser.userName);
				} else {
					//Debug.Log ("Oh... we're not signed in.");
				}
			});
		} else {
			//Debug.Log ("You're already signed in.");
		}
	}

	public void SignOut() {
		PlayGamesPlatform.Instance.SignOut ();
	}

	public void InvitePlayer (){
		//Debug.Log ("invite player");
		const int MinOpponents = 1 , MaxOpponents= 1;
		const int GameVariant = 0;

		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen (MinOpponents, MaxOpponents,
			GameVariant, networkListener);
		//Debug.Log ("invite player done");
	}

	public void CheckInbox (){
		//Debug.Log ("check inbox");
		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox (networkListener);
		//Debug.Log ("check inbox done");
	}

	public void SendFastMessage (byte[] msgBytes){
		SendMessage (msgBytes, false);
	}


	public void SendMessage (byte[] msgBytes, bool reliable){
		//Debug.Log ("send message");
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (reliable, msgBytes);
		//Debug.Log ("send message done");
	}

	//-------------------------------------------
	// Utilities
	//-------------------------------------------


}
