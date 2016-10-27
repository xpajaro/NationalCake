using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class PlayerControl : MonoBehaviour {

	public float MOVT_SPEED = 8;
	public float MOVT_CAP = 150;
	public float MOVT_CAP_EFFECTIVE_RATIO = 50;

	Vector3 movtStartPosition;

	Rigidbody2D playerBody;

	bool facingHomeBase = false; // default position

	void Start () {
		playerBody = GetComponent<Rigidbody2D> ();
	}

	void Update (){
		HandleTouch ();
	}

	//-------------------------------------------
	// Handle player input -- consider making this screen touch not player touch
	//-------------------------------------------
	// host player decided how movement happens
	// client player sends impulse to host and host returns game state
	// game state contains all positions & velocities
	//-------------------------------------------

	void HandleTouch(){
		foreach ( Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				TouchStarted (touch);
			} else if (touch.phase == TouchPhase.Ended) {
				TouchEnded (touch);
			} else if (touch.phase == TouchPhase.Canceled) {
				TouchCanceled ();
			}
		}
	}

	void TouchStarted (Touch touch) {
			movtStartPosition = touch.position;
	}

	void TouchEnded (Touch touch) {
		Vector3 launchDir = CalculateLaunchDirection (touch.position);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;
		MovePlayer (launchDir);
	}

	void TouchCanceled () {
		
	}

	void MovePlayer (Vector3 launchDir){
		if (GameState.GameEnded) {
			return;
		}
		
		Vector3 impulse = CalculateImpulse (launchDir);

		if (GameSetup.isHost) {
			Vector3 drunkImpulse = CalculateWineImpulse (impulse, WineBuzzLevel.PlayerBuzz) ;

			Utilities.FaceCorrectDirection (this.gameObject, drunkImpulse, ref facingHomeBase, true);

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
