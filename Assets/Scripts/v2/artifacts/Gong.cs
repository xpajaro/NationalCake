using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Gong : NetworkBehaviour {

	public GameObject playerStation, enemyStation;

	public static bool swapped;

	SpriteRenderer spriteRenderer;

	float COOL_DOWN = 10f ;


	Animator animator;
	string GONG_VIBRATION_PARAMETER = "vibrating";

	public AudioClip gongSound;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();

		spriteRenderer = GetComponent<SpriteRenderer> ();
	}


	void OnCollisionEnter2D (Collision2D col)
	{	
		if (GameState.gameEnded) {
			return;
		}

		if (isServer && StationManager.stationMovementTriggered) {
			RpcPlayInvalidGongHitSound ();
			return;
		}

		//make sure it's not moving
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
		SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.GONG_HIT);
		SwapSides ();
		Darken ();
		Invoke ("Revive", COOL_DOWN);
		swapped = true;
	}

	[ClientRpc]
	void RpcPlayInvalidGongHitSound() {
		SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.INVALID_GONG_HIT);
	}

	//handle moving after swap
	void SwapSides (){
		Vector2 tempPosition = playerStation.transform.position;
		playerStation.transform.position = enemyStation.transform.position;
		enemyStation.transform.position = tempPosition;
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
		c.r = 255;
		c.b = 255;
		c.g = 255;
		spriteRenderer.color = c;
	}


	void Revive (){
		SwapSides ();
		Brighten ();
		swapped = false;
	}

}
