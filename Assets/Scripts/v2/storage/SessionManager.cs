using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionManager : MonoBehaviour {

	public PlayerData playerData =  new PlayerData();
	public Room currentRoom;

	public static SessionManager Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}


	void Start () {
		LoadPlayerData ();
	}

	public void LoadPlayerData(){
		playerData = LocalStorage.Instance.Load ();
	}

	public void UpdateReserves(bool localPlayerWonGame){

		if (localPlayerWonGame) {
			playerData.Revenue += currentRoom.Recovery;
			playerData.WinCount++;
		} else {
			playerData.Revenue -= currentRoom.Budget;
		}

		LocalStorage.Instance.Save (playerData);
	}

	public void Save(){
		LocalStorage.Instance.Save (playerData);
	}

}
