using UnityEngine;
using System.Collections.Generic;

public class Bloc : MonoBehaviour {
	
	public Sprite CRACKED_BARREL , BROKEN_BARREL ;
	public AudioClip barrelHit, barrelBroken;

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
				SoundManager.Instance.PlaySingle (barrelHit);
				break;
			case 1:
				spriteRenderer.sprite = BROKEN_BARREL;
				SoundManager.Instance.PlaySingle (barrelHit);
				break;
			case 2:
				SoundManager.Instance.PlaySingle (barrelBroken);
				Destroy (gameObject);
				break;
			default:
				break;
		}

	}

}
