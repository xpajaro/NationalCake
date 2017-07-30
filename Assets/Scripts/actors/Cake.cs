using UnityEngine;
using UnityEngine.SceneManagement;
//using GooglePlayGames;
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
					GameState.gameWon = true;
				} else {	
					GameState.gameWon = false;
				}

				Communicator.Instance.ShareGameState ();

				GameState.gameEnded = true;
				StopMoving ();
				LockOnGoal (col.gameObject);

				Invoke ("LeaveScene", 5f);
			}
		}

	}

	void LockOnGoal (GameObject goal){
		float GOAL_RADIUS = 0.36f;
		Vector2 newPos = new Vector2 ();

		newPos.x = goal.transform.position.x;
		newPos.y = goal.transform.position.y + GOAL_RADIUS;

		transform.position = newPos;
	}

	void StopMoving (){
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
	}

	void LeaveScene(){
		GameSetup.LeaveGame ();
	}
}
