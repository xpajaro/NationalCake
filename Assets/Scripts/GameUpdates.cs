using UnityEngine;
using System.Collections;

public class GameUpdates {

	Rigidbody2D playerBody, cakeBody, enemyBody;

	public GameUpdates (){
		playerBody = GameElements.Player.GetComponent<Rigidbody2D> ();
		enemyBody = GameElements.Enemy.GetComponent<Rigidbody2D> ();
		cakeBody = GameElements.Cake.GetComponent<Rigidbody2D> ();
	}

	//-------------------------------------------
	// Handle opponent input (host only)
	//-------------------------------------------

	public void MoveEnemy (Vector3 impulse){
		impulse.x  = impulse.x * -1; //enemy is placed in opp side of screen;
		enemyBody.AddForce (impulse, ForceMode2D.Impulse);
	}


	//-------------------------------------------
	// Handle new network game state (client only)
	// actors are player, enemy and cake (movable game elements with physics)
	//-------------------------------------------

	public void UpdateActors (ActorState state){
		state = FlipPlayersOnClient (state);
		if (!GameSetup.isHost) {
			playerBody.MovePosition (state.PlayerPosition) ;
			enemyBody.MovePosition (state.EnemyPosition) ;
			cakeBody.MovePosition (state.CakePosition) ;
		}

	}


	ActorState FlipPlayersOnClient (ActorState oldState){

		ActorState state = new ActorState();

		Vector3 positionHolder = oldState.EnemyPosition;
		positionHolder.x *= -1;
		state.PlayerPosition = positionHolder;
		Debug.Log ("final position player " + state.PlayerPosition.ToString ("G4"));

		positionHolder = oldState.PlayerPosition;
		positionHolder.x *= -1;
		state.EnemyPosition = positionHolder;
		Debug.Log ("final position player " + state.EnemyPosition.ToString ("G4"));

		return state;
	}

}
