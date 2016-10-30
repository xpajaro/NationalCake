using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUpdates : MonoBehaviour {

	public GameObject cake, cakeEffigy;
	ActivateItem itemActivator;

	void Start (){
		itemActivator = new ActivateItem (cake, cakeEffigy);
		Communicator.Instance.itemUpdates = this;
	}

	public void ShowDroppedItem (Dictionary<string, object> itemDropped){
		int itemID = (int) itemDropped [Deserialization.ITEM_KEY];
		Vector2 pos = (Vector2) itemDropped [Deserialization.POSITION_KEY];

		GameObject newItem = (GameObject) Instantiate ( Resources.Load( ItemManager.GetPickupNameByID(itemID) ));
		newItem.transform.position = pos;
	}

	public void UseItem (Dictionary<string, object> itemUsed){

		int itemID = (int) itemUsed [Deserialization.ITEM_KEY];

		if (itemID == Constants.ITEM_JUJU) {
			itemActivator.ActivateJuju ();
			Invoke ("DeactivateJuju", ActivateItem.JUJU_COOLDOWN);

		} else if (itemID == Constants.ITEM_SPILL){
			Vector2 pos = (Vector2) itemUsed [Deserialization.POSITION_KEY];
			itemActivator.ActivateSpill (pos);
		}
	}


	void DeactivateJuju (){
		itemActivator.DeactivateJuju ();
	}
}
