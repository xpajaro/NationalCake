using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour {

	public AudioClip ghostActivatedSound, 
	playerDangerSound, playerRunningSound,
	itemDropSound, powerUpSound, gasUpSound,
	spillSound, explosionSound,
	blocHitSound, blocBrokenSound,
	gongHitSound, invalidGongHitSound, stationMovementSound, 
	invalidActionSound, clickSound;

	public enum SOUNDS {
		GHOST_ACTIVATED,
		PLAYER_IN_DANGER, PLAYER_RUNNING,
		ITEM_DROPPED, POWERED_UP, GASSED_UP,
		SLIPPED, EXPLOSION,
		BLOC_HIT, BLOC_BROKEN,
		GONG_HIT, STATION_MOVING,
		INVALID_ACTION_TAKEN, CLICKED,
		INVALID_GONG_HIT
	}

	private AudioClip[] soundCollection;

	const float GHOST_DURATION = 8f;

	public static SoundPlayer Instance = null;    

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		soundCollection = new AudioClip[15];
		soundCollection [0] = ghostActivatedSound;
		soundCollection [1] = playerDangerSound;
		soundCollection [2] = playerRunningSound;
		soundCollection [3] = itemDropSound;
		soundCollection [4] = powerUpSound;
		soundCollection [5] = gasUpSound;
		soundCollection [6] = spillSound;
		soundCollection [7] = explosionSound;
		soundCollection [8] = blocHitSound;
		soundCollection [9] = blocBrokenSound;
		soundCollection [10] = gongHitSound;
		soundCollection [11] = stationMovementSound;
		soundCollection [12] = invalidActionSound;
		soundCollection [13] = clickSound;
		soundCollection [14] = invalidGongHitSound;
	}

	public void Play (SOUNDS soundType){
		switch (soundType){
			case SOUNDS.GHOST_ACTIVATED:{
				PlayGhost();
				break;
			}

			default: {
				PlaySound (soundType);
				break;
			}
		}
	}

	void PlaySound (SOUNDS soundType){
		AudioClip audio = soundCollection [(int)soundType];
		SoundManager.Instance.PlaySingle (audio, 1f);
	}

	public void PlayGhost (){
		SoundManager.Instance.musicSource.Pause ();
		SoundManager.Instance.PlaySingle (ghostActivatedSound, 1f);
		Invoke ("ResumeMusic", GHOST_DURATION);
	}

	public void ResumeMusic	(){
		SoundManager.Instance.musicSource.Play ();
	}

}
