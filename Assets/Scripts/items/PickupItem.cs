using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {
	public int itemType ;
	GameObject cake;
	public float LIFETIME = 7f;

	int EXPLOSION_POWER = -300;

	Animator animator;

	public static ItemManager itemManager;

	void Start(){
		animator = GetComponent<Animator>();
		Invoke ("Disappear", LIFETIME);
	}


	void OnTriggerEnter2D (Collider2D col)
	{	
		string actorName = col.gameObject.name;

		Debug.Log ("xxxx - triggered " + actorName);

		if (actorName.Equals ("player") || actorName.Equals ("enemy")) {
			if (itemType == 3) { //bomb
				Debug.Log ("xxxx - triggered bomb found ");
				DetonateBomb( col.gameObject);
			}
		} else if (actorName.Equals( "player")) {
			SaveItem ();
			Destroy (gameObject);
		}
	}

	void DetonateBomb(GameObject player){

		Communicator.Instance.ShareItemUse (Constants.ITEM_BOMB, transform.position);
		Bomb.TriggerExplosion (gameObject);

		if (GameSetup.isHost) {
			Rigidbody2D actorBody = player.GetComponent<Rigidbody2D> ();
			actorBody.AddForce (actorBody.velocity.normalized * EXPLOSION_POWER, ForceMode2D.Impulse);
		}
	}


	void SaveItem (){
		itemManager.SaveItem (itemType);
	}

	void Disappear (){
		Debug.Log ("xxxx - disappear in pickup code");
		Bomb.Deactivate (gameObject);
		Destroy (gameObject);
	}



}
