using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;   
	public AudioSource musicSource;                 
	public static SoundManager instance = null;  

	public AudioClip invalidAction, clickSound;
	bool fxMuted;

	void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (instance != this) {
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
		efxSource.PlayOneShot (clip, level);
	}

	public void PlayWarning() {
		float volume = fxMuted ? 0 : 0.5f;
		PlaySingle (invalidAction, volume);
	}
	public void PlayClick() {
		PlaySingle (clickSound, 1f);
	}

}