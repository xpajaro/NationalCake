using System;
using UnityEngine;

public class ActorState
{

	public Vector2 PlayerPosition { get; set; }
	public Vector2 PlayerVelocity { get; set; }
	public Vector2 EnemyPosition { get; set; }
	public Vector2 EnemyVelocity { get; set; }
	public Vector2 CakePosition { get; set; }

	public bool PlayerFalling { get; set; }
	public bool EnemyFalling { get; set; }
	public bool CakeFalling { get; set; }


	public ActorState (){
	
	}

	public void preparePayload (Rigidbody2D playerBody, Rigidbody2D enemyBody, Rigidbody2D cakeBody){
		PlayerPosition = playerBody.position;
		PlayerVelocity = playerBody.velocity;

		EnemyPosition = enemyBody.position;
		EnemyVelocity = enemyBody.velocity;

		CakePosition = cakeBody.position;
	}
 
}


