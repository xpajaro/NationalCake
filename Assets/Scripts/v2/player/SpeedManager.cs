using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SpeedManager : NetworkBehaviour {

	public Image speedMeter;
	public const float DEFAULT_SPEED = 2f;
	const float DECCELRATION = 0.15f; //0.8f
	const float DECCELRATION_INTERVAL = .5f; //2f

	[SyncVar]
	public float currentSpeed; 

	public static PlayerController PlayerInstance = null;    
	public static PlayerController EnemyInstance = null;   


	public override void OnStartServer(){
		currentSpeed = DEFAULT_SPEED;
		InvokeRepeating("SlowDown", 0.0f, DECCELRATION_INTERVAL);
	}

	void Update(){
		UpdateMeter ();
	}


	void SlowDown (){
		if (currentSpeed > DEFAULT_SPEED) {
			currentSpeed = currentSpeed - DECCELRATION; // *deccel
		} else {
			currentSpeed = DEFAULT_SPEED;
		}
	}

	public void ResetSpeed (){
		currentSpeed = DEFAULT_SPEED;
	}

	void UpdateMeter(){
		if (IsLocalPlayer (name)) {
			speedMeter.fillAmount = currentSpeed / JerryCan.MAX_BOOST;
		}
	}

	bool IsLocalPlayer(string characterName){
		return (isServer && characterName.StartsWith (Constants.PLAYER_NAME)) ||
			(!isServer && characterName.StartsWith (Constants.ENEMY_NAME));
	}
}
