using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class GameController : NetworkBehaviour {

	protected bool isMoving;

	const float MOVT_CAP = 200;
	const float MOVT_CAP_EFFECTIVE_RATIO = 50;


	Vector2 movtStartPosition;
	public ActivateAttack selectedItemRef;

	public static GameController LocalInstance;


	void Start(){
		InitGameState ();
	}

	void InitGameState(){
		GameState.gameEnded = false;
		GameState.gameWon = false;

		PlayerData playerData = SessionManager.Instance.playerData;
		playerData.GameCount++;

		LocalStorage.Instance.Save (playerData);
	
	}


	void Update (){
		//don't allow inputs from enemy character
		if (!isLocalPlayer || !GetPlayerControllerInstance()){
			return;
		}

		if (LocalInstance == null) {
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
				InputBegan (touch.position);
				break;

			case TouchPhase.Ended:
				InputEnded (touch.position);
				break;

			case TouchPhase.Canceled:
				InputCanceled ();
				break;

			}
		}
	} 

	protected void HandleMouse(){
		if (Input.GetMouseButtonDown(0)){
			InputBegan (Input.mousePosition);

		} else if (!MouseInsideBounds()) {
			InputCanceled ();

		} else if (Input.GetMouseButtonUp(0)){
			InputEnded (Input.mousePosition);
		}
	} 

	bool MouseInsideBounds (){
		Rect screenRect = new Rect(0,0, Screen.width, Screen.height);
		return screenRect.Contains (Input.mousePosition);
	}



	//-------------------------------------------
	// Input handlers
	// ( movement in partial class )
	//-------------------------------------------

	void InputBegan (Vector2 position){
		if (selectedItemRef != null) {
			//check itemcontroller file, this is a partial class
			Vector2 positionInWorld = Camera.main.ScreenToWorldPoint (position);
			ActivateSelectedItem (positionInWorld);

		} else if (!isMoving) {
			MovementStarted (position);
		}
	}

	void InputEnded (Vector2 position){
		MovementEnded(position);
	}

	void InputCanceled (){
		MovementCanceled();
	}

}
