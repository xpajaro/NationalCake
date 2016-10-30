using System;
using UnityEngine;

public class ActivateItem {
	GameObject cake, cakeEffigy;
	SpriteRenderer cakeRenderer, cakeEffigyRenderer;

	public MonoBehaviour controller;

	public static int INVALID_ICON = -1;

	public static float JUJU_COOLDOWN = 8;


	public ActivateItem (GameObject _cake, GameObject _cakeEffigy)
	{
		cake = _cake;
		cakeEffigy = _cakeEffigy;

		cakeRenderer = cake.GetComponent <SpriteRenderer> ();
		cakeEffigyRenderer = cakeEffigy.GetComponent <SpriteRenderer> ();
	}

	public int Activate (GameObject icon, Vector2 position){
		int iconType = INVALID_ICON;

		if (icon.name.StartsWith (Constants.ICON_JUJU_NAME)) {
			iconType = CakeProcessor ();

		} else if (icon.name.StartsWith (Constants.ICON_SPILL_NAME)) {
			iconType = SpillProcessor (position);
		}

		if (iconType != INVALID_ICON) {
			Cleanup (icon);
		}

		return iconType;
	}



	//-------------------------------------------
	// item processors
	//-------------------------------------------

	int CakeProcessor (){
		int iconType = INVALID_ICON;

		if (StageManager.cakeOnStage) {
			Communicator.Instance.ShareItemUse (Constants.ITEM_JUJU, Vector2.zero);
			ActivateJuju ();

			iconType = Constants.ITEM_JUJU;
		}

		return iconType;
	}

	int SpillProcessor (Vector2 position){
		int iconType = INVALID_ICON;

		if (StageManager.isOnStage (position)) {
			Communicator.Instance.ShareItemUse (Constants.ITEM_SPILL, position);
			ActivateSpill (position);

			iconType = Constants.ITEM_SPILL;
		}
		return iconType;
	}

	//-------------------------------------------
	// item activators
	//-------------------------------------------

	public void ActivateJuju (){
		Presenter.Detach (cake, cakeRenderer);
		Presenter.Attach (cakeEffigy, cakeEffigyRenderer);

		cakeEffigy.transform.position = cake.transform.position;
	}

	public void DeactivateJuju (){
		Presenter.Detach (cakeEffigy, cakeEffigyRenderer);
		Presenter.Attach (cake, cakeRenderer);
	}

	public void ActivateSpill (Vector2 position){
		StageManager.Instantiate ( 
			Resources.Load( "items/" + Constants.ITEM_NAME_SPILL ), position, Quaternion.identity);
	}

	//-------------------------------------------
	// cleanup
	//-------------------------------------------

	void Cleanup (GameObject icon){
		GameControls.Destroy (icon);
	}
}


