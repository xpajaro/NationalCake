using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class ItemManager : NetworkBehaviour { 

	public Canvas canvas;
	public AudioClip powerUpSound;
	public Button[] storageItems;

	Vector2[] itemPositions;
	Vector2 scale = new Vector2 (1.4f, 1.4f);

	const int FREE_SLOT = -1;
	const int ALL_SLOTS_OCCUPIED = -2;
	const int INVALID_SLOT = -3;

	int[] playerSlots, enemySlots;


	public static ItemManager Instance = null;    

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;

		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	void Start(){
		playerSlots = new int[]{-1, -1};
		enemySlots = new int[]{-1, -1};

		itemPositions = new Vector2[2];
		itemPositions[0] = new Vector2 (-860.0f, -440.0f);
		itemPositions[1] = new Vector2 (-680.0f, -440.0f);
	}

	public void SaveItem (int itemID, bool isServerPlayer){
		int freeSlotIndex;

		if (isServerPlayer){ 
			freeSlotIndex = OccupyFreeSlot (playerSlots, itemID);
		} else {
			freeSlotIndex = OccupyFreeSlot (enemySlots, itemID);
		}

		if (freeSlotIndex != ALL_SLOTS_OCCUPIED) {
			RpcSaveToScreen (itemID, isServerPlayer, freeSlotIndex);

		} 
	}

	[ClientRpc]
	void RpcSaveToScreen (int itemID, bool isServerPlayer, int positionIndex){
		Vector2 newPos = itemPositions [positionIndex];

		if (isServer && isServerPlayer) {
			CreateButton (storageItems[itemID], newPos);

		} else if (!isServer && !isServerPlayer) {
			CreateButton (storageItems[itemID], newPos);
		}
	}


	int OccupyFreeSlot(int[] itemSlots, int itemID){
		int position = ALL_SLOTS_OCCUPIED;

		if (itemSlots [0] == FREE_SLOT) {
			position = 0;
			itemSlots [position] = itemID;

		} else if (itemSlots [1] == FREE_SLOT) {
			position = 1;
			itemSlots [position] = itemID;
		}

		return position;
	}

	public void UpdateUsedItems (bool isServerPlayer, Vector2 position){

		int itemIndex = GetSlotIndexByPosition (position);

		if (itemIndex == INVALID_SLOT) {
			return;
		}

		if (isServerPlayer) {
			playerSlots [itemIndex] = FREE_SLOT;

		} else {
			enemySlots [itemIndex] = FREE_SLOT;
		}
	}

	int GetSlotIndexByPosition(Vector2 positionToCompare) {
		int itemPosition = INVALID_SLOT;

		if (Vector2.Distance (itemPositions [0], positionToCompare) < 0.01) {
			itemPosition = 0;

		} else if (Vector2.Distance (itemPositions [1], positionToCompare) < 0.01){
			itemPosition = 1;
		}

		return itemPosition;
	}

	Button CreateButton(Button buttonPrefab, Vector2 spawnPosition){
		SoundManager.Instance.PlaySingle (powerUpSound);

		Button button = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as Button;
		RectTransform rectTransform = button.GetComponent<RectTransform>();

		rectTransform.SetParent(canvas.transform);
		rectTransform.localScale = scale;
		rectTransform.localPosition = spawnPosition;

		return button;
	}


}
