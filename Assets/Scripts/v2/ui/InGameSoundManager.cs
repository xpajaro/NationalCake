using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameSoundManager : MonoBehaviour {

	public Sprite onSprite, offSprite;

	bool outputOn = true; 

	const string SOUND_BUTTON_NAME = "btnSound";
	const string MUSIC_BUTTON_NAME = "btnMusic";

	
	public void ToggleOutput(){
		outputOn = changeButton ();

		switch (name) {
		case SOUND_BUTTON_NAME: 
			SoundManager.Instance.ToggleAllSound (outputOn);
			break;

		case MUSIC_BUTTON_NAME:
			SoundManager.Instance.ToggleMusic (outputOn);
			break;

		default:
			break;
		}
	}


	bool changeButton (){
		Image img = GetComponent<Image> ();
		img.sprite = outputOn ? onSprite : offSprite; //show on button if we're shutting off

		return !outputOn;
	}
}
