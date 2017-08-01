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
	Vector3 cakePosition;

	public static GameObject serverPlayerRef, serverEnemyRef, serverCakeRef;


	public override void OnStartServer()
	{
		SpawnPlayer ();
		SpawnEnemy ();
		SpawnCake ();
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
		cakePosition = new Vector3 (0, 0.3f, 0);

		GameObject cake = (GameObject)Instantiate(cakePrefab, cakePosition, Quaternion.identity);
		serverCakeRef = cake;

		NetworkServer.Spawn(cake);
	}
}
