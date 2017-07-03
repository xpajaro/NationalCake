using System;
using UnityEngine;
using System.Collections.Generic;

public class Bomb: MonoBehaviour
{
	static string EXPLOSION_PARAMETER = "touchedByUser";

	public static List<GameObject> activeBombs = new List<GameObject>();

	public static void TriggerExplosion(GameObject bomb){
		Debug.Log ("xxxx - trigger explosion");
		Animator animator = bomb.GetComponent<Animator>();
		animator.SetTrigger (EXPLOSION_PARAMETER);
	}

	public static void Deactivate (GameObject bomb){
		Debug.Log ("xxxx - deactivate in bomb code");
		activeBombs.Remove (bomb);
	}
}


