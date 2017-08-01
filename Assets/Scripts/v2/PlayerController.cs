using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerController : NetworkBehaviour {

	//for player
	const float MOVT_SPEED = 5; //use 3
	const float MOVT_CAP = 200;
	const float MOVT_CAP_EFFECTIVE_RATIO = 50;
	const string PLAYER_VELOCITY_PARAMETER = "playerVelocity";
	const string ENEMY_VELOCITY_PARAMETER = "enemyVelocity";

	Vector2 movtStartPosition;
	protected Rigidbody2D playerBody;
	protected Animator animator;
	public AudioClip actorRunningSound;
	public GameObject marker;

	public bool isSwimming; //set in swimming class
	float currentXPosition;
	int currentDirection = 1; //1 is right, -1 is left


	public static PlayerController PlayerInstance = null;    
	public static PlayerController EnemyInstance = null;    

	void Start () {

		SetupInstances ();
		SetupMarkers ();

		playerBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator> ();

		SetupAfterSpawn ();
	}

	void SetupInstances (){
		if (tag.Equals (Constants.PLAYER_NAME)) {
			PlayerInstance = this;

		} else {
			EnemyInstance = this;
		}
	}


	void SetupMarkers (){
		if ( (isServer && tag.Equals (Constants.PLAYER_NAME)) ||
			 (!isServer && tag.Equals (Constants.ENEMY_NAME)) ) {
			PlaceMarker ();
		}
	}

	void Update() {
		if (tag.Equals (Constants.PLAYER_NAME)) {
			animator.SetFloat (PLAYER_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		
		} else {
			animator.SetFloat (ENEMY_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		}
	}

	void FixedUpdate() {
		FaceFrontOnMove ();
	}


	public void SetupAfterSpawn (){
		currentXPosition = transform.position.x; 
		FaceFrontOnSpawn ();
	}


	public void Move (Vector2 launchDir){
		if (!GameState.gameEnded && !isSwimming) {
			Vector2 impulse = CalculateImpulse (launchDir , WineBuzzLevel.PlayerBuzz);
			playerBody.AddForce (impulse, ForceMode2D.Impulse);

			//SoundManager.instance.PlaySingle (actorRunningSound, 1.5f);
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

	public void PlaceMarker(){
		Vector3 markerPosition = transform.position + new Vector3 (0,1,0);
		GameObject _marker = Instantiate (marker, markerPosition, Quaternion.identity) as GameObject;
		_marker.transform.parent = transform;
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

	//-------------------------------------------
	// utilities
	//-------------------------------------------

	Vector2 CalculateImpulse (Vector2 launchDir, float wineLevel){
		return launchDir * MOVT_SPEED * wineLevel; 
	}

}
