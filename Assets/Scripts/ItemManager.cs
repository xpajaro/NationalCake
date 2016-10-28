using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	public GameObject holder1, holder2;
	static public IconHolderState holder1State, holder2State;

	float DROP_ITEM_COOLDOWN = 10f; //change to 40

	// Use this for initialization
	void Start () {
		holder1State = new IconHolderState (holder1);
		holder2State = new IconHolderState (holder2);

		if (GameSetup.isHost) {
			InvokeRepeating ("DropItem", 0.0f, DROP_ITEM_COOLDOWN);
		}
		GameControls.itemManager = this;
	}


	public static void SaveItem (int itemType){
		if (holder1State.icon == null) {
			holder1State.icon = SpawnNewItem (itemType, holder1State.holder.transform.position);
			
		} else if (holder1State.icon == null) {
			holder2State.icon = SpawnNewItem (itemType, holder2State.holder.transform.position);
		}
	}

	static GameObject SpawnNewItem (int itemType, Vector3 position){
		GameObject itemIcon = (GameObject) Instantiate ( Resources.Load( GetIconNameByID(itemType) ));
		itemIcon.transform.position = position;

		return itemIcon;
	}

	public IconHolderState GetItemHolder (GameObject icon){
		IconHolderState holderState = null;

		if (icon == holder1State.icon) {
			holderState =  holder1State;

		} else if (icon == holder1State.icon) {
			holderState =  holder2State;
		}

		return holderState;
	}

	public void FreeHolder (GameObject icon){
		if ( icon == holder1State.icon ) {
			holder1State.icon = null;
		} else if ( icon == holder2State.icon) {
			holder2State.icon = null;
		}
	}

	public void DropItem (){
		System.Random r = new System.Random();
		int rInt = r.Next(2, Constants.ITEM_JUJU); //for ints

		Vector3 newPos = GetRandomStagePosition ();

		GameObject newItem = (GameObject) Instantiate ( Resources.Load( GetPickupNameByID(rInt) ));
		newItem.transform.position = newPos;

		Communicator.Instance.ShareItemDrop (rInt, newPos);
	}

	Vector3 GetRandomStagePosition(){
		Vector3 newPos = Utilities.GetRandomStagePosition ();

		while (!StageManager.isOnStage (newPos)){
			newPos = Utilities.GetRandomStagePosition ();
		}
		return newPos;
	}

	public static string GetPickupNameByID (int ID){
		string pickupName = "itemPickups/" ;

		if (ID == 0) {
			pickupName += "barrelPickup";
		} else if (ID == 1) {
			pickupName += "spillPickup";
		} else if (ID == 2) {
			pickupName += "jujuPickup";
		} 

		return pickupName;
	}

	static string GetIconNameByID (int ID){
		string controllerName = "itemIcons/";

		if (ID == 0) {
			controllerName += "barrelIcon";
		} else if (ID == 1) {
			controllerName += "spillIcon";
		} else if (ID == 2) {
			controllerName += "jujuIcon";
		} 

		return controllerName;
	}


}
