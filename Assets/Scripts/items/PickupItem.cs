using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {
	public int itemType ;
	GameObject cake;
	public float LIFETIME = 7f;

	int EXPLOSION_POWER = -300;

	public static ItemManager itemManager;
	public AudioClip powerUpSound;

	void Start(){
		Utilities.UpdateSortingLayer (gameObject);

		Invoke ("Disappear", LIFETIME);
	}


	void OnTriggerEnter2D (Collider2D col)
	{	
		string actorName = col.gameObject.name;

		if ( (actorName.Equals ("player") || actorName.Equals ("enemy"))
			 && itemType == Constants.ITEM_BOMB
			 && GameSetup.isHost) {
				DetonateBomb (col.gameObject);

		} else if (actorName.Equals ("player")) {
			SoundManager.instance.PlaySingle (powerUpSound);
			SaveItem ();
			Destroy (gameObject);

		} else {
			Destroy (gameObject);
		}
	}

	void DetonateBomb(GameObject player){

		Communicator.Instance.ShareItemUse (Constants.ITEM_BOMB, transform.position);
		Bomb.TriggerExplosion (gameObject);

		FlingPlayer (player);
	}

	void FlingPlayer(GameObject player) {
		Rigidbody2D actorBody = player.GetComponent<Rigidbody2D> ();
		actorBody.AddForce (actorBody.velocity.normalized * EXPLOSION_POWER, ForceMode2D.Impulse);
	}


	void SaveItem (){
		itemManager.SaveItem (itemType);
	}

	void Disappear (){
		if (gameObject.name.StartsWith(Constants.ITEM_NAME_BOMB)){
			Bomb.Deactivate (gameObject);
		}

		Destroy (gameObject);
	}



}
