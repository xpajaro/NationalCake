using UnityEngine;    
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.BasicApi.Multiplayer;
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

	const int LAG_PROBE_COUNT = 5;
	LagMessage[] lagMessages = new LagMessage[LAG_PROBE_COUNT] ;

	public int networkLag = 0;
	int lagMessagesReceived = 0;

	public void SignIn (){
		//Debug.Log ("sign in");

		if (! Social.localUser.authenticated) {
			Social.localUser.Authenticate((bool success) => {
				if (success) {
					UIHandler.GotoMenuScene();
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
		//PlayGamesPlatform.Instance.SignOut ();
	}

	public void InvitePlayer (){
		//Debug.Log ("invite player");
		const int MinOpponents = 1 , MaxOpponents= 1;
		const int GameVariant = 0;

//		PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen (MinOpponents, MaxOpponents,
//			GameVariant, networkListener);
		//Debug.Log ("invite player done");
	}

	public void CheckInbox (){
		//Debug.Log ("check inbox");
//		PlayGamesPlatform.Instance.RealTime.AcceptFromInbox (networkListener);
		//Debug.Log ("check inbox done");
	}

	public void SendFastMessage (byte[] msgBytes){
		SendMessage (msgBytes, false);
	}


	public void SendMessage (byte[] msgBytes, bool reliable){
		//Debug.Log ("send message");
//		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (reliable, msgBytes);
		//Debug.Log ("send message done");
	}

	//send tags, wait for roundtrip and diff timesent/received
	// use a class to map tag, timesent and diff?
	public void PingForLag(){
		if (GameSetup.isHost) {
			for (int i = 0; i < LAG_PROBE_COUNT; i++) {
				char tag = (char)i;

				lagMessages [i] = new LagMessage (DateTime.Now, 0);

				Communicator.Instance.ShareServerGamestamp (tag);
			}
		}

	}

	public void CalculateLag(char tag) {
		int msgIndex = Convert.ToInt32(tag);

		LagMessage msg = lagMessages [msgIndex];
		msg.roundtripDuration = DateTime.Now.Subtract (msg.timeSent).Milliseconds;

		lagMessagesReceived++;
		networkLag = (networkLag + msg.roundtripDuration) / lagMessagesReceived;
		Debug.Log (String.Format("calculating lag {0} , msg: {1}", networkLag, lagMessagesReceived));
	}

	//-------------------------------------------
	// Utilities
	//-------------------------------------------


}
