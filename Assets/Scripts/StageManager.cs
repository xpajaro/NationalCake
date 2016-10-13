using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour 
{
	public float MOVT_DAMPING = 0.5f;

	//for elements to do friction on
	public GameObject cake, player, enemy;
	Rigidbody2D playerBody, enemyBody, cakeBody;

	static Texture2D stageTexture;
	static WorldConverter converter;

	public static bool playerOnStage, enemyOnStage, cakeOnStage;


	void Start(){
		if (GameSetup.isHost) {
			playerBody = player.GetComponent<Rigidbody2D> ();
			enemyBody = enemy.GetComponent<Rigidbody2D> ();
			cakeBody = cake.GetComponent<Rigidbody2D> ();

			converter = new WorldConverter (this.gameObject);

			stageTexture = GetComponent<SpriteRenderer> ().sprite.texture;
		}

	}


	void Update () {
		if (GameSetup.isHost) {
			checkIfActorsOnStage ();
			StartFriction ();
		}
	}		


	//-------------------------------------------
	// Friction
	//-------------------------------------------

	void StartFriction () {
		if (playerOnStage) dampMovement (playerBody, MOVT_DAMPING);
		if (enemyOnStage) dampMovement (enemyBody, MOVT_DAMPING);
		if (cakeOnStage) dampMovement (cakeBody, MOVT_DAMPING);
	}


	void checkIfActorsOnStage (){
		playerOnStage = isOnStage (playerBody.position);
		enemyOnStage = isOnStage (enemyBody.position);
		cakeOnStage = isOnStage (cakeBody.position);
	}

	public static bool isOnStage (Vector2 playerPosition){
		bool onStage = true;
		playerPosition = converter.getPositionInWorld (playerPosition);
		Debug.Log (playerPosition);

		if (Utilities.GetAlphaAtPosition (playerPosition, stageTexture) == 0){
			onStage = false;
			Debug.Log (onStage.ToString());
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

