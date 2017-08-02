using System;
using UnityEngine;

public class ActivateItem {
	GameObject cake, cakeEffigy;
	SpriteRenderer cakeRenderer, cakeEffigyRenderer;

	public MonoBehaviour controller;

	public static int INVALID_ICON = -1;

	public static float JUJU_COOLDOWN = 8;

	Vector2 barrelHalfRect = new Vector2 (.46f, .9f);


	public ActivateItem (GameObject _cake, GameObject _cakeEffigy)
	{
		cake = _cake;
		cakeEffigy = _cakeEffigy;

		cakeRenderer = cake.GetComponent <SpriteRenderer> ();
		cakeEffigyRenderer = cakeEffigy.GetComponent <SpriteRenderer> ();
	}

	public int Activate (GameObject icon, Vector2 position){
		int iconType = INVALID_ICON;

		if (icon.name.StartsWith (Constants.ICON_GHOST_NAME)) {
			iconType = CakeProcessor ();

		} else if (icon.name.StartsWith (Constants.ICON_SPILL_NAME)) {
			iconType = SpillProcessor (position);

		} else if (icon.name.StartsWith (Constants.ICON_BLOC_NAME)) {
			iconType = BarrelProcessor (position);
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

		if (StageManager.isOnStage (cake.transform.position)) {
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

	int BarrelProcessor (Vector2 position){
		int iconType = INVALID_ICON;

		if (StageManager.isOnStage (position) &&
			!areaOccupied (position) ) {

			Communicator.Instance.ShareItemUse (Constants.ITEM_BARREL, position);
			ActivateBarrel (position);

			iconType = Constants.ITEM_BARREL;
		}
		return iconType;
	}

	bool areaOccupied(Vector2 point){
		Vector2 startPos = point + barrelHalfRect;
		Vector2 endPos = point - barrelHalfRect;

		bool collision = (Physics2D.OverlapArea (startPos, endPos) != null);

		if (!collision && !GameSetup.isHost) { //cake has no collider on client
			float distToCake = Vector2.Distance (point, cake.transform.position);
			collision = (distToCake < .5f);
		}

		return collision;
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
		
		SoundManager.instance.PlaySingle (Keeper.itemDropSound);
	}

	public void ActivateBarrel (Vector2 position){
		GameObject newBarrel = (GameObject) StageManager.Instantiate ( 
			Resources.Load( "items/" + Constants.ITEM_NAME_BLOC ), position, Quaternion.identity);
		Bloc.activeBarrels.Add (newBarrel);

		SoundManager.instance.PlaySingle (Keeper.itemDropSound);
	}

	public void ActivateBomb (Vector2 position){
//		foreach(GameObject bomb in Bomb.instance.activeBombs) {
//			Vector2 bombPosition = bomb.transform.position;
//
//			if (bombPosition.Equals (position)) {
//				Bomb.TriggerExplosion (bomb);
//			}
//		}
	}

	//-------------------------------------------
	// cleanup
	//-------------------------------------------

	void Cleanup (GameObject icon){
		GameControls.Destroy (icon);
	}
}


