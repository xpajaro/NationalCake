using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public class FirebaseDB : MonoBehaviour{
	DatabaseReference dbRef;

	public static FirebaseDB Instance;

	const string HIGHSCORE_KEY = "highscore";

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}


	public void Start () {
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://national-cake.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("National-Cake-7bf325ab7af9.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("game-356@national-cake.iam.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password("notasecret");

		dbRef = FirebaseDatabase.DefaultInstance.RootReference;
	}


	public void GetPlayer (string playerId, UnityAction<DataSnapshot> action){
		FirebaseDatabase.DefaultInstance
			.GetReference("Players")
			.Child(playerId)
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("Get Player error - " + task.Exception.Message);

				} else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;

					action.Invoke(snapshot);
				}
			});
	}

	public void SavePlayer (Player player){
		string jsonPlayer = JsonUtility.ToJson (player);

		dbRef.Child ("Players").Child (player.Id).SetRawJsonValueAsync (jsonPlayer);
	}


	public void GetHighscore (UnityAction<int> action){
		FirebaseDatabase.DefaultInstance
			.GetReference(HIGHSCORE_KEY)
			.Child(Utilities.CurrentUnixDate().ToString())
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					Debug.Log("Get highscore error - " + task.Exception.Message);
					action.Invoke(0);

				} else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;

					string val = snapshot.Value != null ? snapshot.Value.ToString() : "0";
					int intVal = int.Parse(val);
					Debug.Log (intVal);

					action.Invoke (intVal);
				}
			});
	}


	public void SaveHighscore (int score){
		string scoreKey = Utilities.CurrentUnixDate ().ToString ();

		dbRef.Child (HIGHSCORE_KEY).Child(scoreKey).SetValueAsync (score);
	}

}
