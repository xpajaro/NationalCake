using UnityEngine;
using System.Collections;

public class Wine : MonoBehaviour {

	int RESPAWN_TIME = 300; //5 secs
	public static int SHOW_WINE = 0;
	public static int HIDE_WINE = 1;
	int respawn_countdown;

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
		if (col.gameObject.name == "player" || col.gameObject.name == "enemy") {
			//send to client to hide
			//Communicator.Instance.ShareWineState (int.Parse(tag), HIDE_WINE); 

			//hide
			Presenter.Detach (this.gameObject, spriteRenderer);


			//show loading bubbles

			//add to speed
		}

	}


	void Update(){
		//if beer is hidden, start countdown
		//after countdown, reattach beer
			//remove loading animation
			//reset countdown
	}
}
