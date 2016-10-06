using UnityEngine;    
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class NetworkListener : RealTimeMultiplayerListener {
	bool showingWaitingRoom = false;
	Communicator communicator;

	public NetworkListener(){
		communicator = new Communicator ();
	}

	public void OnRoomSetupProgress(float progress) {
		// show the default waiting room.
		Debug.Log ("on room setup progresss");
		if (!showingWaitingRoom) {
			showingWaitingRoom = true;
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
		}
	}

	public void OnRoomConnected (bool success)
	{
		Debug.Log ("on room connected");
		if (success) {
			communicator.SayHello ();
		} else {
			// Error!
			// ...show error message to user...
			//or reload the invite pages??
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderID, byte[] data)
	{
		Debug.Log ("on realtime message recieved");
		//Debug.Log (System.Text.Encoding.UTF8.GetString(data));
		communicator.ParseMessage (senderID, data);
	}

	public void OnLeftRoom ()
	{
		Debug.Log ("on left room");
		//throw new System.NotImplementedException ();
	}

	public void OnParticipantLeft (Participant participant)
	{
		Debug.Log ("on participant left");
		//throw new System.NotImplementedException ();
	}

	public void OnPeersConnected (string[] participantIds)
	{
		Debug.Log ("on peers connected");
		//throw new System.NotImplementedException ();
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		Debug.Log ("on peers disconnected");
		// user left wins
	}


}
