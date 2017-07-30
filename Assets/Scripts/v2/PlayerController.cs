using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class PlayerController : NetworkBehaviour {

	//for player
	const float MOVT_SPEED = 3; //liked 5
	const float MOVT_CAP = 200;
	const float MOVT_CAP_EFFECTIVE_RATIO = 50;
	const string PLAYER_VELOCITY_PARAMETER = "playerVelocity";

	Vector2 movtStartPosition;
	Rigidbody2D playerBody;
	Animator animator;
	public AudioClip actorRunningSound;
	public GameObject marker;

	public bool isSwimming; //set in swimming class
	float currentXPosition;
	int currentDirection = 1; //1 is right, -1 is left


	public static PlayerController LocalInstance = null;    

	public override void OnStartLocalPlayer (){
		if (isLocalPlayer) {
			LocalInstance = this;
			PlaceMarker ();
		} 
	}

	void Start () {
		SetTags ();
		playerBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator> ();

		SetupAfterSpawn ();
	}

	void Update() {
		if (isLocalPlayer) {
			animator.SetFloat (PLAYER_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		}
	}

	void FixedUpdate() {
		FaceFrontOnMove ();
	}


	public void SetupAfterSpawn (){
		currentXPosition = transform.position.x; 
		FaceFrontOnSpawn ();
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
	// setup
	//-------------------------------------------

	void SetTags(){
		if (isLocalPlayer) {
			tag = Constants.PLAYER_NAME;
		} else {
			tag = Constants.ENEMY_NAME;
		}
	}

	public void FaceFrontOnSpawn(){
		if (transform.position.x > 0) { //player on right should face inwards
			int direction = -1;

			TurnPlayerAround (direction);
			currentDirection = direction;
		}
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
		Debug.Log (string.Format("TPA {0}", transform.localScale.x));
	}

	//-------------------------------------------
	// utilities
	//-------------------------------------------

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
