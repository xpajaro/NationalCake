using UnityEngine;
using System.Collections;

public class Gong : MonoBehaviour {

	public GameObject pGoal, eGoal;

	public static bool swapped;

	SpriteRenderer spriteRenderer;
	float blueColor, greenColor;

	float COOL_DOWN = 10f ;


	Animator animator;
	string GONG_VIBRATION_PARAMETER = "vibrating";

	public AudioClip gongSound;

	// Use this for initialization
	void Start () {
		Communicator.Instance.gong = this;
		animator = GetComponent<Animator> ();

		spriteRenderer = GetComponent<SpriteRenderer> ();
		blueColor = spriteRenderer.color.b;
		greenColor = spriteRenderer.color.g;
	}


	void OnCollisionEnter2D (Collision2D col)
	{	
		if (GameSetup.isHost && !swapped) {
			string actorName = col.gameObject.name;

			if (actorName.Equals ("player") || actorName.Equals ("enemy")) {
				Communicator.Instance.ShareGongSwap ();

				HandleSwap ();
			}
		}
	}

	public void HandleSwap() {
		SoundManager.Instance.PlaySingle (gongSound);
		SwapSides ();
		Darken ();
		Invoke ("Revive", COOL_DOWN);
		swapped = true;
	}

	//handle moving after swap
	void SwapSides (){
		if (!GameState.gameEnded) {
			Vector2 tempPosition = pGoal.transform.position;
			pGoal.transform.position = eGoal.transform.position;
			eGoal.transform.position = tempPosition;
		}
	}

	void Darken (){
		animator.SetBool (GONG_VIBRATION_PARAMETER, true);

		Color c = spriteRenderer.color;
		c.b = 0;
		c.g = 0;
		spriteRenderer.color = c;
	}

	void Brighten (){
		animator.SetBool (GONG_VIBRATION_PARAMETER, false);

		Color c = spriteRenderer.color;
		c.b = blueColor;
		c.g = greenColor;
		spriteRenderer.color = c;
	}


	void Revive (){
		SwapSides ();
		Brighten ();
		swapped = false;
	}

}
