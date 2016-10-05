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
		if (!showingWaitingRoom) {
			showingWaitingRoom = true;
			PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
		}
	}

	public void OnRoomConnected (bool success)
	{
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
		Debug.Log ("message recieved");
		communicator.ParseMessage (senderID, data);
	}

	public void OnLeftRoom ()
	{
		//throw new System.NotImplementedException ();
	}

	public void OnParticipantLeft (Participant participant)
	{
		//throw new System.NotImplementedException ();
	}

	public void OnPeersConnected (string[] participantIds)
	{
		//throw new System.NotImplementedException ();
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		// user left wins
	}


}
