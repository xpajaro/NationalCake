using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PlayerControl : MonoBehaviour {

	public float MOVT_SPEED = 8;
	public float MOVT_CAP = 150;
	public float MOVT_CAP_EFFECTIVE_RATIO = 50;


	bool touchStarted = false;
	Vector3 movtStartPosition;

	Rigidbody2D playerBody;
	SpriteRenderer spriteRenderer;



	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> (); 
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
		Vector3 launchDir = CalculateLaunchDirection (Input.mousePosition);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;
		MovePlayer (launchDir);
	}

	void MovePlayer (Vector3 launchDir){
		Vector3 impulse = CalculateImpulse (launchDir);

		if (GameSetup.isHost) {
			Vector3 drunkImpulse = CalculateWineImpulse (impulse, WineBuzzLevel.PlayerBuzz) ;
			playerBody.AddForce (drunkImpulse, ForceMode2D.Impulse);
		} else {
			Communicator.Instance.ShareMovement (impulse);
		}
	}



	//-------------------------------------------
	// movt calculations
	//-------------------------------------------

	Vector3 CalculateLaunchDirection (Vector3 movtEndPoint){
		Vector3 launchDir =  movtStartPosition - Input.mousePosition ;

		if (launchDir.magnitude > MOVT_CAP) {
			launchDir = launchDir/ launchDir.magnitude * MOVT_CAP;
		}

		return launchDir;
	}

	Vector3 CalculateImpulse (Vector3 launchDir){
		return launchDir * MOVT_SPEED; 
	}

	public static Vector3 CalculateWineImpulse (Vector3 impulse, float wineLevel){
		return impulse * wineLevel;
	}


}
