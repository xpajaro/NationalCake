using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorting : MonoBehaviour {

	public bool isMovingObject;

	SpriteRenderer spriteRenderer;

	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		Sort ();
	}

	void Update(){
		if (isMovingObject) {
			Sort ();
		}
	}

	void Sort(){
		int layer = Mathf.RoundToInt(transform.position.y * 100f) * -1;
		spriteRenderer.sortingOrder = layer;
	}

}
