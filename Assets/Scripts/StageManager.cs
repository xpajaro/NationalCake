using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour 
{
	public static Vector3 PLAYER_START_POSITION;
	public static Vector3 ENEMY_START_POSITION;
	public static Vector3 CAKE_START_POSITION;

	public float MOVT_DAMPING = 0.5f;
	public float VELOCITY_CAP = 10.0f;

	//for elements to do friction on
	public GameObject cake, player, enemy;
	Rigidbody2D playerBody, enemyBody, cakeBody;

	static Texture2D stageTexture;
	static WorldConverter converter;

	public static bool playerOnStage, enemyOnStage, cakeOnStage;


	void Start(){

		if (GameSetup.isHost) {
			StoreEveryStartPosition ();

			playerBody = player.GetComponent<Rigidbody2D> ();
			enemyBody = enemy.GetComponent<Rigidbody2D> ();
			cakeBody = cake.GetComponent<Rigidbody2D> ();

			converter = new WorldConverter (this.gameObject);

			stageTexture = GetComponent<SpriteRenderer> ().sprite.texture;
		}

	}


	void FixedUpdate () {
		if (GameSetup.isHost) {
			checkIfActorsOnStage ();
			StartAllFriction ();
		}
	}		


	void StoreEveryStartPosition (){
		PLAYER_START_POSITION = player.transform.position;
		ENEMY_START_POSITION = enemy.transform.position;
		CAKE_START_POSITION = cake.transform.position;
	}

	//-------------------------------------------
	// Friction
	//-------------------------------------------

	void StartAllFriction () {
		StartActorFriction (playerOnStage, playerBody);
		StartActorFriction (enemyOnStage, enemyBody);
		StartActorFriction (cakeOnStage, cakeBody);
	}

	void StartActorFriction (bool onStage, Rigidbody2D rb){
		if (onStage) {
			dampMovement (rb, MOVT_DAMPING);
		} else {
			rb.velocity = Vector3.zero;
		}
	}

	void checkIfActorsOnStage (){
		playerOnStage = isOnStage (playerBody.position);
		enemyOnStage = isOnStage (enemyBody.position);
		cakeOnStage = isOnStage (cakeBody.position);
	}

	public static bool isOnStage (Vector2 actorPosition){
		bool onStage = true;
		actorPosition = converter.getPositionInWorld (actorPosition);
		//Debug.Log (actorPosition);

		if (Utilities.GetAlphaAtPosition (actorPosition, stageTexture) == 0){
			onStage = false;
			//Debug.Log (onStage.ToString());
		} 

		return onStage;
	}
		

	void dampMovement (Rigidbody2D rb, float damping){
		if (rb.velocity.x != 0 || rb.velocity.y != 0) {
			rb.velocity = rb.velocity * damping;
		} else if (rb.velocity.magnitude < 0.0001) {
			rb.velocity = new Vector2 (0, 0);
		}
	}


}

