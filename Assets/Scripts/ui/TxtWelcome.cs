using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;
using System.Collections;

public class TxtWelcome : MonoBehaviour {

	void Start () {
		Text txt = GetComponent<Text> ();
		txt.text = "hi " + this.getUsername().ToLower() + "!";
	}

	string getUsername(){
		return ""; //Social.localUser.userName;
	}
}
