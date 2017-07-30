using UnityEngine;    
//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using System.Collections;

public class NetworkListener  {
	bool showingWaitingRoom = false;


	public void OnRoomSetupProgress(float progress) {
		// show the default waiting room.
		//Debug.Log ("on room setup progresss");
		if (!showingWaitingRoom) {
			//PlayGamesPlatform.Instance.RealTime.ShowWaitingRoomUI();
			showingWaitingRoom = true;
		}
	}

	public void OnRoomConnected (bool success)
	{
		//Debug.Log ("on room connected");
		if (success) {
			Communicator.Instance.SayHello ();

		} else {
			// Error!
			// ...show error message to user...
			//or reload the invite pages??
		}
	}

	public void OnRealTimeMessageReceived (bool isReliable, string senderID, byte[] data)
	{
		//Debug.Log ("on realtime message recieved");
		//Debug.Log (System.Text.Encoding.UTF8.GetString(data));
		Communicator.Instance.ParseMessage (senderID, data);
	}

	public void OnLeftRoom ()
	{
		Debug.Log ("conner - onLeftRoom");
		HandleGameTransition (true);
	}

	public void OnParticipantLeft (string participant)
	{
		Debug.Log ("conner - OnParticipantLeft");
		HandleGameTransition (false);
	}

	public void OnPeersConnected (string[] participantIds)
	{
		//Debug.Log ("on peers connected");
		//throw new System.NotImplementedException ();
	}

	public void OnPeersDisconnected (string[] participantIds)
	{
		Debug.Log ("conner - OnPeersDisconnected");
		HandleGameTransition (false);
	}

	void HandleGameTransition (bool thisUserDisconnected){
		Scene scene = SceneManager.GetActiveScene();

		if (!thisUserDisconnected) {
			//PlayGamesPlatform.Instance.RealTime.LeaveRoom ();
		}

		if (scene.name.Equals (Constants.GAME_SCENE) && !GameState.gameEnded) {
			FinishGame (!thisUserDisconnected);
		}

	}

	void FinishGame (bool gameWon){
		GameState.gameEnded = true;
		GameState.gameWon = gameWon;

		GameSetup.NextScene ();
	}

}
