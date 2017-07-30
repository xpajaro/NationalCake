using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalStorage {

	public static LocalStorage _instance;
	public static LocalStorage Instance {
		get {
			if (_instance == null) {
				_instance = new LocalStorage();
			}
			return _instance;
		}
	}


	const string USER_ID = "USER_ID";
	const string USER_DISPLAY_NAME = "USER_DISPLAY_NAME"; 



	public void SaveUserID(string ID){
		Debug.Log("save userID - " + ID);
		PlayerPrefs.SetString(USER_ID, ID);
	}

	public string GetUserID(){

		Debug.Log("get userID - x" + PlayerPrefs.GetString (USER_ID) + "x");
		return PlayerPrefs.GetString (USER_ID);
	}

	public void SaveUserDisplayName(string displayName){
		Debug.Log("save display name - " + displayName);
		PlayerPrefs.SetString(USER_DISPLAY_NAME, displayName);
	}

	public string GetUserDisplayName(){

		Debug.Log("display name - " + PlayerPrefs.GetString (USER_DISPLAY_NAME));
		return PlayerPrefs.GetString (USER_DISPLAY_NAME);
	}
}
