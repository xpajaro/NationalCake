using UnityEngine;
using System.Collections;

public class Spill : MonoBehaviour {

	float EXPIRATION_TIME = 8;
	float SLIP_MAGNITUDE = 150;

	Animator animator;
	string OIL_SPILL_ANIMATION_NAME = "oilSpilling";

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		animator.Play (OIL_SPILL_ANIMATION_NAME);

		Invoke ("ExpireItem", EXPIRATION_TIME);
	}

	void OnTriggerEnter2D (Collider2D col)
	{	if (GameSetup.isHost) {
			Rigidbody2D actorBody = col.gameObject.GetComponent<Rigidbody2D> ();
			actorBody.AddForce (actorBody.velocity.normalized * SLIP_MAGNITUDE, ForceMode2D.Impulse);
		}
	}

	void ExpireItem (){
		Destroy (this.gameObject);
	}
}
