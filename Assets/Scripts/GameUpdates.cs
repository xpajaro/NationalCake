﻿using UnityEngine;
using System.Collections;

public class GameUpdates : MonoBehaviour {

	public GameObject player, enemy, cake;
	Rigidbody2D playerBody, cakeBody, enemyBody;

	float _nextBroadcastTime = 0;

	void Start (){
		playerBody = player.GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();

		Communicator.Instance.gameUpdates = this;
	}



	void Update () {
		if (GameSetup.isHost) {
			if (Time.time > _nextBroadcastTime) {
				Communicator.Instance.ShareState (playerBody, enemyBody, cakeBody); 
				_nextBroadcastTime = Time.time + .30f;
			}
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

			playerBody.MovePosition (state.PlayerPosition) ;
			enemyBody.MovePosition (state.EnemyPosition) ;
			cakeBody.MovePosition (state.CakePosition) ;
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
