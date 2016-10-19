using UnityEngine;
using System.Collections;

public class StateUpdates : MonoBehaviour {

	public GameObject player, enemy, cake;
	Rigidbody2D playerBody, cakeBody, enemyBody;
	SpriteRenderer playerRenderer, enemyRenderer, cakeRenderer;

	//for moving the actors (cake, player, enemy) w interpolation
	Vector3 pCurrPos, pNextPos, eCurrPos, eNextPos, cCurrPos, cNextPos;

	//for keeping falling status
	bool pFalling, eFalling, cFalling; //receiving networked game updates

	float TIME_GAP = .16f;
	float nextBroadcastTime = 0;
	float lastUpdateTime;



	void Start (){
		Communicator.Instance.stateUpdates = this;

		if (!GameSetup.isHost) {
			LoadRenderers ();
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

	public void MoveEnemy (Vector3 impulse){
		Vector3 drunkImpulse =  PlayerControl.CalculateWineImpulse (impulse, WineBuzzLevel.EnemyBuzz) ;
		enemyBody.AddForce (drunkImpulse, ForceMode2D.Impulse);
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
				UpdatePositions (state);

				lastUpdateTime = Time.time;

				lastStateNumber = state.StateNumber;
			}

			HandleAllFalling (state.EnemyFalling, state.PlayerFalling, state.CakeFalling);

		}

	}


	void UpdatePositions (ActorState state){
		pCurrPos = player.transform.position;
		eCurrPos = enemy.transform.position;
		cCurrPos = cake.transform.position;

		pNextPos = state.PlayerPosition ;
		eNextPos = state.EnemyPosition ;
		cNextPos = state.CakePosition ;
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
		Utilities.Interpolate (player, pCurrPos, pNextPos, GetMovementProgresss () );
		Utilities.Interpolate (enemy, eCurrPos, eNextPos, GetMovementProgresss () );
		Utilities.Interpolate (cake, cCurrPos, cNextPos, GetMovementProgresss () );
	}

	float GetMovementProgresss (){
		return (Time.time - lastUpdateTime) / TIME_GAP;
	}

	Vector3 positionHolder;
	ActorState SwitchPlayers (ActorState oldState){
		positionHolder = oldState.PlayerPosition;
		oldState.PlayerPosition = oldState.EnemyPosition ;
		oldState.EnemyPosition = positionHolder ;

		return oldState;
	}

}
