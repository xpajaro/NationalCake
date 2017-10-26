using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {

	Animator animator;
	public RuntimeAnimatorController lvl1Animator, lvl2Animator, lvl3Animator;

	SpriteRenderer spriteRenderer;

	public Sprite lvl1Sprite, lvl2Sprite, lvl3Sprite;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();


		spriteRenderer.sprite = GetSprite();
		animator.runtimeAnimatorController = GetAnimator();
	}

	Sprite GetSprite(){
		Sprite newSprite;

		switch (SessionManager.Instance.currentRoom.ID) {
		case 0:
			newSprite = lvl1Sprite;
			break;
		case 1:
			newSprite = lvl2Sprite;
			break;
		case 2:
			newSprite = lvl3Sprite;
			break;
		default:
			newSprite = lvl1Sprite;
			break;

		}

		return newSprite;

	}

	RuntimeAnimatorController GetAnimator(){
		RuntimeAnimatorController newAnimator;

		switch (SessionManager.Instance.currentRoom.ID) {
		case 0:
			newAnimator = lvl1Animator;
			break;
		case 1:
			newAnimator = lvl2Animator;
			break;
		case 2:
			newAnimator = lvl3Animator;
			break;
		default:
			newAnimator = lvl1Animator;
			break;

		}

		return newAnimator;
	}

}
