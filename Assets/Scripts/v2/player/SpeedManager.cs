using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpeedManager : NetworkBehaviour {

	public const float DEFAULT_SPEED = 2f;
	const float DECCELRATION = 0.8f;

	public float currentSpeed; 

	public static PlayerController PlayerInstance = null;    
	public static PlayerController EnemyInstance = null;   


	public override void OnStartServer(){
		currentSpeed = DEFAULT_SPEED;
		InvokeRepeating("SlowDown", 0.0f, 2.0f);
	}

	void SlowDown (){
		if (currentSpeed > DEFAULT_SPEED) {
			currentSpeed = currentSpeed * DECCELRATION;
		} else {
			currentSpeed = DEFAULT_SPEED;
		}
	}

	public void ResetSpeed (){
		currentSpeed = DEFAULT_SPEED;
	}
}
