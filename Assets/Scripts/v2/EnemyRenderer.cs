using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyRenderer : NetworkBehaviour {

	public Sprite enemySprite;
	public RuntimeAnimatorController enemyController;

	string ENEMY_VELOCITY_PARAMETER = "enemyVelocity";

	Animator animator;
	Rigidbody2D enemyBody;
	SpriteRenderer spriteRenderer;


	void Start () {
		if (!isLocalPlayer) {
			enemyBody = GetComponent<Rigidbody2D>();

			spriteRenderer = GetComponent<SpriteRenderer> ();
			spriteRenderer.sprite = enemySprite;

			animator = this.GetComponent<Animator>();
			animator.runtimeAnimatorController = enemyController;

		}
	}

	void Update() {
		if (!isLocalPlayer) {
			animator.SetFloat (ENEMY_VELOCITY_PARAMETER, enemyBody.velocity.magnitude);
		}
	}
}
