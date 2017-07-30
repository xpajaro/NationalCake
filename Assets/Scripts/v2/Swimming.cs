using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Swimming : NetworkBehaviour {

	const float DISTANCE_TO_DROP = -3.0f;
	const float DISTANCE_TO_SWIM = -20.0f;
	const float SPEED = 7f;

	bool playerFallTriggered;
	bool isReadyToDropAndSwim;
	bool isDropping;
	bool isSwimming;
	Vector3 placeToDropAt, placeToSwimTo;
	Vector3 startPosition;
	PlayerController playerController;

	SpriteRenderer spriteRenderer;
	Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		rigidBody = GetComponent<Rigidbody2D> ();

		startPosition = transform.position;
		playerController = GetComponent<PlayerController> ();
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		TriggerFall ();
		HandleFall ();
	}

	void TriggerFall(){
		if (!Stage.Instance.IsOnStage (transform.position) && 
			!playerFallTriggered) {
			playerFallTriggered = true;
			playerController.isSwimming = true;
		}
	}

	void HandleFall () {
		if (playerFallTriggered){
			
			if (!isReadyToDropAndSwim ) {
				CalculateFinalPositions ();
				Presenter.Detach (gameObject, spriteRenderer);

			} else if (isDropping) {
				DropToWater ();

			} else if (isSwimming) {
				SwimOff ();
			
			} else {
				ReturnToSpawnPosition ();

				if (name.StartsWith (Constants.PLAYER_NAME)){
					//winelevel reset
				}
			}

			//fall and drown animation all done
			//done here because client doesn't do the dropping and swimming
			if (transform.position.Equals(startPosition) &&
				isReadyToDropAndSwim){
				Presenter.Attach (gameObject, spriteRenderer);

				isReadyToDropAndSwim = false;
				playerFallTriggered = false;

				playerController.SetupAfterSpawn ();
				playerController.isSwimming = false;
			}
		}
	}

	void CalculateFinalPositions(){ 
		placeToDropAt = transform.position + new Vector3 (0, DISTANCE_TO_DROP);
		placeToSwimTo = placeToDropAt + new Vector3 (DISTANCE_TO_SWIM, 0);

		isReadyToDropAndSwim = true;
		isDropping = true;
	}

	void DropToWater () {
		if (CanAffectTranform ()) {
			rigidBody.velocity = Vector2.zero;

			float translation = SPEED * Time.deltaTime;
			gameObject.transform.position = Vector2.MoveTowards 
			(transform.position, placeToDropAt, translation);
		}

		if (transform.position.Equals (placeToDropAt)) {
			isDropping = false;
			isSwimming = true;
		}
	}

	void SwimOff () {
		if (CanAffectTranform ()) {
			float translation = SPEED * Time.deltaTime;
			gameObject.transform.position = Vector2.MoveTowards 
			(transform.position, placeToSwimTo, translation);
		}

		if (transform.position.Equals (placeToSwimTo)) {
			isSwimming = false;
		}
	}

	void ReturnToSpawnPosition (){

		if (CanAffectTranform ()) {
			transform.position = startPosition;
		}

	}

	//-------------------------------------------
	// Utilities
	//-------------------------------------------

	bool CanAffectTranform () {
		bool canAffect = false;

		if (isLocalPlayer && name.StartsWith (Constants.PLAYER_NAME)) {
			canAffect = true;

		} else if (isServer && name.StartsWith (Constants.CAKE_NAME)){
			canAffect = true;
		}

		return canAffect;
	}
}
