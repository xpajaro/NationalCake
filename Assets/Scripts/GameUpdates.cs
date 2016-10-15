using UnityEngine;
using System.Collections;

public class GameUpdates : MonoBehaviour {

	public GameObject player, enemy, cake;
	Rigidbody2D playerBody, cakeBody, enemyBody;

	//for moving the actors (cake, player, enemy) w interpolation
	Vector3 pCurrPos, pNextPos, eCurrPos, eNextPos, cCurrPos, cNextPos;

	//for keeping falling status
	bool pFalling, eFalling, cFalling;

	float nextBroadcastTime = 0;
	float timeGap = .6f;
	float lastUpdateTime;

	FallingAnimator pAnimator, eAnimator, cAnimator ;



	void Start (){
		playerBody = player.GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();

		Communicator.Instance.gameUpdates = this;


		if (!GameSetup.isHost) {
			pAnimator = new FallingAnimator (player);
			eAnimator = new FallingAnimator (enemy);
			cAnimator = new FallingAnimator (cake);
		}
	}



	void FixedUpdate () {
		if (GameSetup.isHost) {
			if (Time.time > nextBroadcastTime) {
				Communicator.Instance.ShareState (playerBody, StageManager.playerOnStage, 
					enemyBody, StageManager.enemyOnStage,
					cakeBody, StageManager.cakeOnStage); 
				
				nextBroadcastTime = Time.time + timeGap;
			}
		} else {
			InterpolateAllMovement ();
		}
	}	


	//-------------------------------------------
	// Handle opponent input (host only)
	//-------------------------------------------

	public void MoveEnemy (Vector3 impulse){
		impulse = Utilities.FlipSide (impulse); //enemy is placed in opp side of screen;
		enemyBody.AddForce (impulse, ForceMode2D.Impulse);
	}


	//-------------------------------------------
	// Handle new network game state (client only)
	// actors are player, enemy and cake (movable game elements with physics)
	//-------------------------------------------

	int lastStateNumber = -1;

	public void UpdateActors (ActorState state){

		if (!GameSetup.isHost) {

			if (lastStateNumber < state.StateNumber) {
				state = SwitchPlayers (state);

				pCurrPos = playerBody.position;
				eCurrPos = enemyBody.position;
				cCurrPos = cakeBody.position;

				pNextPos = state.PlayerPosition ;
				eNextPos = state.EnemyPosition ;
				cNextPos = state.CakePosition ;

				lastUpdateTime = Time.time;

				lastStateNumber = state.StateNumber;
			}

			HandleAllFalling (state.PlayerFalling, state.EnemyFalling, state.CakeFalling);

		}

	}


	void HandleAllFalling (bool _pFalling, bool _cFalling, bool _eFalling){
		HandleActorFalling (_pFalling, ref this.pFalling, pAnimator);
		HandleActorFalling (_eFalling, ref this.eFalling, eAnimator);
		HandleActorFalling (_cFalling, ref this.cFalling, cAnimator);
	}

	void HandleActorFalling (bool isFalling, ref bool localFallingRef, FallingAnimator animator){
		if (isFalling && !localFallingRef) {
			localFallingRef = true;
			animator.Detach ();
		} else if (!isFalling && localFallingRef){
			localFallingRef = false;
			animator.Revive ();
		}
	}

	//-------------------------------------------
	// implement server positions (cllient only)
	//-------------------------------------------

	void InterpolateAllMovement (){ 
		InterpolateMovement (playerBody, pCurrPos, pNextPos);
		InterpolateMovement (enemyBody, eCurrPos, eNextPos);
		InterpolateMovement (cakeBody, cCurrPos, cNextPos);
	}

	//move smoothly between curr pos and target position
	void InterpolateMovement (Rigidbody2D rigidBody, Vector3 start, Vector3 destination){ 
		float pctDone = (Time.time - lastUpdateTime) / timeGap;

		if (pctDone <= 1.0) {
			rigidBody.position = Vector3.Lerp (start, destination, pctDone);
		}  
	}


	Vector3 positionHolder;
	ActorState SwitchPlayers (ActorState oldState){
		positionHolder = oldState.PlayerPosition;
		oldState.PlayerPosition = Utilities.FlipSide (oldState.EnemyPosition);
		oldState.EnemyPosition = Utilities.FlipSide (positionHolder);

		return oldState;
	}

}
