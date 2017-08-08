using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ActivateAttack : NetworkBehaviour {
	public int itemID;

	public Vector2 btnPosition;
	RectTransform rectTransform;

	void Start (){
		rectTransform = GetComponent<RectTransform> ();
		btnPosition = rectTransform.localPosition;
	}

	public void Activate (){
		Highlight ();
		GameController.LocalInstance.selectedItemRef = this;
	}

	public void Highlight(){
		rectTransform.localScale = new Vector2 (2, 2);
		rectTransform.localRotation = Quaternion.Euler (0, 0, -30);
	}

	public void Normalize(){
		rectTransform.localScale = new Vector2 (1.5f, 1.5f);
		rectTransform.localRotation = Quaternion.identity;
	}
}
