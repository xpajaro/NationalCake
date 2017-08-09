using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Gong : NetworkBehaviour {

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
		animator = GetComponent<Animator> ();

		spriteRenderer = GetComponent<SpriteRenderer> ();
		blueColor = spriteRenderer.color.b;
		greenColor = spriteRenderer.color.g;
	}


	void OnCollisionEnter2D (Collision2D col)
	{	
		if (GameState.gameEnded) {
			return;
		}

		if (isServer && !swapped) {
			string actorName = col.gameObject.name;

			if (actorName.StartsWith (Constants.PLAYER_NAME) || 
				actorName.StartsWith (Constants.ENEMY_NAME)) {

				RpcHandleSwap ();
			}
		}
	}

	[ClientRpc]
	void RpcHandleSwap() {
		SoundManager.Instance.PlaySingle (gongSound);
		SwapSides ();
		Darken ();
		Invoke ("Revive", COOL_DOWN);
		swapped = true;
	}

	//handle moving after swap
	void SwapSides (){
		Vector2 tempPosition = pGoal.transform.position;
		pGoal.transform.position = eGoal.transform.position;
		eGoal.transform.position = tempPosition;
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
