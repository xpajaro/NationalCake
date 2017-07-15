using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public AudioSource efxSource;                 
	public static SoundManager instance = null;     


	void Awake ()
	{
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
	}



	//Used to play single sound clips.
	public void PlaySingle(AudioClip clip)
	{
		PlaySingle (clip, 0.3f);
	}

	public void PlaySingle(AudioClip clip, float level)
	{
		efxSource.PlayOneShot (clip, level);
	}
}