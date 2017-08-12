using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActivateGhost : NetworkBehaviour {
	RectTransform rectTransform;

	void Start (){
		rectTransform = GetComponent<RectTransform> ();
	}

	public void Activate () {
		if (Cake.LocalInstance.ghostActivated) {
			SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.INVALID_ACTION_TAKEN);
			
		} else {
			GameController.LocalInstance.ActivateGhost (rectTransform.localPosition);
			Destroy (gameObject);
		}
	}
}
