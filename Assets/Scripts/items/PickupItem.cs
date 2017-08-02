using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PickupItem : NetworkBehaviour {
	public int itemType ;
	public float LIFETIME = 7f;

	public AudioClip explosionSound, playerSlipping, powerUpSound;

	const int EXPLOSION_POWER = -300;
	const string EXPLOSION_PARAMETER = "touchedByUser";

	void Start(){
		Invoke ("Disappear", LIFETIME);
	}


	void OnTriggerEnter2D (Collider2D col)
	{	
		string objName = col.gameObject.name;

		//characters pick up bomb
		if ( (objName.StartsWith (Constants.PLAYER_NAME) || objName.Equals (Constants.ENEMY_NAME))
			 && itemType == Constants.ITEM_BOMB) {
				DetonateBomb (col.gameObject);
		
		//characters pick up other items
		} else if (objName.StartsWith (Constants.PLAYER_NAME)) {
			SoundManager.instance.PlaySingle (powerUpSound);
			SaveItem ();
			Destroy (gameObject);
		
		//hit by anything else
		} else {
			Destroy (gameObject);
		}
	}

	void DetonateBomb(GameObject player){

		// Communicator.Instance.ShareItemUse (Constants.ITEM_BOMB, transform.position);
		// Bomb.TriggerExplosion (gameObject);

		Animator animator = GetComponent<Animator>();
		animator.SetTrigger (EXPLOSION_PARAMETER);

		SoundManager.instance.PlaySingle (explosionSound, 1f);
		SoundManager.instance.PlaySingle (playerSlipping);

		FlingPlayer (player);
	}

	void FlingPlayer(GameObject player) {
		if (isServer) {
			Rigidbody2D actorBody = player.GetComponent<Rigidbody2D> ();
			actorBody.AddForce (actorBody.velocity.normalized * EXPLOSION_POWER, ForceMode2D.Impulse);
		}
	}


	void SaveItem (){
		// itemManager.SaveItem (itemType);
	}

	void Disappear (){
		Destroy (gameObject);
	}



}
