using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;
using UnityEngine.Events;

public class FirebaseDB : MonoBehaviour{
	DatabaseReference dbRef;

	public static FirebaseDB Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}


	public void Init () {
		// Set this before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://national-cake.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName("National Cake-a228ff031bca.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail("national-cake@appspot.gserviceaccount.com");
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

	public void CreatePlayer (Player player){
		string jsonPlayer = JsonUtility.ToJson (player);

		dbRef.Child ("Players").Child (player.Id).SetRawJsonValueAsync (jsonPlayer);
	}

	public void UpdatePlayer (string playerId, string field, string value){
		dbRef.Child ("Players").Child (playerId).Child(field).SetValueAsync (value);
	}
}
