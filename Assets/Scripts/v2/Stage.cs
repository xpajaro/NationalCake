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

	public bool IsOnStage (Vector2 gameObjectPosition){
		bool onStage = true;
		gameObjectPosition = worldConverter.GetPositionInWorld (gameObjectPosition);

		if (Utilities.GetAlphaAtPosition (gameObjectPosition, stageTexture) == 0){
			onStage = false;
		} 

		return onStage;
	}

}
