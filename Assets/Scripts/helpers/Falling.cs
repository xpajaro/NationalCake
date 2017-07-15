using UnityEngine;
using System.Collections;

public class Falling : MonoBehaviour {


	public GameObject cake, player, enemy;
	SpriteRenderer cakeRenderer, playerRenderer, enemyRenderer;

	public static bool pFalling, eFalling, cFalling ;

	FallingAnimator pAnimator, eAnimator, cAnimator ;
	public AudioClip playerFallingSound;

	void Start(){

		pAnimator = new FallingAnimator (player);
		eAnimator = new FallingAnimator (enemy);
		cAnimator = new FallingAnimator (cake);

	}

	void FixedUpdate () {
		if (GameSetup.isHost) {
			TriggerFalling ();
			AnimateEveryFall ();
		}
	}

	void TriggerFalling (){
		//animate only if player is not on stage and fall animation hasn't started
		if (!StageManager.playerOnStage && !pFalling) {
			pFalling = true;
			WineBuzzLevel.PlayerBuzz = WineBuzzLevel.DEFAULT_LEVEL;

			SoundManager.instance.PlaySingle (playerFallingSound);
		}
		if (!StageManager.enemyOnStage && !pFalling) {
			eFalling = true; 
			WineBuzzLevel.EnemyBuzz = WineBuzzLevel.DEFAULT_LEVEL;
		}
		if (!StageManager.cakeOnStage && !pFalling) {
			cFalling = true;
		}
	}

	void AnimateEveryFall (){
		AnimateActorFalling (ref pFalling, pAnimator);
		AnimateActorFalling (ref eFalling, eAnimator);
		AnimateActorFalling (ref cFalling, cAnimator);
	}

	// animate only if fall animation is not completed
	void AnimateActorFalling (ref bool isFalling, FallingAnimator animator){
		if (isFalling) {
			animator.animateFall (); 

			if (animator.FallCompleted) {
				animator.Reset ();
				isFalling = false;
			}
		} 
	}

}
