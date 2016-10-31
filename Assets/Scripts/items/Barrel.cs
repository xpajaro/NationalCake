using UnityEngine;
using System.Collections;

public class Barrel : MonoBehaviour {
	
	public Sprite CRACKED_BARREL , BROKEN_BARREL ;
	int noOfHits = 0;

	SpriteRenderer spriteRenderer;

	void Start (){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter2D (Collision2D col){	
		CrushBarrel ();
		noOfHits++;
	}

	void CrushBarrel (){
		
		switch (noOfHits) {
			case 0:
				spriteRenderer.sprite = CRACKED_BARREL;
				break;
			case 1:
				spriteRenderer.sprite = BROKEN_BARREL;
				break;
			case 2:
				ExpireItem ();
				break;
			default:
				break;
		}

	}

	void ExpireItem (){
		Destroy (this.gameObject);
	}
}
