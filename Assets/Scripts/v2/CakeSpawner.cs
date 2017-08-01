using UnityEngine;
using UnityEngine.Networking;

public class CakeSpawner : NetworkBehaviour {

	public GameObject cakePrefab;
	Vector3 spawnPosition;

	public override void OnStartServer()
	{
		spawnPosition = new Vector3 (0, 0.3f, 0);

		GameObject cake = (GameObject)Instantiate(cakePrefab, spawnPosition, Quaternion.identity);
		NetworkServer.Spawn(cake);

	}
}