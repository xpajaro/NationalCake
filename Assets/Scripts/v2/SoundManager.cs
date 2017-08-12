using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource, musicSource;  
	public AudioClip invalidAction, clickSound;
	public bool keepAlive;

	bool fxMuted;

	public static SoundManager Instance = null;  

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;

			if (keepAlive) {
				DontDestroyOnLoad (gameObject);
			}
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	public void ToggleAllSound(bool allSoundOn){
		fxMuted = !allSoundOn;
		ToggleMusic (allSoundOn);
	}

	public void ToggleMusic(bool musicOn){
		if (musicOn) {
			musicSource.Play ();
		} else {
			musicSource.Pause ();
		}
	}


	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip) {
		float volume = fxMuted ? 0 : 0.3f;
		PlaySingle (clip, volume);
	}

	public void PlaySingle(AudioClip clip, float level) {
		float volume = fxMuted ? 0 : level;
		efxSource.PlayOneShot (clip, volume);
	}


}