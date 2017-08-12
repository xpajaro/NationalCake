using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSpawner : NetworkBehaviour {

	Vector2[] JERRY_CAN_POSITIONS;


	const float ITEM_SPAWN_INTERVAL = 20f; //change to 40?
	const float ITEM_CLEAR_RADIUS = .7f;

	public GameObject[] itemPrefabs, itemAttackPrefabs;

	public static GameSpawner serverInstance;

	void Awake(){
		if (!serverInstance) {
			serverInstance = this;
		}
	}

	public override void OnStartServer()
	{
		InvokeRepeating ("DropItem", ITEM_SPAWN_INTERVAL, ITEM_SPAWN_INTERVAL);
	}


	public void DropItem (){
		System.Random r = new System.Random();
		int itemIndex = r.Next(0, 4);

		Vector2 newPos = GetRandomStagePosition ();

		GameObject newItem = (GameObject) Instantiate 
			(itemPrefabs[itemIndex], newPos, Quaternion.identity );

		NetworkServer.Spawn (newItem);

		GameController.LocalInstance.RpcPlayItemDropSound ();
	}


	public void DropAttackItem (int itemIndex, Vector2 newPos){
		GameObject newItem = (GameObject) Instantiate 
			(itemAttackPrefabs[itemIndex], newPos, Quaternion.identity );

		NetworkServer.Spawn (newItem);

		GameController.LocalInstance.RpcPlayItemDropSound ();
	}


	//-------------------------------------------
	// utilites
	//-------------------------------------------

	Vector2 GetRandomStagePosition(){
		Vector2 newPos = Stage.Instance.GetRandomStagePosition ();

		while (Physics2D.OverlapCircle(newPos, ITEM_CLEAR_RADIUS)){
			newPos = Stage.Instance.GetRandomStagePosition ();
		}

		return newPos;
	}
}
