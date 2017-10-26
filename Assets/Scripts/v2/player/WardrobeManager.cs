using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WardrobeManager : NetworkBehaviour {

	const string PLAYER_VELOCITY_PARAMETER = "playerVelocity";
	const string ENEMY_VELOCITY_PARAMETER = "enemyVelocity";

	Animator animator;
	public RuntimeAnimatorController blueEnemyAnimator, blackEnemyAnimator, 
	redEnemyAnimator, whiteEnemyAnimator;

	SpriteRenderer spriteRenderer;
	Rigidbody2D playerBody;

	public Sprite blueEnemySprite, blackEnemySprite, 
	whiteEnemySprite, redEnemySprite;

	public GameObject marker;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		playerBody = GetComponent<Rigidbody2D>();

		SetupCharacters ();
	}
	
	// player is server
	// enemy is client
	void Update() {
		AnimateCharacters ();
	}


	void SetupCharacters (){
		if (IsOpponent ()) {
			spriteRenderer.sprite = GetSprite();
			animator.runtimeAnimatorController = GetAnimator();

		} else {
			PlaceMarker ();
		}
	}

	Sprite GetSprite(){
		Sprite newSprite;

		switch (SessionManager.Instance.currentRoom.ID) {
		case 0:
			newSprite = blueEnemySprite;
			break;
		case 1:
			newSprite = redEnemySprite;
			break;
		case 2:
			newSprite = blackEnemySprite;
			break;
		default:
			newSprite = whiteEnemySprite;
			break;

		}

		return newSprite;
	
	}

	RuntimeAnimatorController GetAnimator(){
		RuntimeAnimatorController newAnimator;

		switch (SessionManager.Instance.currentRoom.ID) {
		case 0:
			newAnimator = blueEnemyAnimator;
			break;
		case 1:
			newAnimator = redEnemyAnimator;
			break;
		case 2:
			newAnimator = blackEnemyAnimator;
			break;
		default:
			newAnimator = whiteEnemyAnimator;
			break;

		}

		return newAnimator;
	}

	void AnimateCharacters (){
		if (!IsOpponent ()) {
			animator.SetFloat (PLAYER_VELOCITY_PARAMETER, playerBody.velocity.magnitude);

		} else {
			animator.SetFloat (ENEMY_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		}
	}

	bool IsOpponent (){
		return (isServer && tag.Equals (Constants.ENEMY_NAME)) ||
		(!isServer && tag.Equals (Constants.PLAYER_NAME));
	}


	public void PlaceMarker(){
		Vector3 markerPosition = transform.position + new Vector3 (0,1,0);
		GameObject _marker = Instantiate (marker, markerPosition, Quaternion.identity) as GameObject;
		_marker.transform.parent = transform;
	}

}
