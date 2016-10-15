using UnityEngine;
using System.Collections;

public class FallingAnimator {

	int GAME_LAYER = 0 ;
	int COLLISION_FREE_LAYER_ACTOR = 8 ;
	int COLLISION_FREE_LAYER_ENEMY = 9 ;
	int COLLISION_FREE_LAYER_CAKE = 10 ;
	string SORTING_LAYER_WATER_TOP = "water-top" ;
	string SORTING_LAYER_RIG_TOP = "rig-top" ;

	//no of frames for drop and drown animations
	int DROP_ANIMATION_TIME = 35; 
	int DROWN_ANIMATION_TIME = 120;
	int animationSpan; 

	//fall adjustments (how far)
	Vector3 DROP_VECTOR = new Vector3 (0, -2.0f, 0);
	Vector3 DROWN_VECTOR =  new Vector3 (-20.0f, 0, 0);

	Vector3 start, dropDestination, drownDestination;

	bool fallStarted;

	public bool FallCompleted {
		get; set;
	}

	GameObject actor;
	SpriteRenderer actorRenderer ;

	int animationCounter;
	float dropProgress = 0, drownProgress = 0;


	public FallingAnimator (GameObject _actor){
		actor = _actor;
		actorRenderer = actor.GetComponent<SpriteRenderer> ();

		animationSpan  = DROP_ANIMATION_TIME + DROWN_ANIMATION_TIME;  
	}

	//called on updates of implementing classes
	public void animateFall (){
		PrepareForDropAndDrown ();

		if (animationCounter < animationSpan) {
			DropAndDrown ();
		} else {
			FallCompleted = true;
			Revive ();
			Debug.Log ("counter revive");
		}

		animationCounter++; //drives animation
	}

	void PrepareForDropAndDrown (){

		if (!fallStarted) {
			Detach ();
			CalculateMilestones ();

			Debug.Log ("start pos " + start.ToString("G4") + 
				" drop dest " + dropDestination.ToString("G4") +
				" drown dest " + drownDestination.ToString("G4"));

			fallStarted = true;
			FallCompleted = false;
		}
	}


	void DropAndDrown (){
		Debug.Log ("counter " + animationCounter + " drop progress " + dropProgress 
			+ " drown progress " + drownProgress);

		if (dropProgress < 1) {
			dropProgress = Drop ();
		} else {
			drownProgress = Drown ();
		}
	}

	float Drop (){
		float pctDone = (float)animationCounter / DROP_ANIMATION_TIME; //prevent integer division
		return Interpolate (actor, start, dropDestination, pctDone);
	}

	float Drown  (){
		float pctDone = (float)(animationCounter - DROP_ANIMATION_TIME) / DROWN_ANIMATION_TIME; //dropping ends, progress starts at 1
		return Interpolate (actor, dropDestination, drownDestination, pctDone);
	}

	float Interpolate (GameObject actor, Vector3 start, Vector3 destination, float  pctDone){
		if (pctDone <= 1.0) {
			actor.transform.position = Vector3.Lerp (start, destination, pctDone);
		}
		return pctDone;
	}

	public void Revive (){
		ReturnToHome ();
		ReturnToRig ();
		startCollisions ();
	}

	public void Detach (){
		RemoveFromRig ();
		stopCollisions ();
	}



	//-------------------------------------------
	// utilities
	//-------------------------------------------


	void ReturnToHome (){
		if (actor.name.Equals ("player")) {
			actor.transform.position = StageManager.PLAYER_START_POSITION ;
		} else if (actor.name.Equals ("enemy")) {
			actor.transform.position = StageManager.ENEMY_START_POSITION ;
		} else  if (actor.name.Equals ("cake")) {
			actor.transform.position = StageManager.CAKE_START_POSITION ;
		}
	}

	void ReturnToRig (){
		actorRenderer.sortingLayerName = SORTING_LAYER_RIG_TOP;
	}


	void RemoveFromRig (){
		actorRenderer.sortingLayerName = SORTING_LAYER_WATER_TOP;
	}


	void stopCollisions (){
		if (actor.name.Equals ("player")) {
			actor.layer = COLLISION_FREE_LAYER_ACTOR ;
		} else if (actor.name.Equals ("enemy")) {
			actor.layer = COLLISION_FREE_LAYER_ENEMY ;
		} else  if (actor.name.Equals ("cake")) {
			actor.layer = COLLISION_FREE_LAYER_CAKE ;
		}
	}


	void startCollisions (){
		actor.layer = GAME_LAYER ;
	}

	void CalculateMilestones (){
		start = actor.transform.position;
		dropDestination = start + DROP_VECTOR;
		drownDestination = dropDestination + DROWN_VECTOR;
	}

	public void Reset (){
		fallStarted = false;
		FallCompleted = false;
		animationCounter = 0;
		dropProgress = 0;
		drownProgress = 0;
	}

}
