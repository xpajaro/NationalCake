using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class TxtWelcome : MonoBehaviour {

	void Start () {
		Text txt = GetComponent<Text> ();
		txt.text = "hi " + this.getUsername() + "!";
	}

	string getUsername(){
		return Social.localUser.userName;
	}
}
