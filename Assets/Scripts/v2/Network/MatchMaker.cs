using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using System.Collections.Generic;
using System;

public class MatchMaker : MonoBehaviour {

	List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
	bool matchCreated;
	NetworkMatch matchMaker;

	// remember to change port
	const int SERVER_PORT = 30010;

	public static MatchMaker Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

		matchMaker = gameObject.AddComponent<NetworkMatch>();
	}

	void OnGUI()
	{ }

	public void StartNewMatch(){
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
				const int matchSize = 2 , skillLevel= 0 , requestDomain= 0;
				const string password = "" , publicClientAddress= "" , privateClientAddress= "";

				matchMaker.JoinMatch (matchList [0].networkId, password, publicClientAddress,
					privateClientAddress, skillLevel, requestDomain, OnMatchJoined);
			
			} else {
				Debug.Log(String.Format("No matches available {0}", matchList.Count));
				CreateMatch ();
			}

		} else {
			Debug.Log(String.Format("Match list failed {0}, {1}", success, matchList));
			CreateMatch ();
		}
	}


	private void OnMatchJoined (bool success, string extendedInfo, MatchInfo matchInfo){
		if (success) {
			Debug.Log("Join match succeeded");

			if (matchCreated){
				Debug.LogWarning("Match already set up, aborting...");
				return;
			}

			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

			NetworkClient myClient = new NetworkClient();
			myClient.RegisterHandler(MsgType.Connect, OnConnected);
			myClient.Connect(matchInfo);

		} else {
			Debug.LogError("Join match failed");
		}
	}


	private void CreateMatch() {
		string roomName = Guid.NewGuid ().ToString();
		const int matchSize = 2, skillLevel = 0, requestDomain =0;
		const bool advertiseMatch = true;
		const string password = "", publicClientAddress = "", privateClientAddress = "";

		matchMaker.CreateMatch(roomName, matchSize, advertiseMatch,
			password, publicClientAddress, privateClientAddress, skillLevel,
			requestDomain, OnMatchCreate);

	}


	private void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo) {
		if (success)
		{
			Debug.Log("Create match succeeded");
			matchCreated = true;

			Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);
			
			NetworkServer.Listen(SERVER_PORT);

		} else {
			Debug.Log(String.Format("Create match failed {0}, {1}", success, matchInfo));
		}
	}


	private void OnConnected(NetworkMessage msg) {
		Debug.Log("Connected!");
	}
}
