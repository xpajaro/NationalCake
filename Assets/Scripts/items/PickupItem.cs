using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour {
	public int itemType ;
	GameObject cake;
	public float LIFETIME = 7f;

	void Start(){
		IgnoreCake ();
		Invoke ("Disappear", LIFETIME);
	}

	void IgnoreCake (){
		cake = GameObject.Find ("cake");
		Physics2D.IgnoreCollision (GetComponent<Collider2D>(), cake.GetComponent<Collider2D>());
	}

	void OnTriggerEnter2D (Collider2D col)
	{	
		string actorName = col.gameObject.name;

		if (actorName.Equals( "player")) {
			SaveItem ();
		}

		Destroy (gameObject);

	}

	void SaveItem (){
		ItemManager.SaveItem (itemType);
	}

	void Disappear (){
		Destroy (gameObject);
	}

}
