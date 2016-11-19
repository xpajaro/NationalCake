using UnityEngine;
using GooglePlayGames;
using System.Collections;

public class StateUpdates : MonoBehaviour {

	public GameObject player, enemy, cake;
	Rigidbody2D playerBody, cakeBody, enemyBody;
	SpriteRenderer playerRenderer, enemyRenderer, cakeRenderer;
	Animator pAnimator, eAnimator;

	string PLAYER_MOVT_PARAMETER = "playerDistanceToMove";
	string ENEMY_MOVT_PARAMETER = "enemyDistanceToMove";

	float outOfScreen = -10f;

	//for moving the actors (cake, player, enemy) w interpolation
	Vector2 pCurrPos, pNextPos, eCurrPos, eNextPos, cCurrPos, cNextPos;

	//for keeping falling status
	bool pFalling, eFalling, cFalling; //receiving networked game updates

	float TIME_GAP = .16f;
	float nextBroadcastTime = 0;
	float lastUpdateTime;

	Moving moveEnemy;


	void Start (){
		Communicator.Instance.stateUpdates = this;

		moveEnemy = new Moving (enemy);

		if (!GameSetup.isHost) {
			LoadRenderers ();
			LoadAnimators ();
		} else {
			LoadRigidBodies ();
		}
	}

	void LoadRigidBodies (){
		playerBody = player.GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();
	}

	void LoadRenderers (){
		playerRenderer = player.GetComponent<SpriteRenderer> ();
		enemyRenderer = enemy.GetComponent<SpriteRenderer> ();
		cakeRenderer = cake.GetComponent<SpriteRenderer> ();
	}

	void LoadAnimators (){
		pAnimator = player.GetComponent<Animator> ();
		eAnimator = enemy.GetComponent<Animator> ();
	}

	void FixedUpdate () {
		if (GameSetup.isHost) {
			if (Time.time > nextBroadcastTime) {
				Communicator.Instance.ShareActorState (playerBody, Falling.pFalling, 
					enemyBody, Falling.eFalling,
					cakeBody, Falling.cFalling); 


				nextBroadcastTime = Time.time + TIME_GAP;
			}
		} else {
			InterpolateAllMovement ();
		}
	}	


	//-------------------------------------------
	// Handle opponent input (host only)
	//-------------------------------------------
	public void MoveEnemy (Vector2 impulse){
		if (GameState.gameEnded) {
			return;
		}

		moveEnemy.NetworkImpulseReceived (impulse, WineBuzzLevel.EnemyBuzz);
	}


	//-------------------------------------------
	// End game
	//-------------------------------------------

	public void EndGame(){
		Invoke ("LeaveScene", 5f);
	}


	void LeaveScene(){
		GameSetup.LeaveGame ();
	}

	//-------------------------------------------
	// Handle new network game state (client only)
	// actors are player, enemy and cake (movable game elements with physics)
	//-------------------------------------------

	int lastStateNumber = -1;
	public void UpdateActors (ActorState state){

		if (!GameSetup.isHost) {

			if (lastStateNumber < state.stateNumber) {
				state = SwitchPlayers (state); 

				UpdatePositions (state);
				FaceCorrectDirection ();

				lastUpdateTime = Time.time;
				lastStateNumber = state.stateNumber;
			}

			HandleAllFalling (state.enemyFalling, state.playerFalling, state.cakeFalling);

		}

	}



	void UpdatePositions (ActorState state){
		pCurrPos = player.transform.position;
		eCurrPos = enemy.transform.position;
		cCurrPos = cake.transform.position;

		pNextPos = state.playerPosition ;
		eNextPos = state.enemyPosition ;
		cNextPos = state.cakePosition ;
	}


	bool enemyFacingHomeBase_Client = false, playerFacingHomeBase_Client = false;
	void FaceCorrectDirection (){
		
		if (pCurrPos.x > pNextPos.x && playerFacingHomeBase_Client) { //going left
			Utilities.TurnAround (player);
			playerFacingHomeBase_Client = false;

		} else if (pCurrPos.x < pNextPos.x && !playerFacingHomeBase_Client) {
			Utilities.TurnAround (player);
			playerFacingHomeBase_Client = true;
		}


		if (eCurrPos.x > eNextPos.x && !enemyFacingHomeBase_Client) { //going left
			Utilities.TurnAround (enemy);
			enemyFacingHomeBase_Client = true;

		} else if (eCurrPos.x < eNextPos.x && enemyFacingHomeBase_Client) {
			Utilities.TurnAround (enemy);
			enemyFacingHomeBase_Client = false;
		}

	}


	void HandleAllFalling (bool _pFalling, bool _eFalling, bool _cFalling){
		HandleActorFalling (_pFalling, ref this.pFalling, player, playerRenderer);
		HandleActorFalling (_eFalling, ref this.eFalling, enemy, enemyRenderer);
		HandleActorFalling (_cFalling, ref this.cFalling, cake, cakeRenderer);
	}

	void HandleActorFalling (bool isFalling, ref bool localFallingRef, GameObject actor, SpriteRenderer renderer){
		if (isFalling && !localFallingRef) {
			localFallingRef = true;
			Presenter.Detach (actor, renderer);
		} else if (!isFalling && localFallingRef){
			localFallingRef = false;
			Presenter.Attach (actor, renderer);
		}
	}

	//-------------------------------------------
	// implement server positions (client only)
	//-------------------------------------------

	void InterpolateAllMovement (){ 
		InterpolateUnlessReviving (player, pCurrPos, pNextPos);
		InterpolateUnlessReviving (enemy, eCurrPos, eNextPos);
		InterpolateUnlessReviving (cake, cCurrPos, cNextPos);
	}

	void InterpolateUnlessReviving (GameObject actor, Vector2 currPos, Vector2 nextPos ){
		if (currPos.x > outOfScreen ) {

			if (!currPos.Equals (nextPos)) {
				float movementProgress = GetMovementProgress ();
				Utilities.Interpolate (actor, currPos, nextPos, movementProgress);
			}

			AnimateActor (actor, Vector2.Distance (currPos, nextPos));

		} else {
			actor.transform.position = nextPos;
		}
	}

	void AnimateActor (GameObject actor, float movtDistance){
		if (actor.name.Equals (Constants.PLAYER_NAME)) {
			pAnimator.SetFloat (PLAYER_MOVT_PARAMETER, movtDistance);

		} else if (actor.name.Equals (Constants.ENEMY_NAME)) {
			eAnimator.SetFloat (ENEMY_MOVT_PARAMETER, movtDistance);
		}
	}

	float GetMovementProgress (){
		return (Time.time - lastUpdateTime) / TIME_GAP;
	}

	ActorState SwitchPlayers (ActorState oldState){
		Vector2 positionHolder = oldState.playerPosition;
		oldState.playerPosition = oldState.enemyPosition ;
		oldState.enemyPosition = positionHolder ;

		return oldState;
	}

}
