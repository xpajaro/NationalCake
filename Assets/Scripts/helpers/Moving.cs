using System;
using UnityEngine;

public class Moving {

	//for player
	float MOVT_SPEED = 4; //liked 5
	float MOVT_CAP = 200;
	float MOVT_CAP_EFFECTIVE_RATIO = 50;

	GameObject actor;
	Rigidbody2D actorBody;

	Vector2 movtStartPosition;
	bool facingHomeBase = false; // default position

	public Moving (GameObject _actor){
		actor = _actor;
		actorBody = actor.GetComponent<Rigidbody2D> ();
	}


	public void MovementInputStarted (Vector2 startPos){
		movtStartPosition = startPos;
	}

	public void NetworkImpulseReceived (Vector2 impulse, float buzz){
		Vector2 drunkImpulse =  CalculateWineImpulse (impulse, buzz) ;

		Utilities.FaceCorrectDirection (actor, drunkImpulse, ref facingHomeBase, false);

		actorBody.AddForce (drunkImpulse, ForceMode2D.Impulse);
	}

	public void MovementInputEnded(Vector2 endPos){
		Vector2 launchDir = CalculateLaunchDirection (endPos);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;
		MoveActor (launchDir);
	}

	void MoveActor (Vector2 launchDir){
		if (GameState.gameEnded) {
			return;
		}

		Vector2 impulse = CalculateImpulse (launchDir);

		if (GameSetup.isHost) {
			Vector2 drunkImpulse = CalculateWineImpulse (impulse, WineBuzzLevel.PlayerBuzz) ;

			Utilities.FaceCorrectDirection (actor, drunkImpulse, ref facingHomeBase, true);

			actorBody.AddForce (drunkImpulse, ForceMode2D.Impulse);

		} else {
			Communicator.Instance.ShareMovement (impulse);
		}
	}

	//-------------------------------------------
	// movt calculations
	//-------------------------------------------

	Vector2 CalculateLaunchDirection (Vector2 movtEndPoint){
		Vector2 launchDir =  movtStartPosition - movtEndPoint ;

		if (launchDir.magnitude > MOVT_CAP) {
			launchDir = launchDir/ launchDir.magnitude * MOVT_CAP;
		}

		return launchDir;
	}

	Vector2 CalculateImpulse (Vector2 launchDir){
		return launchDir * MOVT_SPEED; 
	}

	Vector2 CalculateWineImpulse (Vector2 impulse, float wineLevel){
		return impulse * wineLevel;
	}
}


