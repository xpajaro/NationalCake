using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class MatchMaker : MonoBehaviour {

	public Text lblStatus;

	public bool isHost;
	public MatchInfo currentMatch;

	NetworkMatch matchMaker;

	// remember to change port
	const int SERVER_PORT = 30010;

	public static MatchMaker Instance;

	void Awake() {
		if (Instance == null) {
			Debug.Log("awoken");
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Debug.Log("instance destroyed");
			Destroy (gameObject);
		}

	}

	private void SetupStatusLabel(){
		if (lblStatus == null){
			lblStatus = GameObject.Find("Canvas/lblStatus").GetComponent<Text>();
		}
	}


	public void StartNewMatch(){
		SetupStatusLabel ();

		NetworkManager.singleton.StartMatchMaker ();
		matchMaker = NetworkManager.singleton.matchMaker;

		JoinOrCreateMatch ();
	}


	private void JoinOrCreateMatch() {
		const int startPage = 0 , pageSize= 5, skillLevel=0, requestDomain=0;
		bool showPrivateMatches = false;
		const string nameFilter= "";

		matchMaker.ListMatches(startPage, pageSize, nameFilter,
			showPrivateMatches, skillLevel, requestDomain, OnMatchList);
	}


	private void OnMatchList (bool success, string extendedInfo, List<MatchInfoSnapshot> matchList){
		if (success && matchList != null) {
			Debug.Log("Match list succeeded");

			if (matchList.Count > 0) {
				lblStatus.text = ".. joining a game ..";

				const int matchSize = 2 , skillLevel= 0 , requestDomain= 0;
				const string password = "" , publicClientAddress= "" , privateClientAddress= "";

				matchMaker.JoinMatch (matchList [0].networkId, password, publicClientAddress,
					privateClientAddress, skillLevel, requestDomain, OnMatchJoined);

				isHost = false;
			
			} else {
				Debug.Log(String.Format("No matches available {0}", matchList.Count));
				NetworkServer.Reset ();
				CreateMatch ();
			}

		} else {
			Debug.Log(String.Format("Match list failed {0}, {1}", success, matchList));
			NetworkServer.Reset ();
			CreateMatch ();
		}
	}


	private void OnMatchJoined (bool success, string extendedInfo, MatchInfo matchInfo){
		if (success) {
			lblStatus.text = ".. waiting for opponent ..";

			Debug.Log("Join match succeeded");
			currentMatch = matchInfo;

			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

			NetworkManager.singleton.StartClient ();


		} else {
			Debug.LogError("Join match failed");
		}
	}


	private void CreateMatch() {
		lblStatus.text = ".. starting a game ..";

		string roomName = Guid.NewGuid ().ToString();
		const int matchSize = 2, skillLevel = 0, requestDomain =0;
		const bool advertiseMatch = true;
		const string password = "", publicClientAddress = "", privateClientAddress = "";

		matchMaker.CreateMatch(roomName, matchSize, advertiseMatch,
			password, publicClientAddress, privateClientAddress, skillLevel,
			requestDomain, OnMatchCreated);

	}


	private void OnMatchCreated(bool success, string extendedInfo, MatchInfo matchInfo) {
		if (success){
			lblStatus.text = ".. waiting for opponent ..";

			isHost = true;

			Debug.Log("Create match succeeded");
			currentMatch = matchInfo;

			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

			NetworkServer.Listen(SERVER_PORT);
			NetworkManager.singleton.StartHost ();

		} else {
			Debug.Log(String.Format("Create match failed {0}, {1}", success, matchInfo));
		}
	}

	public void DestroyCurrentMatch(){
		const int requestDomain = 0;

		if (MatchMaker.Instance.currentMatch != null) {
			if (isHost) {
				matchMaker.DestroyMatch 
				(currentMatch.networkId, requestDomain, OnDestroyMatch);

			} else {
				matchMaker.DropConnection 
				(currentMatch.networkId, requestDomain, 0, OnDestroyMatch);
			}

		} else{
			MatchMaker.Instance.currentMatch = null;
			CleanupNetwork ();
		}
	}


	public void OnDestroyMatch(bool success, string extendedInfo){
		CleanupNetwork ();
		MatchMaker.Instance.currentMatch = null;

		if (success) {
			Debug.Log ("Match destroyed - call");

		} else {
			Debug.Log ("Match not destroyed - call");
		}
	}
		
	private void CleanupNetwork(){
		if (isHost) {
			NetworkManager.singleton.StopHost ();
		}

		NetworkManager.singleton.StopClient ();
		NetworkManager.singleton.StopMatchMaker ();

		Network.Disconnect ();
	}
}
