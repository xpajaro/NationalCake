using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class PickupItem : NetworkBehaviour {
	public float LIFETIME = 7f;

	public AudioClip explosionSound, playerSlipping, powerUpSound;
	public int itemToSave;

	const int EXPLOSION_POWER = -300;
	const string EXPLOSION_PARAMETER = "touchedByUser";

	void Start(){
		if (isServer) {
			Invoke ("Disappear", LIFETIME);
		}

	}

	//happens only on server
	void OnTriggerEnter2D (Collider2D col)
	{	
		string objName = col.gameObject.name;
		bool isGameCharacter = objName.StartsWith (Constants.PLAYER_NAME) || 
			objName.Equals (Constants.ENEMY_NAME);

		//characters pick up bomb
		if ( isGameCharacter && name.StartsWith (Constants.ITEM_NAME_BOMB) ) {
			DetonateBomb (col.gameObject);
		
		//characters pick up other items
		} else if (isGameCharacter) {
			bool isPlayer = objName.StartsWith (Constants.PLAYER_NAME);
			ItemManager.Instance.SaveItem (itemToSave, isPlayer);
			Destroy (gameObject);
		
		//hit by anything else
		} else {
			Destroy (gameObject);
		}
	}

	void DetonateBomb(GameObject player){

		Animator animator = GetComponent<Animator>();
		animator.SetTrigger (EXPLOSION_PARAMETER);
		RpcPlayExplosionSound ();

		FlingPlayer (player);
	}

	[ClientRpc]
	void RpcPlayExplosionSound(){
		SoundManager.Instance.PlaySingle (explosionSound, 1f);
		SoundManager.Instance.PlaySingle (playerSlipping);
	}

	void FlingPlayer(GameObject player) {
		Rigidbody2D actorBody = player.GetComponent<Rigidbody2D> ();
		actorBody.AddForce (actorBody.velocity.normalized * EXPLOSION_POWER, ForceMode2D.Impulse);
	}
		
	void Disappear (){
		Destroy (gameObject);
	}

	void GetItemID (){
	
	}


}
