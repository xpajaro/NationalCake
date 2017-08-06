using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class GameController : NetworkBehaviour {


	void MovementStarted (Vector2 pos){
		isMoving = true;
		movtStartPosition = pos;
	}

	void MovementEnded (Vector2 pos){
		if (isMoving) {
			isMoving = false;

			Vector2 launchDir = CalculateLaunchDirection (pos);
			launchDir = launchDir / MOVT_CAP_EFFECTIVE_RATIO;

			CmdMove (launchDir, isServer);

		}
	}

	[Command]
	void CmdMove (Vector2 launchDir, bool movePlayer){

		if (movePlayer) {
			PlayerController.PlayerInstance.Move (launchDir);

		} else {
			PlayerController.EnemyInstance.Move (launchDir);
		}

	}

	void MovementCanceled (){
		isMoving = false;
	}

	//-------------------------------------------
	// utilities
	//-------------------------------------------


	PlayerController GetPlayerControllerInstance (){
		PlayerController instance;

		if (isServer) {
			instance = PlayerController.PlayerInstance;
		} else {
			instance = PlayerController.EnemyInstance; 
		}

		return instance;
	}

	Vector2 CalculateLaunchDirection (Vector2 movtEndPoint){
		Vector2 launchDir =  movtStartPosition - movtEndPoint ;

		if (launchDir.magnitude > MOVT_CAP) {
			launchDir = launchDir/ launchDir.magnitude * MOVT_CAP;
		}

		return launchDir;
	}

}
