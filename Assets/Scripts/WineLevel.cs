using System;
using UnityEngine;

public class WineLevel : MonoBehaviour
{
	public static float PlayerLevel = 3f, EnemyLevel = 3f;
	public float SOBER_RATE = 0.8f;

	void Start (){
		InvokeRepeating("SoberUpActors", 0.0f, 2.0f);
	}


	void SoberUpActors(){
		SoberUp (ref PlayerLevel);
		SoberUp (ref EnemyLevel);
	}

	void SoberUp(ref float level){
		if (level > 1) {
			level = level * SOBER_RATE;
		} else {
			level = 1;
		}
			
	}
}


