using UnityEngine;
using System.Collections.Generic;

public class Bloc : MonoBehaviour {
	
	public Sprite CRACKED_BARREL , BROKEN_BARREL ;
	public AudioClip barrelHit, barrelBroken;

	//keep track of items for client updates
	public static List<GameObject> activeBarrels = new List<GameObject>();    

	int noOfHits = 0;

	void Start (){
		StoreRefs ();
		Utilities.UpdateSortingLayer (gameObject);
	}

	void StoreRefs(){
		Keeper.CRACKED_BARREL = CRACKED_BARREL;
		Keeper.BROKEN_BARREL = BROKEN_BARREL;
		Keeper.barrelBroken = barrelBroken;
		Keeper.barrelHit = barrelHit;
	}

	void OnCollisionEnter2D (Collision2D col){	
		if (GameSetup.isHost) {
			Communicator.Instance.ShareBarrelhit(transform.position, noOfHits);

			Bloc.CrushBarrel (noOfHits, gameObject);
			noOfHits++;
		}
	}

	public static void updateClientBarrel (Dictionary<string, object>  barrelHit){
		int hitCount = (int) barrelHit [Deserialization.HITCOUNT_KEY];
		Vector2 pos = (Vector2) barrelHit [Deserialization.POSITION_KEY];

		foreach(GameObject barrel in Bloc.activeBarrels) {
			Vector2 barrelPosition = barrel.transform.position;

			if (barrelPosition.Equals (pos)) {
				Bloc.CrushBarrel (hitCount, barrel);
			}
		}
	}


	public static void CrushBarrel (int hitCount, GameObject barrel){

		SpriteRenderer renderer = barrel.GetComponent<SpriteRenderer> ();

		switch (hitCount) {
			case 0:
				renderer.sprite = Keeper.CRACKED_BARREL;
				SoundManager.instance.PlaySingle (Keeper.barrelHit);
				break;
			case 1:
				renderer.sprite = Keeper.BROKEN_BARREL;
				SoundManager.instance.PlaySingle (Keeper.barrelHit);
				break;
			case 2:
				ExpireItem (barrel);
				SoundManager.instance.PlaySingle (Keeper.barrelBroken);
				break;
			default:
				break;
		}

	}

	static void ExpireItem (GameObject barrel){
		Destroy (barrel);
		Bloc.activeBarrels.Remove (barrel);
	}
}
