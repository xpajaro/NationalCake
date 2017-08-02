using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerController : NetworkBehaviour {

	//for player
	const float BASIC_MOVT_FORCE = 5; //use 3

	Vector2 movtStartPosition;
	Rigidbody2D playerBody;
	SpeedManager speedManager;
	public AudioClip actorRunningSound;

	public bool isSwimming; //set in swimming class
	float currentXPosition;
	int currentDirection = 1; //1 is right, -1 is left


	public static PlayerController PlayerInstance = null;    
	public static PlayerController EnemyInstance = null;    

	void Start () {

		SetupInstances ();

		playerBody = GetComponent<Rigidbody2D>();
		speedManager = GetComponent<SpeedManager> ();

		SetupAfterSpawn ();
	}

	void SetupInstances (){
		if (tag.Equals (Constants.PLAYER_NAME)) {
			PlayerInstance = this;

		} else {
			EnemyInstance = this;
		}
	}

	void FixedUpdate() {
		FaceFrontOnMove ();
	}


	public void SetupAfterSpawn (){
		currentXPosition = transform.position.x; 
		FaceFrontOnSpawn ();
	}


	public void Move (Vector2 launchDirection){
		if (!GameState.gameEnded && !isSwimming) {
			
			Vector2 impulse = launchDirection * BASIC_MOVT_FORCE * speedManager.currentSpeed; 
			playerBody.AddForce (impulse, ForceMode2D.Impulse);

			SoundManager.instance.PlaySingle (actorRunningSound, 1.5f);
		}
	}

	//-------------------------------------------
	// setup
	//-------------------------------------------


	public void FaceFrontOnSpawn(){
		int direction = (transform.position.x > 0) ? -1 : 1;

		TurnPlayerAround (direction);
		currentDirection = direction;
	}


	//-------------------------------------------
	// movement
	//-------------------------------------------

	void FaceFrontOnMove () {
		float newXPosition = transform.position.x;

		if (!newXPosition.Equals(currentXPosition)) {
			int newDirection = (newXPosition > currentXPosition) ? 1 : -1;

			if (newDirection != currentDirection) {
				TurnPlayerAround (newDirection);
				currentDirection = newDirection;
			}

			currentXPosition = newXPosition;
		}
	}


	void TurnPlayerAround (int direction){
		transform.localScale = new Vector3(direction, 1, 1) ;
	}



}
