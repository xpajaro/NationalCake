using UnityEngine;
using System.Collections;

public class Spill : MonoBehaviour {

	float EXPIRATION_TIME = 8;
	float SLIP_MAGNITUDE = 2;

	// Use this for initialization
	void Start () {
		Invoke ("ExpireItem", EXPIRATION_TIME);
	}

	void OnCollisionEnter2D (Collision2D col)
	{	if (GameSetup.isHost) {
			Rigidbody2D actorBody = col.gameObject.GetComponent<Rigidbody2D> ();
			actorBody.velocity *= SLIP_MAGNITUDE;
		}
	}

	void ExpireItem (){
		Destroy (this.gameObject);
	}
}
