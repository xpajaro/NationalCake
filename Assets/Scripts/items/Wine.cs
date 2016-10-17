using UnityEngine;
using System.Collections;

public class Wine : MonoBehaviour {

	int RESPAWN_TIME = 300; //5 secs
	int respawn_countdown;

	void OnCollisionEnter (Collision col)
	{
		if (GameSetup.isHost) {
			if (col.gameObject.name == "player" || col.gameObject.name == "enemy") {
				//hide
				//send to client to hide
				//show loading bubbles
				//add to speed
			}
		}
	}

	void Update(){
		//if beer is hidden, start countdown
		//after countdown, reattach beer
			//remove loading animation
			//reset countdown
	}
}
