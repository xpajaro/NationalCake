using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerController : NetworkBehaviour {

	//for player
	float MOVT_SPEED = 3; //liked 5
	float MOVT_CAP = 200;
	float MOVT_CAP_EFFECTIVE_RATIO = 50;
	string PLAYER_VELOCITY_PARAMETER = "playerVelocity";

	Vector2 movtStartPosition;
	Rigidbody2D playerBody;
	Animator animator;
	public AudioClip actorRunningSound;

	float currentXPosition;
	int currentDirection = 1; //1 is right, -1 is left


	public static PlayerController LocalInstance = null;    

	public override void OnStartLocalPlayer (){
		if (isLocalPlayer) {
			LocalInstance = this;
		}
	}

	void Start () {
		playerBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator> ();

		currentXPosition = transform.position.x;
		if (currentXPosition > 0) { //player on right should face inwards
			TurnPlayerAround ();
			currentDirection = -1;
		}
	}

	void Update() {
		if (isLocalPlayer) {
			animator.SetFloat (PLAYER_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		}

		FaceCorrectDirection ();
	}

	public void StartMovement (Vector2 startPos){
		movtStartPosition = startPos;
	}

	public void CompleteMovement (Vector2 endPos){
		Vector2 launchDir = CalculateLaunchDirection (endPos);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;

		Move (launchDir);
	}


	void Move (Vector2 launchDir){
		if (!GameState.gameEnded) {
			Vector2 impulse = CalculateImpulse (launchDir , WineBuzzLevel.PlayerBuzz);
			playerBody.AddForce (impulse, ForceMode2D.Impulse);

			//SoundManager.instance.PlaySingle (actorRunningSound, 1.5f);
		}
	}

	//-------------------------------------------
	// utilities
	//-------------------------------------------

	void FaceCorrectDirection () {
		float newXPosition = transform.position.x;

		if (newXPosition != currentXPosition) {
			int newDirection = (newXPosition > currentXPosition) ? 1 : -1;

			if (newDirection != currentDirection) {
				TurnPlayerAround ();
				currentDirection = newDirection;
			}

			currentXPosition = newXPosition;
		}
	}

	void TurnPlayerAround (){
		transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
	}


	Vector2 CalculateLaunchDirection (Vector2 movtEndPoint){
		Vector2 launchDir =  movtStartPosition - movtEndPoint ;

		if (launchDir.magnitude > MOVT_CAP) {
			launchDir = launchDir/ launchDir.magnitude * MOVT_CAP;
		}

		return launchDir;
	}

	Vector2 CalculateImpulse (Vector2 launchDir, float wineLevel){
		return launchDir * MOVT_SPEED * wineLevel; 
	}

}
