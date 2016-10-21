using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cake : MonoBehaviour {
	string PLAYER_GOAL = "pGoal";
	string ENEMY_GOAL = "eGoal";

	void OnCollisionEnter2D (Collision2D col)
	{
		if (GameSetup.isHost) {
			string cause = col.gameObject.name;

			if (cause.Equals (PLAYER_GOAL) || cause.Equals (ENEMY_GOAL)) {	
			
				if (cause.Equals (PLAYER_GOAL)) {
					GameState.GameWon = true;
				} else {	
					GameState.GameWon = false;
				}

				Communicator.Instance.ShareGameState ();

				StopMoving ();
				LockOnGoal (col.gameObject);

				Invoke ("LeaveScene", 5f);
			}
		}

	}

	void LockOnGoal (GameObject goal){
		transform.position = goal.transform.position;
	}

	void StopMoving (){
		GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
	}

	void LeaveScene(){
		GameSetup.EndGame ();
	}
}
