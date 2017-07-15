using UnityEngine;
using System.Collections.Generic;

public class Spill : MonoBehaviour {

	//keep track of items for client updates 

	float EXPIRATION_TIME = 8;
	float SLIP_MAGNITUDE = 150;

	Animator animator;
	string OIL_SPILL_ANIMATION_NAME = "oilSpilling";

	public AudioClip spillSound, playerSlipping;

	// Use this for initialization
	void Start () {
		StoreRefs ();

		animator = GetComponent<Animator> ();
		animator.Play (OIL_SPILL_ANIMATION_NAME);

		Invoke ("ExpireItem", EXPIRATION_TIME);
	}

	void StoreRefs(){
		Keeper.spillSound = spillSound;
		Keeper.playerSlipping = playerSlipping;	
	}

	void OnTriggerEnter2D (Collider2D col)
	{	if (GameSetup.isHost) {
			Communicator.Instance.ShareSlip ();

			Rigidbody2D actorBody = col.gameObject.GetComponent<Rigidbody2D> ();
			actorBody.AddForce (actorBody.velocity.normalized * SLIP_MAGNITUDE, ForceMode2D.Impulse);
			Spill.PlaySounds ();
		}
	}

	void ExpireItem (){
		Destroy (this.gameObject);
	}

	public static void PlaySounds(){
		SoundManager.instance.PlaySingle (Keeper.playerSlipping);
		SoundManager.instance.PlaySingle (Keeper.spillSound);
	}

}
