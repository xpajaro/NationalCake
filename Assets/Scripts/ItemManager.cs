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
	}


	public static void SaveItem (int itemType){
		if (holder1State.icon == null) {
			holder1State.icon = SpawnNewItem (itemType, holder1State.holder.transform.position);
			
		} else if (holder2State.icon == null) {
			holder2State.icon = SpawnNewItem (itemType, holder2State.holder.transform.position);
		}
	}

	static GameObject SpawnNewItem (int itemType, Vector2 position){
		GameObject itemIcon = (GameObject) Instantiate ( Resources.Load( GetIconNameByID(itemType) ));
		itemIcon.transform.position = position;

		return itemIcon;
	}

	public static void HighlightHolder (GameObject holder){
		ChangeHolderColor (holder, 0.8f);
	}

	public static void RemoveHolderHighlight (GameObject holder){
		ChangeHolderColor (holder, 1.25f);
	}


	public static void ChangeHolderColor (GameObject holder, float factor){
		SpriteRenderer _renderer = holder.GetComponent<SpriteRenderer>();

		Color color = _renderer.color;
		color.g = color.g * factor;
		color.b = color.b * factor;

		_renderer.color = color;
	}

	public static IconHolderState GetItemHolder (GameObject icon){
		IconHolderState holderState = null;

		if (icon == holder1State.icon) {
			holderState =  holder1State;

		} else if (icon == holder2State.icon) {
			holderState =  holder2State;
		}

		return holderState;
	}

	public static GameObject GetIconByHolder (GameObject holder){
		GameObject icon = null;

		if (holder1State.holder == holder ) {
			icon =  holder1State.icon;

		} else if (holder2State.holder == holder ) {
			icon =  holder2State.icon;
		}

		return icon;
	}

	public static void RemoveHolderIcon (GameObject holder){
		if ( holder == holder1State.holder ) {
			holder1State.icon = null;
		} else if ( holder == holder2State.holder) {
			holder2State.icon = null;
		}
	}

	public void DropItem (){
		System.Random r = new System.Random();
		int rInt = r.Next(1, Constants.ITEM_JUJU); //for ints

		Vector2 newPos = GetRandomStagePosition ();

		GameObject newItem = (GameObject) Instantiate ( Resources.Load( GetPickupNameByID(rInt) ));
		newItem.transform.position = newPos;

		Communicator.Instance.ShareItemDrop (rInt, newPos);
	}

	Vector2 GetRandomStagePosition(){
		Vector2 newPos = Utilities.GetRandomStagePosition ();

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

	public static string GetItemNameByID (int ID){
		string controllerName = "items/";

		if (ID == 0) {
			controllerName += "barrel1";
		} else if (ID == 1) {
			controllerName += "spill";
		}

		return controllerName;
	}


}
