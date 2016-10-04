using UnityEngine;    
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class NetworkListener : RealTimeMultiplayerListener {
	private bool showingWaitingRoom = false;

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
			Communicator.SayHello ();
		} else {
			// Error!
			// ...show error message to user...
			//or reload the invite pages??
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderID, byte[] data)
	{
		NetworkManager.ParseMessage (senderID, data);
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
