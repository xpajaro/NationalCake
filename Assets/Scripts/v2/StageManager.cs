using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour {
	

	SpriteRenderer spriteRenderer;

	public Sprite lvl1Sprite, lvl2Sprite, lvl3Sprite;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();

		spriteRenderer.sprite = GetSprite();
	}

	Sprite GetSprite(){
		Sprite newSprite;

		switch (SessionManager.Instance.currentRoom.ID) {
		case 0:
		case 1:
			newSprite = lvl1Sprite;
			break;
		case 2:
			newSprite = lvl2Sprite;
			break;
		case 3:
			newSprite = lvl3Sprite;
			break;
		default:
			newSprite = lvl1Sprite;
			break;

		}

		return newSprite;

	}

}
