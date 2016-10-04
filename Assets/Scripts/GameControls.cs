using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public float MOVT_SPEED = 5;
	public float MOVT_CAP = 150;
	public float MOVT_CAP_EFFECTIVE_RATIO = 50;

	public GameObject stage;
	public GameObject enemy;
	public GameObject princess;

	bool touchStarted = false;
	Vector3 movtStartPosition;

	Rigidbody2D playerBody;
	Rigidbody2D princessBody;

	//static GameObject enemy;
	static Rigidbody2D enemyBody;


	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();

		MovementHandler.LoadStage (stage);
	}

	void FixedUpdate () {
		if (GameManager.isHost) {
			MovementHandler.doFriction (playerBody);
			MovementHandler.doFriction (enemyBody);
			MovementHandler.doFriction (princessBody);

			Communicator.ShareState (playerBody, enemyBody, princessBody ); 
		}

		if (MovementHandler.isOnStage (transform.position)) {
			//items and stuff
		} 
	}
		


	//-------------------------------------------
	// Handle player input
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
			Communicator.ShareMovement (impulse);
		}
	}


	//-------------------------------------------
	// Handle opponent input
	//-------------------------------------------

	public static void MoveEnemy (Dictionary<string, object> networkData){
		Vector3 impulse = (Vector3) networkData [Communicator.IMPULSE];
		impulse.x  = impulse.x * -1; //enemy is placed in opp side of screen;
		enemyBody.AddForce (impulse, ForceMode2D.Impulse);
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



}
