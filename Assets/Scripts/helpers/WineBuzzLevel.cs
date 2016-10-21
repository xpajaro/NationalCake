using System;
using UnityEngine;

public class WineBuzzLevel : MonoBehaviour
{
	public static float DEFAULT_LEVEL = 3f;
	public static float PlayerBuzz , EnemyBuzz ;
	public float SOBER_RATE = 0.8f;

	void Start (){
		InitializeLevels ();
		InvokeRepeating("SoberUpActors", 0.0f, 2.0f);
	}

	void InitializeLevels(){
		PlayerBuzz = DEFAULT_LEVEL;
		EnemyBuzz = DEFAULT_LEVEL;
	}

	void SoberUpActors(){
		SoberUp (ref PlayerBuzz);
		SoberUp (ref EnemyBuzz);
	}

	void SoberUp(ref float level){
		if (level > 1) {
			level = level * SOBER_RATE;
		} else {
			level = 1;
		}
			
	}
}


