using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour {

	protected bool isMoving;

	const float MOVT_CAP = 200;
	const float MOVT_CAP_EFFECTIVE_RATIO = 50;


	Vector2 movtStartPosition;
	public static GameController LocalInstance;

	void Update (){
		//don't allow inputs from enemy character
		if (!isLocalPlayer || !GetPlayerControllerInstance()){
			return;
		}

		if (!LocalInstance) {
			LocalInstance = this;
		}

		HandleTouch ();
		HandleMouse ();
	}


	protected void HandleTouch(){
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			switch (touch.phase) {

			case TouchPhase.Began:
				MovementStarted (touch.position);
				break;

			case TouchPhase.Ended:
				MovementEnded (touch.position);
				break;

			case TouchPhase.Canceled:
				MovementCanceled ();
				break;

			}
		}
	} 

	protected void HandleMouse(){
		if (Input.GetMouseButtonDown(0)){
			if (!isMoving) {
				MovementStarted (Input.mousePosition);
			}
		} else if (!MouseInsideBounds()) {
			MovementCanceled ();

		} else if (Input.GetMouseButtonUp(0)){
			MovementEnded(Input.mousePosition);

		}
	} 

	bool MouseInsideBounds () {
		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
		return screenRect.Contains (Input.mousePosition);
	}

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
