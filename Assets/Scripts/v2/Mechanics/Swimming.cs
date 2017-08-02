using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Swimming : NetworkBehaviour {

	const float DISTANCE_TO_DROP = -3.0f;
	const float DISTANCE_TO_SWIM = -20.0f;
	const float SPEED = 7f;

	bool fallTriggered;
	bool isReadyToDropAndSwim;
	bool isDropping;
	bool isSwimming;
	Vector3 placeToDropAt, placeToSwimTo;
	Vector3 startPosition;
	PlayerController playerController;
	SpeedManager speedManager;

	SpriteRenderer spriteRenderer;
	Rigidbody2D rigidBody;


	public AudioClip playerFallingSound;



	// Use this for initialization
	void Start () {

		spriteRenderer = GetComponent<SpriteRenderer> ();
		rigidBody = GetComponent<Rigidbody2D> ();

		startPosition = transform.position;

		if (ThisIsAPlayer ()) {
			playerController = GetComponent<PlayerController> ();
			speedManager = GetComponent<SpeedManager> ();
		}
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		TriggerFall ();
		HandleFall ();
	}

	void TriggerFall(){
		if (!fallTriggered && !Stage.Instance.IsOnStage (transform.position) ) {
			fallTriggered = true;

			if (ThisIsAPlayer ()) {
				playerController.isSwimming = true;

				if (isServer) { //only server affects transform
					speedManager.ResetSpeed ();
				}
			}

			//SoundManager.instance.PlaySingle (playerFallingSound);
		}
	}

	void HandleFall () {

		if (fallTriggered){

			if (!isReadyToDropAndSwim ) {
				CalculateFinalPositions ();
				Presenter.Detach (gameObject, spriteRenderer);
				rigidBody.velocity = Vector2.zero;

			} else if (isDropping) {
				DropToWater ();

			} else if (isSwimming) {
				SwimOff ();
			
			} else {
				ReturnToSpawnPosition ();
			}

			//fall and drown animation all done
			//done here because client doesn't do the dropping and swimming
			if (transform.position.Equals(startPosition) &&
				isReadyToDropAndSwim){
				Presenter.Attach (gameObject, spriteRenderer);

				isReadyToDropAndSwim = false;
				fallTriggered = false;

				if (ThisIsAPlayer ()) {
					playerController.SetupAfterSpawn ();
					playerController.isSwimming = false;
				}
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

		if (isServer) { //only server affects transform
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
		float translation = SPEED * Time.deltaTime;

		if (isServer) { //only server affects transform
			gameObject.transform.position = Vector2.MoveTowards 
				(transform.position, placeToSwimTo, translation);
		}

		if (transform.position.Equals (placeToSwimTo)) {
			isSwimming = false;
		}
	}

	void ReturnToSpawnPosition (){
		transform.position = startPosition;
	}

	//-------------------------------------------
	// Utilities
	//-------------------------------------------

	bool ThisIsAPlayer(){
		return name.StartsWith (Constants.PLAYER_NAME) ||
			name.StartsWith (Constants.ENEMY_NAME);
	}
}
