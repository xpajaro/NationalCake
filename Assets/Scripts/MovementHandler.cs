using UnityEngine;
using System.Collections;

public class MovementHandler : MonoBehaviour 
{
	//TODO separate friction and onstage
	//move onstage to some sort of world to local utility
	public float MOVT_DAMPING = 0.5f;


	//for elements to do friction on
	public GameObject cake, player, enemy;
	Rigidbody2D playerBody, enemyBody, cakeBody;

	static Texture2D stageTexture;
	static WorldConverter converter;

	float _nextBroadcastTime = 0;



	//-------------------------------------------
	// Logic
	//-------------------------------------------


	void Start(){
		playerBody = player.GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();

		converter = new WorldConverter (this.gameObject);

		stageTexture = GetComponent<SpriteRenderer> ().sprite.texture;

		StartFriction ();
	}


	void Update () {
		if (Time.time > _nextBroadcastTime) {
			Communicator.Instance.ShareState (playerBody, enemyBody, cakeBody); 
			_nextBroadcastTime = Time.time + .30f;
		}
	}		


	void StartFriction () {
		StartFrictionOnElement (playerBody);
		StartFrictionOnElement (enemyBody);
		StartFrictionOnElement (cakeBody);
	}


	void StartFrictionOnElement (Rigidbody2D rb) {
		if (isOnStage (rb.position)) {
			dampMovement (rb, MOVT_DAMPING);
		} 
	}
		
	public static bool isOnStage (Vector2 playerPosition){
		bool onStage = true;
		playerPosition = converter.getPositionInWorld (playerPosition);
		if (Utilities.GetAlphaAtPosition (playerPosition, stageTexture) == 0){
			onStage = false;
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

