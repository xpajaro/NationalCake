using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActivateGhost : NetworkBehaviour {
	public AudioClip warningSound;
	RectTransform rectTransform;

	void Start (){
		rectTransform = GetComponent<RectTransform> ();
	}

	public void Activate () {
		if (Cake.LocalInstance.ghostActivated) {
			SoundManager.instance.PlaySingle (warningSound);
			
		} else {
			GameController.LocalInstance.ActivateGhost (rectTransform.localPosition);
			Destroy (gameObject);
		}
	}
}
