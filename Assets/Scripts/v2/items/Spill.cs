using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

public class Spill : NetworkBehaviour {

	//keep track of items for client updates 

	const float EXPIRATION_TIME = 8;
	const float SLIP_MAGNITUDE = 150;

	Animator animator;
	string OIL_SPILL_ANIMATION_NAME = "oilSpilling";


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		animator.Play (OIL_SPILL_ANIMATION_NAME);

		Invoke ("ExpireItem", EXPIRATION_TIME);
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (isServer) {
			Rigidbody2D actorBody = col.gameObject.GetComponent<Rigidbody2D> ();
			actorBody.AddForce (actorBody.velocity.normalized * SLIP_MAGNITUDE, ForceMode2D.Impulse);
		}

		if (IsLocalPlayer(col.gameObject.name)){
			PlaySounds ();
		}
	}

	void PlaySounds(){
		SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.PLAYER_IN_DANGER);
		SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.SLIPPED);
	}

	bool IsLocalPlayer(string characterName){
		return (isServer && characterName.StartsWith (Constants.PLAYER_NAME)) ||
			(!isServer && characterName.StartsWith (Constants.ENEMY_NAME));
	}

	void ExpireItem (){
		Destroy (this.gameObject);
	}

}
