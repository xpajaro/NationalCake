using UnityEngine;
using System.Collections;

public class Gong : MonoBehaviour {

	public GameObject pGoal, eGoal;

	public static bool swapped;

	SpriteRenderer spriteRenderer;
	float blueColor, greenColor;


	void Start(){
		spriteRenderer = GetComponent<SpriteRenderer> ();
		blueColor = spriteRenderer.color.b;
		greenColor = spriteRenderer.color.g;
	}

	float COOL_DOWN = 10f ;

	void OnCollisionEnter2D (Collision2D col)
	{	
		if (!swapped) {
			string actorName = col.gameObject.name;
			if (actorName.Equals("player") || actorName.Equals( "enemy")) {

				SwapSides ();
				swapped = true;

				Darken ();

				Invoke ("Revive", COOL_DOWN);
			}
		}

	}

	void Revive (){
		SwapSides ();
		Brighten ();
	}

	//handle moving after swap
	void SwapSides (){
		if (!GameState.gameEnded) {
			Vector2 tempPosition = pGoal.transform.position;
			pGoal.transform.position = eGoal.transform.position;
			eGoal.transform.position = tempPosition;
			swapped = false;
		}
	}

	void Darken (){
		Color c = spriteRenderer.color;
		c.b = 0;
		c.g = 0;
		spriteRenderer.color = c;
	}

	void Brighten (){
		Color c = spriteRenderer.color;
		c.b = blueColor;
		c.g = greenColor;
		spriteRenderer.color = c;
	}
}
