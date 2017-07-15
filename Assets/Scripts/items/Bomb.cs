using System;
using UnityEngine;
using System.Collections.Generic;

public class Bomb: MonoBehaviour
{
	static string EXPLOSION_PARAMETER = "touchedByUser";

	//keep track of items for client updates
	public static List<GameObject> activeBombs = new List<GameObject>();    
	public AudioClip explosionSound, playerSlipping;

	public static Bomb instance = null;    

	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}


	public static void TriggerExplosion(GameObject bomb){
		Animator animator = bomb.GetComponent<Animator>();
		animator.SetTrigger (EXPLOSION_PARAMETER);

		SoundManager.instance.PlaySingle (instance.explosionSound, 1f);
		SoundManager.instance.PlaySingle (instance.playerSlipping);
	}

	public static void Deactivate (GameObject bomb){
		activeBombs.Remove (bomb);
	}
}


