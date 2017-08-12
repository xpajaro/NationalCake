using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StationManager : NetworkBehaviour {

	public GameObject playerStation, enemyStation, greenSmoke, redSmoke;
	public Sprite greenStationSprite, redStationSprite;
	public RuntimeAnimatorController redStationAnimator, greenStationAnimator;

	ParticleSystem greenSmokeRef, redSmokeRef;
	bool stationMovementTriggered;
	Vector2 playerStationStartPosition, enemyStationStartPosition;
	int timesMoved = 1;

	Vector2 DISTANCE_TO_MOVE = new Vector2 (1,0);
	const int TIMES_TO_MOVE = 4;
	const int SMOKE_DURATION = 5;
	const float STATION_MOVEMENT_INTERVAL = 90f;
	const float STATION_MOVEMENT_SPEED = 2f;


	void Start () {
		if (!isServer) {
			SwitchSides ();

		} else {
			playerStationStartPosition = playerStation.transform.position;
			enemyStationStartPosition = enemyStation.transform.position;

			InvokeRepeating("TriggerStationMovement", 
				STATION_MOVEMENT_INTERVAL, 
				STATION_MOVEMENT_INTERVAL);
		}

		AddSmokeParticles ();
	}

	void Update() {
		if (!isServer) {
			return;
		}

		if (stationMovementTriggered) {
			MoveStation (playerStation, 
				playerStationStartPosition + (DISTANCE_TO_MOVE * timesMoved));
			
			MoveStation (enemyStation, 
				enemyStationStartPosition + (DISTANCE_TO_MOVE * -timesMoved));
		}
	}

	void TriggerStationMovement (){
		if (timesMoved < TIMES_TO_MOVE) {
			timesMoved++;

			RpcAddFx ();
			stationMovementTriggered = true;
		}
	}
		

	void MoveStation (GameObject station, Vector2 destination){
		float translation = STATION_MOVEMENT_SPEED * Time.deltaTime;

		station.transform.position = Vector2.MoveTowards 
			(station.transform.position, destination, translation);

		if (Vector2.Distance(station.transform.position, destination) < 0.01f) {
			stationMovementTriggered = false;
		}
	}

	void SwitchSides (){
		playerStation.GetComponent<SpriteRenderer>().sprite = redStationSprite;
		playerStation.GetComponent<Animator> ().runtimeAnimatorController = redStationAnimator;

		enemyStation.GetComponent<SpriteRenderer>().sprite = greenStationSprite;
		enemyStation.GetComponent<Animator> ().runtimeAnimatorController = greenStationAnimator;
	}

	[ClientRpc]
	void RpcAddFx(){
		SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.STATION_MOVING);

		BlowSmoke ();
		Invoke ("StopSmoke", SMOKE_DURATION);
	}

	void AddSmokeParticles (){
		if (isServer) {
			redSmokeRef = AddSmokeToStation(enemyStation, redSmoke, redSmoke.transform.rotation);
			greenSmokeRef = AddSmokeToStation(playerStation, greenSmoke, greenSmoke.transform.rotation);

		} else {
			redSmokeRef = AddSmokeToStation(playerStation, redSmoke, greenSmoke.transform.rotation);
			greenSmokeRef = AddSmokeToStation(enemyStation, greenSmoke, redSmoke.transform.rotation);
		}

		StopSmoke ();
	}

	ParticleSystem AddSmokeToStation (GameObject station, GameObject smoke, Quaternion rotation){
		GameObject _smoke = Instantiate (smoke, 
			station.transform.position, 
			rotation);
		
		_smoke.transform.parent = station.transform;

		return _smoke.GetComponent<ParticleSystem> ();
	}

	void BlowSmoke(){
		greenSmokeRef.Play ();
		redSmokeRef.Play ();
	}

	void StopSmoke (){
		greenSmokeRef.Stop ();
		redSmokeRef.Stop ();
	}



}
