using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class JerryCan : NetworkBehaviour {

	const float RESPAWN_TIME = 10f; 
	const float SPEED_BOOST = 3f; //3.5 was good
	const float MAX_BOOST = 20f;

	SpriteRenderer spriteRenderer;


	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{	
		string objName = col.gameObject.name;

		Presenter.Detach (gameObject, spriteRenderer);
		Invoke("ShowAgain", RESPAWN_TIME);

		if (IsLocalPlayer (objName)) {
			SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.GASSED_UP);
		}

		if (( objName.StartsWith (Constants.PLAYER_NAME) ||
			  objName.StartsWith (Constants.ENEMY_NAME)) ) {

			SpeedUp (col.gameObject);
		}
	}


	void SpeedUp (GameObject characterRef){
		SpeedManager speedManager  = characterRef.GetComponent<SpeedManager> ();

		float newSpeed = Math.Min( MAX_BOOST, speedManager.currentSpeed + SPEED_BOOST );
		speedManager.currentSpeed = newSpeed;
	}


	void ShowAgain (){
		Presenter.Attach (this.gameObject, spriteRenderer);
	}


	//-------------------------------------------
	// utilities
	//-------------------------------------------

	bool IsLocalPlayer (string objName){
		return (objName.StartsWith (Constants.PLAYER_NAME) && isServer) ||
		(objName.StartsWith (Constants.ENEMY_NAME) && !isServer);
	}
}
