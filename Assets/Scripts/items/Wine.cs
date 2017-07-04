using UnityEngine;
using System.Collections;
using System;

public class Wine : MonoBehaviour {

	float RESPAWN_TIME = 10f; 
	float BUZZ = 3f; //3.5 was good
	float BUZZ_MAX = 20f;

	SpriteRenderer spriteRenderer;

	public GameObject cake;

	void Start(){
		Utilities.UpdateSortingLayer (gameObject);

		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
	}

	void OnTriggerEnter2D (Collider2D col)
	{	
		string actorName = col.gameObject.name;

		//hide
		Presenter.Detach (this.gameObject, spriteRenderer);
		Invoke("ShowAgain", RESPAWN_TIME);

		if (actorName.Equals("player") || actorName.Equals("enemy") ) {
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
