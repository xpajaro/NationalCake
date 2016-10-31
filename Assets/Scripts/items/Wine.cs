using UnityEngine;
using System.Collections;
using System;

public class Wine : MonoBehaviour {

	float RESPAWN_TIME = 10f; 
	float BUZZ = 3.5f;
	float BUZZ_MAX = 20f;

	SpriteRenderer spriteRenderer;

	public GameObject cake;

	void Start(){
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		IgnoreCake ();
	}

	void IgnoreCake (){
		Physics2D.IgnoreCollision (GetComponent<Collider2D>(), cake.GetComponent<Collider2D>());
	}

	void OnTriggerEnter2D (Collider2D col)
	{	
		string actorName = col.gameObject.name;
		if (actorName.Equals("player") || actorName.Equals("enemy") ) {

			//hide
			Presenter.Detach (this.gameObject, spriteRenderer);

			// show again after countdwown
			Invoke("ShowAgain", RESPAWN_TIME);

			//add to speed
			if (GameSetup.isHost) {
				GoFaster (actorName);
			}

		}

	}

	void GoFaster (string actorName){
		if (actorName.Equals("player") ) {
			WineBuzzLevel.PlayerBuzz = Math.Min( BUZZ_MAX, WineBuzzLevel.PlayerBuzz + BUZZ );
		} else if ( actorName == "enemy") {
			WineBuzzLevel.EnemyBuzz =  Math.Min( BUZZ_MAX, WineBuzzLevel.EnemyBuzz + BUZZ );
		}
	}


	void ShowAgain (){
		Presenter.Attach (this.gameObject, spriteRenderer);
	}
}
