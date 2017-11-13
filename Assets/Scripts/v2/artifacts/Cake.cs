using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class Cake : NetworkBehaviour {

	public GameObject cakeGhost, prophet;
	SpriteRenderer spriteRenderer, ghostRenderer, prophetRenderer;

	public GameObject gameOverPanel;

	const string PLAYER_GOAL = "playerStation";
	const string ENEMY_GOAL = "enemyStation";
	const float GHOST_FX_DURATION = 8f;

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

	void OnTriggerEnter2D (Collider2D col){
		if (isServer) {
			string cause = col.gameObject.name;

			if (cause.Equals (PLAYER_GOAL) || cause.Equals (ENEMY_GOAL)) {	
				bool gameWon = false;

				if (cause.Equals (PLAYER_GOAL)) {
					gameWon = true;
				} 

				StopMoving ();
				LockOnGoal (col.gameObject);

				RpcConcludeGame (gameWon);
			}
		}

	}


	void StopMoving (){
		GetComponent<Rigidbody2D> ().velocity = Vector2.zero;
	}


	void LockOnGoal (GameObject goal){
		float GOAL_RADIUS = 0.36f;
		Vector2 newPos = new Vector2 ();

		newPos.x = goal.transform.position.x;
		newPos.y = goal.transform.position.y + GOAL_RADIUS;

		transform.position = newPos;
	}



	[ClientRpc] 
	void RpcConcludeGame(bool serverWonGame){

		gameOverPanel.SetActive (true);

		GameState.gameEnded = true;

		bool localPlayerWonGame = false;
		if (isServer && serverWonGame || !isServer && !serverWonGame) {
			localPlayerWonGame = true;
			GameState.gameWon = true;
		}

		SessionManager.Instance.UpdateReserves (localPlayerWonGame);
		StartCoroutine (LeaveScene (localPlayerWonGame));
	}



	IEnumerator LeaveScene(bool localPlayerWonGame){

		yield return new WaitForSeconds(5f);

		if (localPlayerWonGame) {
			SceneManager.LoadScene (Constants.GAME_WON_SCENE);

		} else {
			SceneManager.LoadScene (Constants.GAME_LOST_SCENE);
		}
	}

	public void ActivateGhost() {
		Presenter.Detach (gameObject, spriteRenderer);

		Presenter.Attach (cakeGhost, ghostRenderer);
		Presenter.Attach (prophet, prophetRenderer);
		ghostActivated = true;

		SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.GHOST_ACTIVATED);
		Invoke ("DeactivateGhost", GHOST_FX_DURATION);
	}

	public void DeactivateGhost (){
		Presenter.Attach (gameObject, spriteRenderer);

		Presenter.Detach (cakeGhost, ghostRenderer);
		Presenter.Detach (prophet, prophetRenderer);
		ghostActivated = false;
	}

}
