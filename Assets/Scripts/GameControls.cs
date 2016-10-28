using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public GameObject cake, cakeEffigy;
	GameObject iconTouched, holder;


	Rigidbody2D playerBody;

	Moving movePlayer;
	ActivateItem itemActivator;
	public static ItemManager itemManager; //instantiated in the start of ItemManager class

	bool moving;

	void Start () {
		itemActivator = new ActivateItem (cake, cakeEffigy);
		movePlayer = new Moving (gameObject);
	}

	void Update (){
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

			} else if (touch.phase == TouchPhase.Moved){
				TouchMoving (touch);

			} else if (touch.phase == TouchPhase.Ended) {
				TouchEnded (touch);

			} else if (touch.phase == TouchPhase.Canceled) {
				TouchCanceled ();
			}
		}
	} 


	void TouchStarted (Touch touch){
		iconTouched = GetIconTouched (touch);

		if (iconTouched == null) {
			movePlayer.MovementInputStarted (touch.position) ;
			moving = true;
		}
	}

	void TouchMoving (Touch touch){
		if (iconTouched != null) {
			DragMovableIcons (touch);
		}
	}

	void TouchEnded (Touch touch) {
		if (iconTouched !=  null) {
			HandleItemActivation ();
		} else if (moving) {
			movePlayer.MovemenInputEnded (touch.position);
		}

		Cleanup ();
	}

	void TouchCanceled () {
		Cleanup ();
	}


	void Cleanup (){
		moving = false;
		itemManager.FreeHolder (iconTouched);
		iconTouched = null;
	}


	//-------------------------------------------
	// item/icon logic
	//-------------------------------------------
	void HandleItemActivation (){
		int itemType = itemActivator.activate (iconTouched);

		if (itemType == Constants.ITEM_JUJU) {
			Invoke ("DeactivateJuju", ActivateItem.JUJU_COOLDOWN);
		}
	}

	void DeactivateJuju (){
		itemActivator.DeactivateJuju ();
	}

	GameObject GetIconTouched (Touch touch){
		int layerMask = 1 << Constants.ITEM_ICONS_LAYER;
		GameObject objectTouched = Utilities.GetOverLappingItem (
			Camera.main.ScreenToWorldPoint(touch.position), layerMask);

		return objectTouched;
	}

	void DragMovableIcons (Touch touch){
		if (!iconTouched.name.Equals (Constants.ICON_JUJU_NAME)) {
			iconTouched.transform.position =  Camera.main.ScreenToWorldPoint(touch.position);
		}
	}


}
