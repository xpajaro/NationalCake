using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public GameObject cake, cakeEffigy, spillPrefab ;
	GameObject iconTouched;


	Rigidbody2D playerBody;

	Moving movePlayer;
	ActivateItem itemActivator;

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
					ItemManager.HighlightHolder (holderTouched);
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
		GameObject holder = ItemManager.GetItemHolder (iconTouched).holder;

		ItemManager.RemoveHolderHighlight (holder);

		if (itemType != ActivateItem.INVALID_ICON) {
			ItemManager.RemoveHolderIcon (holder);
		} 

		iconTouched = null;
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
