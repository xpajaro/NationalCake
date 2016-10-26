using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemUpdates : MonoBehaviour {


	void Start (){
		Communicator.Instance.itemUpdates = this;
	}

	public void ShowDroppedItem (Dictionary<string, object> itemDropped){
		int itemID = (int) itemDropped [Deserialization.ITEM_KEY];
		Vector3 pos = (Vector3) itemDropped [Deserialization.POSITION_KEY];

		GameObject newItem = (GameObject) Instantiate ( Resources.Load( ItemManager.GetPickupNameByID(itemID) ));
		newItem.transform.position = pos;
	}
}
