using UnityEngine;
using System.Collections;

public class MovementHandler : MonoBehaviour 
{

	public static float MOVT_DAMPING = 0.5f;

	private static GameObject stage;

	static private Vector2 stageDimensions;
	static private Vector2 stagePadding;


	//-------------------------------------------
	// Setup
	//-------------------------------------------

	public static void LoadStage (GameObject _stage){
		stage = _stage;
	}


	//-------------------------------------------
	// Logic
	//-------------------------------------------


	public static void doFriction (Rigidbody2D rb) {
		if (isOnStage (rb.position)) {
			dampMovement (rb, MOVT_DAMPING);
		} 
	}

	static SpriteRenderer stageRenderer;
	static Texture2D texture ;
	static Color color ;
	public static bool isOnStage (Vector2 playerPosition){

		if (stage == null) {
			throw new UnityException("Stage object not loaded");
		}

		bool onStage = true;

		//get stage texture
		stageRenderer = stage.GetComponent<SpriteRenderer>();
		texture = stageRenderer.sprite.texture ;

		playerPosition = getLocalPlayerPosition (playerPosition, stageRenderer, texture);
		color = texture.GetPixel ((int)playerPosition.x, (int)playerPosition.y);

		if (color.a == 0){
			onStage = false;
		} 

		return onStage;
	}

	public static void dampMovement (Rigidbody2D rb, float damping){
		if (rb.velocity.x != 0 || rb.velocity.y != 0) {
			rb.velocity = rb.velocity * damping;
		} else if (rb.velocity.magnitude < 0.0001) {
			rb.velocity = new Vector2 (0, 0);
		}
	}




	//-------------------------------------------
	// Utilities
	//-------------------------------------------

	static Vector2 scaledPlayerPosition;
	static Vector2 getLocalPlayerPosition (Vector2 playerPosition, Renderer renderer, Texture2D texture){
		
		//convert unity world coordinates to original stage sprite coords
		stagePadding = getStagePadding (renderer.bounds.extents);

		if (stageDimensions == Vector2.zero) {
			stageDimensions = getStageDimensions (renderer.transform.position);
		}

		scaledPlayerPosition = getPositionInScale (playerPosition, texture);

		return  scaledPlayerPosition;

	}

	static Vector2 getStagePadding (Vector2 stageBounds){

		//(renderer.bounds.extents * -1.0f); //to get stage padding measurement
		//this is distance from left of screen and bottom of screen
		return Camera.main.WorldToScreenPoint (stageBounds * -1.0f); 
	}


	static Vector2 getStageDimensions (Vector2 stagePosition){
		Vector2 tempStageDimensions = new Vector2 ();
		Vector2 stagePositionInPixels = Camera.main.WorldToScreenPoint (stagePosition);

		//stage dimensions in screenPoint
		tempStageDimensions.x = (stagePositionInPixels.x - stagePadding.x) *2;
		tempStageDimensions.y = (stagePositionInPixels.y - stagePadding.y) *2;

		return tempStageDimensions;
	}


	static Vector2 positionInScale = new Vector2 ();
	static Vector2 getPositionInScale(Vector2 position, Texture2D stageTexture){

		position = Camera.main.WorldToScreenPoint (position);

		//convert from position on screen to position on stage
		// use stage bottom left as (0,0) origin for object
		position.x = position.x - stagePadding.x;
		position.y = position.y - stagePadding.y;

		//pixels on unity have diff resolutions than sprite pixels
		positionInScale.x = (int) (position.x/stageDimensions.x * stageTexture.width);
		positionInScale.y = (int) (position.y/stageDimensions.y * stageTexture.height);

		return positionInScale;
	}


}

