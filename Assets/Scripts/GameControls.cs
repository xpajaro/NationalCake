using UnityEngine;
using System.Collections;
using System.Text;

public class GameControls : MonoBehaviour {

	public float MOVT_SPEED = 5;
	public float MOVT_CAP = 150;
	public float MOVT_CAP_EFFECTIVE_RATIO = 50;

	public GameObject stage;
	public GameObject enemy;

	bool touchStarted = false;
	Vector3 movtStartPosition;

	Rigidbody2D playerBody;

	//static GameObject enemy;
	static Rigidbody2D enemyBody;


	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {

		if (MovementHandler.isOnStage (transform.position, stage)) {
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
		ShareMovement (impulse);
		playerBody.AddForce (impulse, ForceMode2D.Impulse);
	}

	void ShareMovement (Vector3 impulse){
		string msg = NetworkManager.MessageTypes.mvmt.ToString () +
		             NetworkManager.MESSAGE_DIVIDER +
		             Utilities.SerializeVector3 (impulse);

		NetworkManager.SendMessage (msg);
	}


	//-------------------------------------------
	// Handle opponent input
	//-------------------------------------------

	public static void MoveEnemy (string rawImpulse){
		Vector3 impulse = Utilities.DeserializeVector3(rawImpulse);
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
