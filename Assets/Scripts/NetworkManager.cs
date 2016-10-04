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

	public enum MessageTypes {hello, mvmt, item, state};

	static NetworkListener networkListener = new NetworkListener();


	public static void InvitePlayer (){

		Social.localUser.Authenticate ((bool success) => {
			const int MinOpponents = 1 , MaxOpponents= 1;
			const int GameVariant = 0;

			PlayGamesPlatform.Instance.RealTime.CreateWithInvitationScreen (MinOpponents, MaxOpponents,
				GameVariant, networkListener);
		});
	}

	public static void CheckInbox (){

		Social.localUser.Authenticate ((bool success) => {
			PlayGamesPlatform.Instance.RealTime.AcceptFromInbox(networkListener);
		});
	}

	public static void ParseMessage (string senderID, byte[] msgBytes){
		Dictionary<string, object> msg = Utilities.Deserialize(msgBytes);

		string msgType = msg [Communicator.MESSAGE_TYPE].ToString ();

		if ( msgType.Equals (MessageTypes.hello.ToString()) ) {
			GameManager.ChooseHost (senderID);
			GameManager.StartGame ();
		} else {
			RouteMessage (msg);
		}
	}

	static void RouteMessage (Dictionary<string, object> msg){

		string msgType = msg [Communicator.MESSAGE_TYPE].ToString ();

		if ( msgType.Equals (MessageTypes.mvmt.ToString()) ) {
			//GameControls.MoveEnemy (msgContent);
		} else if (msgType.Equals (MessageTypes.state.ToString()) ){
			
		}
	
	}

	public static new void SendMessage (byte[] msgBytes){
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, msgBytes);
	}

	//-------------------------------------------
	// Utilities
	//-------------------------------------------


}
