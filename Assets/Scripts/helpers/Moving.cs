using System;
using UnityEngine;

public class Moving {

	//for player
	float MOVT_SPEED = 4; //liked 5
	float MOVT_CAP = 200;
	float MOVT_CAP_EFFECTIVE_RATIO = 50;

	GameObject actor;
	Rigidbody2D actorBody;

	Vector3 movtStartPosition;
	bool facingHomeBase = false; // default position

	public Moving (GameObject _actor){
		actor = _actor;
		actorBody = actor.GetComponent<Rigidbody2D> ();
	}


	public void MovementInputStarted (Vector3 startPos){
		movtStartPosition = startPos;
	}

	public void NetworkImpulseReceived (Vector3 impulse, float buzz){
		Vector3 drunkImpulse =  CalculateWineImpulse (impulse, buzz) ;

		Utilities.FaceCorrectDirection (actor, drunkImpulse, ref facingHomeBase, false);

		actorBody.AddForce (drunkImpulse, ForceMode2D.Impulse);
	}

	public void MovemenInputEnded(Vector3 endPos){
		Vector3 launchDir = CalculateLaunchDirection (endPos);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;
		MoveActor (launchDir);
	}

	void MoveActor (Vector3 launchDir){
		if (GameState.gameEnded) {
			return;
		}

		Vector3 impulse = CalculateImpulse (launchDir);

		if (GameSetup.isHost) {
			Vector3 drunkImpulse = CalculateWineImpulse (impulse, WineBuzzLevel.PlayerBuzz) ;

			Utilities.FaceCorrectDirection (actor, drunkImpulse, ref facingHomeBase, true);

			actorBody.AddForce (drunkImpulse, ForceMode2D.Impulse);

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

	Vector3 CalculateWineImpulse (Vector3 impulse, float wineLevel){
		return impulse * wineLevel;
	}
}


