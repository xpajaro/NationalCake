using UnityEngine;
using System.Collections;

public class ItemManager : MonoBehaviour {

	string WINE_PREFAB = "wine";
	Vector3[] wineLocations;

	// Use this for initialization
	void Start () {
		LoadWines ();
	}

	void LoadWines (){
		LoadWineLocations ();

		for (int i = 0; i < wineLocations.Length; i++) {
			LoadWine (i);
		}
	}

	void LoadWineLocations (){
		wineLocations = new Vector3[4];
		wineLocations [0] = new Vector3 (1, -1.5f);
		wineLocations [1] = new Vector3 (1, 1.5f);
		wineLocations [2] = new Vector3 (1, -1.5f);
		wineLocations [3] = new Vector3 (-1, -1.5f);
	}


	void LoadWine (int i){
		Instantiate (Resources.Load (WINE_PREFAB), wineLocations [i], Quaternion.identity);
	}

}
