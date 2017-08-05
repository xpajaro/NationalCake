using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PickupItem : NetworkBehaviour {
	public float LIFETIME = 7f;

	public AudioClip explosionSound, playerSlipping, powerUpSound;
	public Button itemButton; //button to click for item activation

	const int EXPLOSION_POWER = -300;
	const string EXPLOSION_PARAMETER = "touchedByUser";

	void Start(){
		if (isServer) {
			Invoke ("Disappear", LIFETIME);
		}

	}


	void OnTriggerEnter2D (Collider2D col)
	{	
		string objName = col.gameObject.name;

		//characters pick up bomb
		if ( (objName.StartsWith (Constants.PLAYER_NAME) || objName.Equals (Constants.ENEMY_NAME))
			&& name.StartsWith (Constants.ITEM_NAME_BOMB) ) {
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
		Debug.Log ("about to save");
		ItemManager.Instance.SaveItem (itemButton);
	}

	void Disappear (){
		Destroy (gameObject);
	}



}
