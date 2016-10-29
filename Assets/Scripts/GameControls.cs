using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public GameObject cake, cakeEffigy;
	GameObject iconTouched;


	Rigidbody2D playerBody;

	Moving movePlayer;
	ActivateItem itemActivator;
	public static ItemManager itemManager; //instantiated in the start of ItemManager class

	bool moving;

	void Start () {
		itemActivator = new ActivateItem (cake, cakeEffigy);
		movePlayer = new Moving (gameObject);
	}

	void FixedUpdate (){
		HandleTouch ();
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
				iconTouched = ItemManager.GetIconByHolder (holderTouched); 

				if (iconTouched != null) {
					ItemManager.ActivateHolder (holderTouched);
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
	void HandleItemActivation (Vector3 position){
		int itemType = itemActivator.Activate (iconTouched, Camera.main.ScreenToWorldPoint(position) );

		if (itemType == Constants.ITEM_JUJU) {
			Invoke ("DeactivateJuju", ActivateItem.JUJU_COOLDOWN);
		}

		CleanupItem (itemType);
	}

	void CleanupItem (int itemType){
		if (itemType != -1) {
			GameObject holder = itemManager.GetItemHolder (iconTouched).holder;

			itemManager.FreeHolder (holder);
			ItemManager.DectivateHolder (holder);

			iconTouched = null;
		}
	}

	void DeactivateJuju (){
		itemActivator.DeactivateJuju ();
	}

	GameObject GetHolderTouched (Touch touch){
		int layerMask = 1 << Constants.ITEM_ICONS_LAYER;
		GameObject holderTouched = Utilities.GetOverLappingItem (
			Camera.main.ScreenToWorldPoint(touch.position), layerMask);

		return holderTouched ;
	}

	GameObject GetIconTouched (GameObject holderTouched){
		return ItemManager.GetIconByHolder (holderTouched);
	}

}
