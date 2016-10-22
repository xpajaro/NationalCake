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

	float COOL_DOWN = 25f, DURATION = 10f;

	void OnCollisionEnter2D (Collision2D col)
	{	
		if (!swapped) {
			string actorName = col.gameObject.name;
			if (actorName == "player" || actorName == "enemy") {

				SwapSides ();
				Darken ();
				Presenter.StopCollisions (this.gameObject);

				Invoke ("SwapSides", DURATION);
				Invoke ("Revive", COOL_DOWN);
			}
		}

	}

	void Revive (){
		Presenter.StartCollisions (this.gameObject);
		Brighten ();
	}

	//handle moving after swap
	void SwapSides (){
		Vector3 tempPosition = pGoal.transform.position;
		pGoal.transform.position = eGoal.transform.position;
		eGoal.transform.position = tempPosition;
		swapped = !swapped;
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
