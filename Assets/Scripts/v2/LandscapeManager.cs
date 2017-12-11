using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandscapeManager : MonoBehaviour {

	Animator animator;
	public RuntimeAnimatorController lvl1Animator, lvl2Animator, lvl3Animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		animator.runtimeAnimatorController = GetAnimator();
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
