using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

	Texture2D stageTexture;
	WorldConverter worldConverter;

	public static Stage Instance = null;    

	void Awake ()
	{
		if (Instance == null) {
			Instance = this;

		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		worldConverter = new WorldConverter (renderer);

		stageTexture = GetComponent<SpriteRenderer> ().sprite.texture;
	}

	public bool IsOnStage (Vector2 objPosition){
		bool onStage = true;
		objPosition = worldConverter.GetPositionInWorld (objPosition);

		if (GetAlphaAtPosition (objPosition) == 0){
			onStage = false;
		} 

		return onStage;
	}

	public Vector2 GetRandomStagePosition() {
		Vector2 pos = GetRandomStageCoordinates ();

		while (!IsOnStage (pos)) {
			pos = GetRandomStageCoordinates ();
		}

		return pos;
	} 


	//-------------------------------------------
	// utilites
	//-------------------------------------------


	Vector2 GetRandomStageCoordinates () {

		System.Random r = new System.Random();

		float randomX = (float)( r.NextDouble() * 9f) - 4.5f;
		float randomY = (float)( r.NextDouble() * 5.5f) - 2.25f;

		return new Vector2 (randomX, randomY);
	}


	float GetAlphaAtPosition (Vector2 position){
		Color color = stageTexture.GetPixel ((int)position.x, (int)position.y);
		return color.a;
	}
}
