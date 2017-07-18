using System;
using System.Collections;
using UnityEngine;

public class Moving {

	//for player
	float MOVT_SPEED = 3; //liked 5
	float MOVT_CAP = 200;
	float MOVT_CAP_EFFECTIVE_RATIO = 50;

	GameObject actor;
	Rigidbody2D actorBody;

	Vector2 movtStartPosition;
	bool facingHomeBase = false; // default position

	AudioClip actorRunningSound;

	public Moving (GameObject _actor, AudioClip _actorRunningSound){
		actor = _actor;
		actorBody = actor.GetComponent<Rigidbody2D> ();
		actorRunningSound = _actorRunningSound;
	}


	public void MovementInputStarted (Vector2 startPos){
		movtStartPosition = startPos;
	}

	public void NetworkImpulseReceived (Vector2 impulse, float buzz){
		SoundManager.instance.PlaySingle (actorRunningSound, 1f);

		if (GameSetup.isHost) {
			Vector2 drunkImpulse = CalculateWineImpulse (impulse, buzz);

			Utilities.FaceCorrectDirection (actor, drunkImpulse, ref facingHomeBase, false);

			actorBody.AddForce (drunkImpulse, ForceMode2D.Impulse);
		}
	}

	public void MovementInputEnded(Vector2 endPos){
		Vector2 launchDir = CalculateLaunchDirection (endPos);
		launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;
		MoveActor (launchDir);
	}

	void MoveActor (Vector2 launchDir){
		if (!GameState.gameEnded) {
			Vector2 impulse = CalculateImpulse (launchDir);

			if (GameSetup.isHost) {
				Vector2 drunkImpulse = CalculateWineImpulse (impulse, WineBuzzLevel.PlayerBuzz);

				Utilities.FaceCorrectDirection (actor, drunkImpulse, ref facingHomeBase, true);

				actorBody.AddForce (drunkImpulse, ForceMode2D.Impulse);
			} 

			SoundManager.instance.PlaySingle (actorRunningSound, 1f);
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


