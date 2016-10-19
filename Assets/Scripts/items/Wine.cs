using UnityEngine;
using System.Collections;
using System;

public class Wine : MonoBehaviour {

	public float RESPAWN_TIME = 5f; 
	public float BUZZ = 3.5f;
	public float BUZZ_MAX = 20f;

	SpriteRenderer spriteRenderer;

	public GameObject cake;

	void Start(){
		spriteRenderer = this.gameObject.GetComponent<SpriteRenderer> ();
		IgnoreCake ();
	}

	void IgnoreCake (){
		Physics2D.IgnoreCollision (GetComponent<Collider2D>(), cake.GetComponent<Collider2D>());
	}

	void OnCollisionEnter2D (Collision2D col)
	{	
		string actorName = col.gameObject.name;
		if (actorName == "player" || actorName == "enemy") {

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
		if (actorName == "player") {
			WineLevel.PlayerLevel = Math.Min( BUZZ_MAX, WineLevel.PlayerLevel + BUZZ );
		} else if ( actorName == "enemy") {
			WineLevel.EnemyLevel =  Math.Min( BUZZ_MAX, WineLevel.EnemyLevel + BUZZ );
		}
	}


	void ShowAgain (){
		Presenter.Attach (this.gameObject, spriteRenderer);
	}
}
