using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbySceneManager : MonoBehaviour {
	public Text txtRoomName, txtRoomDesc, txtRoomCost, txtRoomReward;
	private int currentRoomID = 0;
	private Room[] rooms = Room.GetAllRooms ();

	public static LobbySceneManager Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}
	// Use this for initialization
	void Start () {
		UpdateRoom ();
	}

	public void PreviousRoom(){
		if (currentRoomID > 0){
			currentRoomID--;
			UpdateRoom ();
		}
	}

	public void NextRoom(){
		if (currentRoomID < rooms.Length - 1){
			currentRoomID++;
			UpdateRoom ();
		}
	}

	
	// Update is called once per frame
	void UpdateRoom () {
		Room currRoom = rooms [currentRoomID];

		txtRoomName.text = currRoom.Name;
		txtRoomDesc.text = currRoom.Description;
		txtRoomCost.text = currRoom.Budget;
		txtRoomReward.text = currRoom.Recovery;

	}
}
