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
		playerData = LocalStorage.Instance.Load ();
	}

}
