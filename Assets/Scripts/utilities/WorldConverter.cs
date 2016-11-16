using System;
using UnityEngine;


//where world is a texture or sprite 
//we need to get the local (x,y) conversion of a position that's in unity world coordinates

public class WorldConverter
{

	//world stuff
	Vector2 dimensions, padding;
	SpriteRenderer renderer;
	Texture2D texture ;

	public WorldConverter (GameObject world)
	{
		renderer = world.GetComponent<SpriteRenderer>();
		texture = renderer.sprite.texture ;

		//always calculate padding before dimensions
		padding = getPadding (renderer.bounds.extents);
		dimensions = getDimensions (renderer.transform.position);
	}

	public Vector2 getPositionInWorld (Vector2 rawPosition){
		Vector2 scaledPosition = getScaledPosition (rawPosition);
		return  scaledPosition;
	}

	Vector2 getPadding (Vector2 stageBounds){

		//(renderer.bounds.extents * -1.0f); //to get stage padding measurement
		//this is distance from left of screen and bottom of screen
		return Camera.main.WorldToScreenPoint (stageBounds * -1.0f); 
	}


	Vector2 getDimensions (Vector2 stagePosition){
		Vector2 stageDimensions = new Vector2 ();
		Vector2 stagePositionInPixels = Camera.main.WorldToScreenPoint (stagePosition);

		//stage dimensions in screenPoint
		stageDimensions.x = (stagePositionInPixels.x - padding.x) *2;
		stageDimensions.y = (stagePositionInPixels.y - padding.y) *2;

		return stageDimensions;
	}


	Vector2 getScaledPosition (Vector2 position){
		Vector2 positionInScale = new Vector2 ();
		position = Camera.main.WorldToScreenPoint (position);

		//convert from position on screen to position on stage
		// use stage bottom left as (0,0) origin for object
		position.x = position.x - padding.x;
		position.y = position.y - padding.y;

		//pixels on unity have diff resolutions than sprite pixels
		positionInScale.x = (int) (position.x/dimensions.x * texture.width);
		positionInScale.y = (int) (position.y/dimensions.y * texture.height);

		return positionInScale;
	}

}


