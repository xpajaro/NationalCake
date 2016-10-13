using UnityEngine;
using System.Collections;

public class GameUpdates : MonoBehaviour {

	public GameObject player, enemy, cake;
	Rigidbody2D playerBody, cakeBody, enemyBody;

	Vector3 pCurrPos, pNextPos, eCurrPos, eNextPos, cCurrPos, cNextPos;

	float nextBroadcastTime = 0;
	float timeGap = .6f;
	float lastUpdateTime;

	void Start (){
		playerBody = player.GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();

		Communicator.Instance.gameUpdates = this;
	}



	void FixedUpdate () {
		if (GameSetup.isHost) {
			if (Time.time > nextBroadcastTime) {
				Communicator.Instance.ShareState (playerBody, enemyBody, cakeBody); 
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
		impulse = Utilities.FlipX (impulse); //enemy is placed in opp side of screen;
		enemyBody.AddForce (impulse, ForceMode2D.Impulse);
	}


	//-------------------------------------------
	// Handle new network game state (client only)
	// actors are player, enemy and cake (movable game elements with physics)
	//-------------------------------------------

	public void UpdateActors (ActorState state){
		
		if (!GameSetup.isHost) {
			state = SwitchPlayers (state);

			pCurrPos = playerBody.position;
			eCurrPos = enemyBody.position;
			cCurrPos = cakeBody.position;

			pNextPos = state.PlayerPosition ;
			eNextPos = state.EnemyPosition ;
			cNextPos = state.CakePosition ;

			lastUpdateTime = Time.time;
		}

	}



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
		oldState.PlayerPosition = Utilities.FlipX (oldState.EnemyPosition);
		oldState.EnemyPosition = Utilities.FlipX (positionHolder);

		return oldState;
	}

}
