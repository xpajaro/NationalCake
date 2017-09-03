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

	const string ACCESS_TOKEN = "ACCESS_TOKEN";

	public void SaveAccessToken(string token){
		Debug.Log("save ACCESS_TOKEN - " + token);
		PlayerPrefs.SetString(ACCESS_TOKEN, token);
	}

	public string GetAccessToken(){

		Debug.Log("get ACCESS_TOKEN - x" + PlayerPrefs.GetString (ACCESS_TOKEN) + "x");
		return PlayerPrefs.GetString (ACCESS_TOKEN);
	}
}
