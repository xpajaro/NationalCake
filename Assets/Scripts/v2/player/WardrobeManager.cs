using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WardrobeManager : NetworkBehaviour {

	const string PLAYER_VELOCITY_PARAMETER = "playerVelocity";
	const string ENEMY_VELOCITY_PARAMETER = "enemyVelocity";

	Animator animator;
	public RuntimeAnimatorController enemyAnimator;
	SpriteRenderer spriteRenderer;
	Rigidbody2D playerBody;

	public Sprite enemySprite;
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
			spriteRenderer.sprite = enemySprite;
			animator.runtimeAnimatorController = enemyAnimator;

		} else {
			PlaceMarker ();
		}
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
