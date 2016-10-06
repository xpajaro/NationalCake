using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public float MOVT_SPEED = 5;
	public float MOVT_CAP = 150;
	public float MOVT_CAP_EFFECTIVE_RATIO = 50;

	public GameObject stage, enemy, cake;

	bool touchStarted = false;
	Vector3 movtStartPosition;

	Rigidbody2D playerBody, cakeBody, enemyBody;

	Communicator communicator;

	float _nextBroadcastTime = 0;


	void Start () {

		//store all important refs to game elements first
		GameElements.Player = this.gameObject;
		GameElements.Enemy = enemy;
		GameElements.Cake = cake;
		GameElements.Stage = stage;

		playerBody = GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();

		MovementHandler.LoadStage (stage);

		communicator = new Communicator ();
	}

	void Update () {
		if (Time.time > _nextBroadcastTime) {
			communicator.ShareState (playerBody, enemyBody, cakeBody); 
			_nextBroadcastTime = Time.time + .30f;
		}
	}

	void FixedUpdate () {
		if (GameManager.isHost) {
			implementFriction ();
		}

		//remember items and stuff
	}
		


	//-------------------------------------------
	// Handle player input -- consider making this screen touch not player touch
	//-------------------------------------------
	// host player decided how movement happens
	// client player sends impulse to host and host returns game state
	// game state contains all positions & velocities
	//-------------------------------------------

	void OnMouseDown () {
		if (!touchStarted) {
			movtStartPosition = Input.mousePosition;
			touchStarted = true;
		}
	}

	void OnMouseUp () {
		touchStarted = false;
		Vector3 launchDir = calculateLaunchDirection (Input.mousePosition);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;
		MovePlayer (launchDir);
	}

	void MovePlayer (Vector3 launchDir){
		Vector3 impulse = calculateImpulse (launchDir);

		if (GameManager.isHost) {
			playerBody.AddForce (impulse, ForceMode2D.Impulse);
		} else {
			communicator.ShareMovement (impulse);
		}
	}



	//-------------------------------------------
	// movt calculations
	//-------------------------------------------

	Vector3 calculateLaunchDirection (Vector3 movtEndPoint){
		Vector3 launchDir =  movtStartPosition - Input.mousePosition ;

		if (launchDir.magnitude > MOVT_CAP) {
			launchDir = launchDir/ launchDir.magnitude * MOVT_CAP;
		}

		return launchDir;
	}
 
	Vector3 calculateImpulse (Vector3 launchDir){
		return launchDir * MOVT_SPEED; 
	}

	void implementFriction(){
		MovementHandler.doFriction (playerBody);
		MovementHandler.doFriction (enemyBody);
		MovementHandler.doFriction (cakeBody);
	}


}
