using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {
	public GameObject holder1, holder2;

	static Vector3 holder1Pos, holder2Pos;
	static bool holder1Occupied = false, holder2Occupied = false; //change to true once we test use

	float DROP_ITEM_COOLDOWN = 40f; //change to 40

	// Use this for initialization
	void Start () {
		holder1Pos = holder1.transform.position;
		holder2Pos = holder2.transform.position;

		if (GameSetup.isHost) {
			InvokeRepeating ("DropItem", 0.0f, DROP_ITEM_COOLDOWN);
		}
	}


	public static void SaveItem (int itemType){
		if (!holder1Occupied) {
			SpawnNewItem (itemType, holder1Pos);
			holder1Occupied = true;
			
		} else if (!holder2Occupied) {
			SpawnNewItem (itemType, holder2Pos);
			holder2Occupied = true;
		}
	}

	static void SpawnNewItem (int itemType, Vector3 position){
		GameObject itemIcon = (GameObject) Instantiate ( Resources.Load( GetIconNameByID(itemType) ));
		itemIcon.transform.position = position;
	}

	public void DropItem (){
		System.Random r = new System.Random();
		int rInt = r.Next(0, 3); //for ints

		Vector3 newPos = Utilities.GetRandomStagePosition ();

		GameObject newItem = (GameObject) Instantiate ( Resources.Load( GetPickupNameByID(rInt) ));
		newItem.transform.position = newPos;

		Communicator.Instance.ShareItemDrop (rInt, newPos);
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
