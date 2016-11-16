using UnityEngine;
using System.Collections;

public class StageManager : MonoBehaviour 
{
	public static Vector2 PLAYER_START_POSITION;
	public static Vector2 ENEMY_START_POSITION;
	public static Vector2 CAKE_START_POSITION;

	public float MOVT_DAMPING = 0.5f;
	public float VELOCITY_CAP = 10.0f;

	float LEG_HEIGHT = 0.36f;

	//for elements to do friction on
	public GameObject cake, player, enemy;
	Rigidbody2D playerBody, enemyBody, cakeBody;
	SpriteRenderer pRenderer, eRenderer;

	static Texture2D stageTexture;
	static WorldConverter converter;

	public static bool playerOnStage, enemyOnStage, cakeOnStage;


	void Start(){
		LoadRenderers ();
		StoreEveryStartPosition ();
		LoadRigidBodies ();
		converter = new WorldConverter (this.gameObject);

		stageTexture = GetComponent<SpriteRenderer> ().sprite.texture;

		if (!GameSetup.isHost) {
			SwitchSides ();
			RemoveClientPhysics ();
		}

	}

	void LoadRenderers (){
		pRenderer = player.GetComponent<SpriteRenderer> ();
		eRenderer = enemy.GetComponent<SpriteRenderer> ();
	}

	void LoadRigidBodies (){
		playerBody = player.GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		cakeBody = cake.GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate () {
		SortAppearances ();

		if (GameSetup.isHost) {
			CheckIfActorsOnStage ();
			StartAllFriction ();
		}
	}		

	void SortAppearances (){
		SortCakeAppearance (player, pRenderer);
		SortCakeAppearance (enemy, eRenderer);

		SortEnemyAppearance ();
	}

	void SortCakeAppearance (GameObject actor, SpriteRenderer _renderer){
		if (actor.transform.position.y > cake.transform.position.y) {
			_renderer.sortingOrder = Constants.SORTING_ORDER_BACK;
		} else {
			_renderer.sortingOrder = Constants.SORTING_ORDER_FRONT;
		}
	}

	void SortEnemyAppearance (){
		//if characters are both in front or back of cake, sort appearance
		if (eRenderer.sortingOrder == pRenderer.sortingOrder){
			if (enemy.transform.position.y > player.transform.position.y) {
				eRenderer.sortingOrder = pRenderer.sortingOrder - 1;
			} else {
				eRenderer.sortingOrder = pRenderer.sortingOrder + 1;
			}
		}
	}


	void StoreEveryStartPosition (){
		PLAYER_START_POSITION = player.transform.position;
		ENEMY_START_POSITION = enemy.transform.position;
		CAKE_START_POSITION = cake.transform.position;
	}

	void SwitchSides (){
		player.transform.position = ENEMY_START_POSITION;
		enemy.transform.position = PLAYER_START_POSITION;

		Utilities.TurnAround (player);
		Utilities.TurnAround (enemy);
	}


	void RemoveClientPhysics(){
		//Destroy (playerBody);
		//Destroy (enemyBody);
		Destroy (cakeBody);
		DestroyColliders (cake);
	}

	void DestroyColliders (GameObject actor){
		Collider2D[] colliders = actor.GetComponents<Collider2D> ();
		foreach (Collider2D c in colliders){
			Destroy (c);
		}
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
			rb.velocity = Vector2.zero;
		}
	}


	//-------------------------------------------
	// actor on stage checker
	//-------------------------------------------

	void CheckIfActorsOnStage (){
		playerOnStage = isOnStage ( GetFeetPosition(player) );
		enemyOnStage = isOnStage ( GetFeetPosition(enemy) );
		cakeOnStage = isOnStage ( GetFeetPosition(cake) );
	}

	Vector2 GetFeetPosition (GameObject actor){
		Vector2 feetPosition = new Vector2 ();
		feetPosition.x = actor.transform.position.x;
		feetPosition.y = actor.transform.position.y; //- LEG_HEIGHT;

		return feetPosition;
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

