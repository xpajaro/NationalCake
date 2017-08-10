using UnityEngine;
using System.Collections;

public class GoalManager : MonoBehaviour {

	public GameObject pGoal, eGoal, greenSmoke, redSmoke;
	Vector2 pStart, eStart, pDestination, eDestination;

	public Vector2 DISTANCE_TO_MOVE = new Vector2 (1,0);
	public AudioClip goalMovementSound;

	public int TIMES_TO_MOVE = 4;
	public int WHEN_TO_MOVE = 2400; //1800f at 30f/s is one minute
	public int ANIMATION_DURATION = 60;
	int SMOKE_DURATION = 5;

	int timesMoved = 0, animationCounter = 0;
	GameObject greenSmokeRef, redSmokeRef;


	void Start () {
		if (!GameSetup.isHost){ //switch sides
			SwitchSides ();
		}
	}

	void FixedUpdate (){
		if (timesMoved < TIMES_TO_MOVE) {
			if (animationCounter >= WHEN_TO_MOVE) {
				if (Gong.swapped) {
					return;
				}

				float pctDone = (float) (animationCounter - WHEN_TO_MOVE) / ANIMATION_DURATION;

				if (pctDone == 0) {
					Prepare ();
					AddSmokeParticles ();
					SoundManager.Instance.PlaySingle (goalMovementSound);
					Invoke ("RemoveSmokeParticles", SMOKE_DURATION);
				} 

				MoveGoals (pctDone);

				if (pctDone == 100f) {
					animationCounter = 0;
					timesMoved++;
				}
			}
			animationCounter++;
		}
	}



	void Prepare(){
		pStart = pGoal.transform.position;
		eStart = eGoal.transform.position;

		if (GameSetup.isHost) {
			pDestination = pStart + DISTANCE_TO_MOVE;
			eDestination = eStart + (DISTANCE_TO_MOVE * -1);
		} else { // sides are switched for enemy
			pDestination = pStart + (DISTANCE_TO_MOVE * -1);
			eDestination = eStart + DISTANCE_TO_MOVE ;
		}

	}

	void MoveGoals (float pctDone){
		MoveGoal (pGoal, pStart, pDestination, pctDone);
		MoveGoal (eGoal, eStart, eDestination, pctDone);
	}

	void MoveGoal (GameObject goal, Vector2 start, Vector2 destination, float pctDone){
		Utilities.Interpolate (goal, start, destination, pctDone);
	}

	void SwitchSides (){
		Vector2 temp = pGoal.transform.position;
		pGoal.transform.position = eGoal.transform.position;
		eGoal.transform.position = temp;
	}

	void AddSmokeParticles(){
		greenSmokeRef = Instantiate (greenSmoke, pGoal.transform.position, greenSmoke.transform.rotation);
		greenSmokeRef.transform.parent = pGoal.transform;

		redSmokeRef = Instantiate (redSmoke, eGoal.transform.position, redSmoke.transform.rotation);
		redSmokeRef.transform.parent = eGoal.transform;

		if (!GameSetup.isHost) {
			greenSmokeRef.transform.Rotate(Vector3.up * -180);
			redSmokeRef.transform.Rotate(Vector3.up * 180);
		}
	}

	void RemoveSmokeParticles(){
		Destroy (greenSmokeRef);
		Destroy (redSmokeRef);
	}
}
