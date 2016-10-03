using UnityEngine;    
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;
using System;
using System.Text;

public class NetworkManager : MonoBehaviour {

	public static string HELLO_MESSAGE  = "hello";
	public static string MESSAGE_DIVIDER  = "->";
	public enum MessageTypes {mvmt, item};

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
		string msg = System.Text.Encoding.Default.GetString(msgBytes);
		Debug.Log ("new message" + msg);

		if (msg.Equals (HELLO_MESSAGE)) {
			GameManager.ChooseHost (senderID);
			GameManager.StartGame ();
		} else {
			RouteMessage (msg);
		}
	}

	static void RouteMessage (String msg){
		String[] msgParts = msg.Split ( new [] {MESSAGE_DIVIDER}, 2, StringSplitOptions.None);

		MessageTypes msgType = GetMessageType (msgParts);
		String msgContent = msgParts[1];

		if (msgType == MessageTypes.mvmt) {
			GameControls.MoveEnemy (msgContent);
		} else if (msgType == MessageTypes.item){
			
		}
	
	}

	public static new void SendMessage (String msg){
		byte[] msgBytes = Encoding.ASCII.GetBytes (msg);
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (false, msgBytes);
	}

	public static void SendMessage (bool reliable, String msg){
		byte[] msgBytes = Encoding.ASCII.GetBytes (msg);
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll (reliable, msgBytes);
	}


	//-------------------------------------------
	// Utilities
	//-------------------------------------------


	static MessageTypes GetMessageType(String[] msgParts){
		if (MessageTypes.item.ToString().Equals (msgParts [0]) ) {
			return MessageTypes.item;
		} else { //if (MessageTypes.mvmt.CompareTo (msgParts [0]) == 0){
			return MessageTypes.mvmt;
		}
	}

}
