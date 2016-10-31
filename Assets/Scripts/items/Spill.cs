using UnityEngine;
using System.Collections;

public class Spill : MonoBehaviour {

	float EXPIRATION_TIME = 8;
	float SLIP_MAGNITUDE = 150;

	// Use this for initialization
	void Start () {
		Invoke ("ExpireItem", EXPIRATION_TIME);
	}

	void OnTriggerEnter2D (Collider2D col)
	{	if (GameSetup.isHost) {
			Rigidbody2D actorBody = col.gameObject.GetComponent<Rigidbody2D> ();
			Debug.Log ("slip magnitude " + actorBody.velocity.ToString() + " normal " + actorBody.velocity.normalized.ToString());
			actorBody.AddForce (actorBody.velocity.normalized * SLIP_MAGNITUDE, ForceMode2D.Impulse);
		}
	}

	void ExpireItem (){
		Destroy (this.gameObject);
	}
}
