using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cake : NetworkBehaviour {

	public GameObject cakeGhost, prophet;
	SpriteRenderer spriteRenderer, ghostRenderer, prophetRenderer;

	const string PLAYER_GOAL = "pGoal";
	const string ENEMY_GOAL = "eGoal";
	const float GHOST_FX_DURATION = 8;

	[SyncVar]
	public bool ghostActivated;

	public static Cake LocalInstance;

	void Awake (){
		if (LocalInstance == null) {
			LocalInstance = this;
		}
	}

	void Start (){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		ghostRenderer = cakeGhost.GetComponent<SpriteRenderer> ();
		prophetRenderer = prophet.GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (isServer) {
			string cause = col.gameObject.name;

			if (cause.Equals (PLAYER_GOAL) || cause.Equals (ENEMY_GOAL)) {	
			
				if (cause.Equals (PLAYER_GOAL)) {
					GameState.gameWon = true;
				} else {	
					GameState.gameWon = false;
				}

				//Communicator.Instance.ShareGameState ();

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

	public void ActivateGhost() {
		Presenter.Detach (gameObject, spriteRenderer);

		Presenter.Attach (cakeGhost, ghostRenderer);
		Presenter.Attach (prophet, prophetRenderer);
		ghostActivated = true;

		Invoke ("DeactivateGhost", GHOST_FX_DURATION);
	}

	public void DeactivateGhost (){
		Presenter.Attach (gameObject, spriteRenderer);

		Presenter.Detach (cakeGhost, ghostRenderer);
		Presenter.Detach (prophet, prophetRenderer);
		ghostActivated = false;
	}

}
