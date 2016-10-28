using System;
using UnityEngine;

public class ActivateItem {
	GameObject cake, cakeEffigy;
	SpriteRenderer cakeRenderer, cakeEffigyRenderer;

	public MonoBehaviour controller;

	public static float JUJU_COOLDOWN = 8;


	public ActivateItem (GameObject _cake, GameObject _cakeEffigy)
	{
		cake = _cake;
		cakeEffigy = _cakeEffigy;

		cakeRenderer = cake.GetComponent <SpriteRenderer> ();
		cakeEffigyRenderer = cakeEffigy.GetComponent <SpriteRenderer> ();
	}

	public int activate (GameObject icon){
		int iconType = -1;

		if (icon.name.StartsWith (Constants.ICON_JUJU_NAME)) {
			if (StageManager.cakeOnStage) {
				Communicator.Instance.ShareItemUse (Constants.ITEM_JUJU, Vector3.zero);
				ActivateJuju ();

				iconType = Constants.ITEM_JUJU;
			}
		} 

		Cleanup (icon);

		return iconType;
	}

	public void ActivateJuju (){
		Presenter.Detach (cake, cakeRenderer);
		Presenter.Attach (cakeEffigy, cakeEffigyRenderer);

		cakeEffigy.transform.position = cake.transform.position;
	}

	public void DeactivateJuju (){
		Presenter.Detach (cakeEffigy, cakeEffigyRenderer);
		Presenter.Attach (cake, cakeRenderer);
	}



	//-------------------------------------------
	// cleanup
	//-------------------------------------------

	void Cleanup (GameObject icon){
		GameControls.Destroy (icon);
	}
}


