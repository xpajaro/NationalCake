using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameSpawner : NetworkBehaviour {

	public GameObject playerPrefab;
	public GameObject enemyPrefab;

	public GameObject playerPosition;
	public GameObject enemyPosition;

	public GameObject cakePrefab;
	Vector3 CAKE_SPAWN_POSITION;

	public GameObject jerryCanPrefab;
	Vector2[] JERRY_CAN_POSITIONS;


	const float ITEM_SPAWN_INTERVAL = 8f; //20f; //change to 40?
	const float ITEM_CLEAR_RADIUS = .7f;

	public static GameObject serverPlayerRef, serverEnemyRef, serverCakeRef;
	public GameObject[] itemPrefabs, itemAttackPrefabs;
	public AudioClip itemDropSound;

	public static GameSpawner serverInstance;

	void Awake(){
		if (!serverInstance) {
			serverInstance = this;
		}
	}

	public override void OnStartServer()
	{
		SetupPositions ();

		SpawnPlayer ();
		SpawnEnemy ();
		SpawnCake ();
		SpawnJerryCans ();

		InvokeRepeating ("DropItem", 0.0f, ITEM_SPAWN_INTERVAL);
	}

	void SetupPositions (){
		CAKE_SPAWN_POSITION = new Vector3 (0, 0.3f, 0);

		JERRY_CAN_POSITIONS = new Vector2[4];
		JERRY_CAN_POSITIONS [0] = new Vector2 (-2.3f, 2.3f);
		JERRY_CAN_POSITIONS [1] = new Vector2 (-2.3f, -2.3f);
		JERRY_CAN_POSITIONS [2] = new Vector2 (2.3f, 2.3f);
		JERRY_CAN_POSITIONS [3] = new Vector2 (2.3f, -2.3f);
	}

	void SpawnPlayer(){

		GameObject player = (GameObject)Instantiate
			(playerPrefab, playerPosition.transform.position, Quaternion.identity);
		player.name = Constants.PLAYER_NAME;
		serverPlayerRef = player;

		NetworkServer.Spawn(player);
	}

	void SpawnEnemy(){

		GameObject enemy = (GameObject)Instantiate
			(enemyPrefab, enemyPosition.transform.position, Quaternion.identity);
		enemy.name = Constants.ENEMY_NAME;
		serverEnemyRef = enemy;

		NetworkServer.Spawn(enemy);
	}

	void SpawnCake()
	{
		GameObject cake = (GameObject)Instantiate
			(cakePrefab, CAKE_SPAWN_POSITION, Quaternion.identity);
		serverCakeRef = cake;

		NetworkServer.Spawn(cake);
	}

	void SpawnJerryCans()
	{
		for (int i = 0; i < JERRY_CAN_POSITIONS.Length; i++) {
			GameObject jerryCan = (GameObject)Instantiate
				(jerryCanPrefab, JERRY_CAN_POSITIONS[i], Quaternion.identity);

			NetworkServer.Spawn (jerryCan);
		}
	}


	public void DropItem (){
		System.Random r = new System.Random();
		int itemIndex = 0;//r.Next(0, 4);

		Vector2 newPos = GetRandomStagePosition ();

		GameObject newItem = (GameObject) Instantiate 
			(itemPrefabs[itemIndex], newPos, Quaternion.identity );

		NetworkServer.Spawn (newItem);

		SoundManager.Instance.PlaySingle (itemDropSound);
	}


	public void DropAttackItem (int itemIndex, Vector2 newPos){
		GameObject newItem = (GameObject) Instantiate 
			(itemAttackPrefabs[itemIndex], newPos, Quaternion.identity );

		NetworkServer.Spawn (newItem);

		SoundManager.Instance.PlaySingle (itemDropSound);
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
