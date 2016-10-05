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

	static Rigidbody2D playerBody, cakeBody, enemyBody;

	Communicator communicator;


	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();

		MovementHandler.LoadStage (stage);

		communicator = new Communicator ();
	}

	void FixedUpdate () {
		if (GameManager.isHost) {
			implementFriction ();
			communicator.ShareActorState (playerBody, enemyBody, cakeBody ); 
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
	// Handle opponent input (host only)
	//-------------------------------------------

	public static void MoveEnemy (Dictionary<string, object> networkData){
		Vector3 impulse = (Vector3) networkData [Communicator.IMPULSE];
		impulse.x  = impulse.x * -1; //enemy is placed in opp side of screen;
		enemyBody.AddForce (impulse, ForceMode2D.Impulse);
	}


	//-------------------------------------------
	// Handle new network game state (client only)
	// actors are player, enemy and cake (movable game elements with physics)
	//-------------------------------------------

	public static void UpdateActors (Dictionary<string, object> networkData){
		//player is host, //enemy is client
		if (!GameManager.isHost) {
			ActorState actorState = (ActorState) networkData [Communicator.ACTOR_STATE];

			playerBody.MovePosition (actorState.EnemyPosition) ;
			enemyBody.MovePosition (actorState.PlayerPosition) ;
			cakeBody.MovePosition (actorState.CakePosition) ;
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
