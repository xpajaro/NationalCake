﻿using UnityEngine;
using System.Collections;

public class FallingAnimator {

	//no of frames for drop and drown animations
	int DROP_ANIMATION_TIME = 30; 
	int DROWN_ANIMATION_TIME = 60;
	int REVIVE_ANIMATION_TIME = 0;
	int animationSpan; 

	//fall adjustments (how far)
	Vector2 DROP_VECTOR = new Vector2 (0, -2.0f);
	public static Vector2 DROWN_VECTOR =  new Vector2 (-20.0f, 0);

	Vector2 start, dropDestination, drownDestination;

	bool fallStarted;

	public bool FallCompleted {
		get; set;
	}

	GameObject actor;
	SpriteRenderer actorRenderer ;

	int animationCounter;
	float dropProgress = 0, drownProgress=0 ;


	public FallingAnimator (GameObject _actor){
		actor = _actor;
		actorRenderer = actor.GetComponent<SpriteRenderer> ();

		animationSpan  = DROP_ANIMATION_TIME + DROWN_ANIMATION_TIME + REVIVE_ANIMATION_TIME;  
	}

	//called on updates of implementing classes
	public void animateFall (){
		Prepare ();

		if (animationCounter <= animationSpan) {
			FallDrown ();
		} else {
			Revive ();
			FallCompleted = true;
			Presenter.Attach (actor, actorRenderer);
//			Debug.Log ("counter revive");
		}

		animationCounter++; //drives animation
	}

	void Prepare (){

		if (!fallStarted) {
			Presenter.Detach (actor, actorRenderer);
			CalculateMilestones ();

//			Debug.Log ("start pos " + start.ToString("G4") + 
//				" drop dest " + dropDestination.ToString("G4") +
//				" drown dest " + drownDestination.ToString("G4"));

			fallStarted = true;
			FallCompleted = false;
		}
	}


	void FallDrown (){
		//Debug.Log ("counter " + animationCounter + " drop progress " + dropProgress 
		//	+ " drown progress " + drownProgress);

		if (dropProgress < 1) {
			dropProgress = Fall ();
		} else if (drownProgress < 1) {
			drownProgress = Drown ();
		} 
		else {
			Revive ();
		}
	}

	float Fall (){
		float pctDone = (float)animationCounter / DROP_ANIMATION_TIME; //prevent integer division
		return Utilities.Interpolate (actor, start, dropDestination, pctDone);
	}

	float Drown  (){
		float pctDone = (float)(animationCounter - DROP_ANIMATION_TIME) / DROWN_ANIMATION_TIME; //dropping ends, progress starts at 1
		return Utilities.Interpolate (actor, dropDestination, drownDestination, pctDone);
	}

	void Revive  (){
		actor.transform.position = GetHomePosition ();
		//float pctDone = (float)(animationCounter - (DROP_ANIMATION_TIME + DROWN_ANIMATION_TIME)) / REVIVE_ANIMATION_TIME; 
		//return Utilities.Interpolate (actor, drownDestination, GetHomePosition (), pctDone);
	}



	//-------------------------------------------
	// utilities
	//-------------------------------------------


	Vector2 GetHomePosition (){
		Vector2 home = new Vector2();

		if (actor.name.Equals ("player")) {
			home = StageManager.PLAYER_START_POSITION ;
		} else if (actor.name.Equals ("enemy")) {
			home = StageManager.ENEMY_START_POSITION ;
		} else  if (actor.name.Equals ("cake")) {
			home = StageManager.CAKE_START_POSITION ;
		}

		return home;
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
