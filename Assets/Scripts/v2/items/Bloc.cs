using UnityEngine;
using System.Collections.Generic;

public class Bloc : MonoBehaviour {
	
	public Sprite CRACKED_BARREL , BROKEN_BARREL ;

	int noOfHits = 0;
	SpriteRenderer spriteRenderer;

	void Start (){
		spriteRenderer = GetComponent<SpriteRenderer> ();
	}

	void OnCollisionEnter2D (Collision2D col){	
		CrushBarrel (noOfHits);
		noOfHits++;
	}

	void CrushBarrel (int hitCount){


		switch (hitCount) {
			case 0:
				spriteRenderer.sprite = CRACKED_BARREL;
				SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.BLOC_HIT);
				break;
			case 1:
				spriteRenderer.sprite = BROKEN_BARREL;
				SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.BLOC_HIT);
				break;
			case 2:
				SoundPlayer.Instance.Play (SoundPlayer.SOUNDS.BLOC_BROKEN);
				Destroy (gameObject);
				break;
			default:
				break;
		}

	}

}
