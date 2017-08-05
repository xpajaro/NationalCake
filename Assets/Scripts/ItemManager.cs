using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemManager : MonoBehaviour {

	Vector2 firstItemPosition = new Vector2 (-860, -440);
	Vector2 secondItemPosition = new Vector2 (-680, -440);
	Vector2 scale = new Vector2 (1.4f, 1.4f);

	public Canvas canvas;

	bool holder1Occupied, holder2Occupied;


	public static ItemManager Instance = null;    

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;

		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	public void SaveItem (Button itemToSave){
		if (!holder1Occupied) {
			CreateButton (itemToSave, firstItemPosition);
			holder1Occupied = true;
			
		} else if (!holder2Occupied) {
			CreateButton (itemToSave, secondItemPosition);
			holder2Occupied = true;
		}
	}

	void CreateButton(Button buttonPrefab, Vector2 spawnPosition){
		Button button = Object.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity) as Button;
		RectTransform rectTransform = button.GetComponent<RectTransform>();

		rectTransform.SetParent(canvas.transform);
		rectTransform.localScale = scale;
		rectTransform.localPosition = spawnPosition;
	}


}
