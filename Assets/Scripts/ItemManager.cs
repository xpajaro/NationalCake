using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

	public GameObject barrelPickupPrefab, spillPickupPrefab, jujuPickupPrefab, bombPickupPrefab;
	public GameObject barrelIconPrefab, spillIconPrefab, jujuIconPrefab, bombIconPrefab;

	public GameObject holder1, holder2;
	public Sprite activeHolderSprite, holderSprite;
	static public IconHolderState holder1State, holder2State;

	float DROP_ITEM_COOLDOWN = 10f; //change to 40

	// Use this for initialization
	void Start () {
		ItemUpdates.itemManager = this;
		PickupItem.itemManager = this;
		GameControls.itemManager = this;

		holder1State = new IconHolderState (holder1);
		holder2State = new IconHolderState (holder2);

		if (GameSetup.isHost) {
			InvokeRepeating ("DropItem", 0.0f, DROP_ITEM_COOLDOWN);
		}
	}


	public void SaveItem (int itemType){
		if (holder1State.icon == null) {
			holder1State.icon = SpawnNewItem (itemType, holder1State.holder.transform.position);
			
		} else if (holder2State.icon == null) {
			holder2State.icon = SpawnNewItem (itemType, holder2State.holder.transform.position);
		}
	}

	GameObject SpawnNewItem (int itemType, Vector2 position){
		GameObject itemIcon = (GameObject) Instantiate ( GetIconPrefabByID(itemType) );
		itemIcon.transform.position = position;

		return itemIcon;
	}

	public void HighlightHolder (GameObject holder){
		//ChangeHolderSprite (holder, activeHolderSprite);
	}

	public void RemoveHolderHighlight (GameObject holder){
		//ChangeHolderSprite (holder, holderSprite);
	}


	void ChangeHolderSprite (GameObject holder, Sprite holderStateSprite){
		SpriteRenderer _renderer = holder.GetComponent<SpriteRenderer>();
		_renderer.sprite = holderStateSprite;
	}

	public IconHolderState GetItemHolder (GameObject icon){
		IconHolderState holderState = null;

		if (icon == holder1State.icon) {
			holderState =  holder1State;

		} else if (icon == holder2State.icon) {
			holderState =  holder2State;
		}

		return holderState;
	}

	public GameObject GetIconByHolder (GameObject holder){
		GameObject icon = null;

		if (holder1State.holder == holder ) {
			icon =  holder1State.icon;

		} else if (holder2State.holder == holder ) {
			icon =  holder2State.icon;
		}

		return icon;
	}

	public void RemoveHolderIcon (GameObject holder){
		if ( holder == holder1State.holder ) {
			holder1State.icon = null;
		} else if ( holder == holder2State.holder) {
			holder2State.icon = null;
		}
	}

	public void DropItem (){
		System.Random r = new System.Random();
		int rInt = r.Next(0, 4);

		Vector2 newPos = GetRandomStagePosition ();

		GameObject newItem = (GameObject) Instantiate ( GetPickupPrefabByID(rInt) );
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

	public GameObject GetPickupPrefabByID (int ID){
		GameObject pickupPrefab = null ;

		if (ID == 0) {
			pickupPrefab = barrelPickupPrefab;
		} else if (ID == 1) {
			pickupPrefab = spillPickupPrefab;
		} else if (ID == 2) {
			pickupPrefab = jujuPickupPrefab;
		} else if (ID == 3) {
			pickupPrefab = bombPickupPrefab;
		} 

		return pickupPrefab;
	}

	GameObject GetIconPrefabByID (int ID){
		GameObject iconPrefab = null ;

		if (ID == 0) {
			iconPrefab = barrelIconPrefab;
		} else if (ID == 1) {
			iconPrefab = spillIconPrefab;
		} else if (ID == 2) {
			iconPrefab = jujuIconPrefab;
		} else if (ID == 3) {
			iconPrefab = bombIconPrefab;
		} 

		return iconPrefab;
	}


}
