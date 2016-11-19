using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public GameObject cake, enemy, cakeEffigy, spillPrefab, holder1, holder2 ;

	float HOLDER_DISTANCE = 1.1f;

	GameObject iconTouched;

	public static ItemManager itemManager;

	Rigidbody2D playerBody, enemyBody;

	Moving movePlayer;
	ActivateItem itemActivator;

	Animator animator, enemyAnimator;
	string PLAYER_VELOCITY_PARAMETER = "playerVelocity";
	string ENEMY_VELOCITY_PARAMETER = "enemyVelocity";

	bool moving;

	void Start () {
		//components
		playerBody = GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		enemyAnimator = enemy.GetComponent<Animator> ();

		//classes
		itemActivator = new ActivateItem (cake, cakeEffigy);
		movePlayer = new Moving (gameObject);
	}

	void FixedUpdate (){
		HandleTouch ();
		UpdateAnimationParemeters ();
	}

	void UpdateAnimationParemeters (){
		animator.SetFloat (PLAYER_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		enemyAnimator.SetFloat (ENEMY_VELOCITY_PARAMETER, enemyBody.velocity.magnitude);
	}

	//-------------------------------------------
	// Handle player input -- consider making this screen touch not player touch
	//-------------------------------------------
	// host player decided how movement happens
	// client player sends impulse to host and host returns game state
	// game state contains all positions & velocities
	//-------------------------------------------

	void HandleTouch(){
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Began) {
				TouchStarted (touch);

			} else if (touch.phase == TouchPhase.Ended) {
				TouchEnded (touch);

			} else if (touch.phase == TouchPhase.Canceled) {
				TouchCanceled ();
			}
		}
	} 


	void TouchStarted (Touch touch){
		if (iconTouched != null ) { //touched before, activate item
			HandleItemActivation (touch.position);

		} else {
			GameObject holderTouched = GetHolderTouched (touch);

			if (holderTouched != null) {
				iconTouched = itemManager.GetIconByHolder (holderTouched); 

				if (iconTouched != null) {
					itemManager.HighlightHolder (holderTouched);
				} 
			}
			else {
				movePlayer.MovementInputStarted (touch.position);
				moving = true;
			}
		}
	}

	void TouchEnded (Touch touch) {
		if (moving) {
			movePlayer.MovementInputEnded (touch.position);
			moving = false;
		}
	}

	void TouchCanceled () {
		moving = false;
	}
		


	//-------------------------------------------
	// item/icon logic
	//-------------------------------------------
	void HandleItemActivation (Vector2 position){
		Vector2 scaledPosition = Camera.main.ScreenToWorldPoint (position);
		int itemType = itemActivator.Activate (iconTouched,  scaledPosition);

		if (itemType == Constants.ITEM_JUJU) {
			Invoke ("DeactivateJuju", ActivateItem.JUJU_COOLDOWN);
		} 

		CleanupHolder (itemType);
	}

	void CleanupHolder (int itemType){
		GameObject holder = itemManager.GetItemHolder (iconTouched).holder;

		itemManager.RemoveHolderHighlight (holder);

		if (itemType != ActivateItem.INVALID_ICON) {
			itemManager.RemoveHolderIcon (holder);
		} 

		iconTouched = null;
	}

	void DeactivateJuju (){
		itemActivator.DeactivateJuju ();
	}

	GameObject GetHolderTouched (Touch touch){
		Vector2 touchPosition = Camera.main.ScreenToWorldPoint (touch.position);
		GameObject holderTouched = null;

		if (Vector2.Distance (holder1.transform.position, touchPosition) < HOLDER_DISTANCE) {
			holderTouched = holder1;
		} else if (Vector2.Distance (holder2.transform.position, touchPosition) < HOLDER_DISTANCE) {
			holderTouched = holder2;
		}

		return holderTouched ;
	}


	GameObject GetIconTouched (GameObject holderTouched){
		return itemManager.GetIconByHolder (holderTouched);
	}

}
