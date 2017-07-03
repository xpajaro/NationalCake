using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("EnterGame", 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			//consider animation to give user feedback
			this.EnterGame();
		}
	}

	void EnterGame(){
		GameSetup.setupGame ();
	}
}
