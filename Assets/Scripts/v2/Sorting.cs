using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting : MonoBehaviour {

	void Update(){
		int layer = Mathf.RoundToInt(gameObject.transform.position.y * 100f) * -1;
		gameObject.GetComponent<SpriteRenderer> ().sortingOrder = layer;
	}

}
