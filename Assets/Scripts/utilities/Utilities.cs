using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Utilities : MonoBehaviour{


	static float OVERLAP_RADIUS = 1.2f;
	public static GameObject GetOverLappingItem (Vector2 pos, int layerMask){
		GameObject collisionObject = null;

		Collider2D collider = Physics2D.OverlapCircle (pos, OVERLAP_RADIUS, layerMask);
		if (collider != null) {
			collisionObject = collider.gameObject;
		}

		return collisionObject;
	}

	public static void FaceCorrectDirection (GameObject actor, Vector2 impulse, ref bool facingHomeBase, bool leftIsHome){

		if (leftIsHome) {
			TurnByImpulse (actor, impulse.x, ref facingHomeBase);
		} else {
			TurnByImpulse (actor, (impulse.x * -1), ref facingHomeBase);
		}
	}

	static void TurnByImpulse (GameObject actor, float direction, ref bool facingHomeBase){
		if (direction > 0 && facingHomeBase) {
			Utilities.TurnAround (actor);
			facingHomeBase = false;

		} else if (direction < 0 && !facingHomeBase) {
			Utilities.TurnAround (actor);
			facingHomeBase = true;
		}
	}

	public static void TurnAround (GameObject actor){
		Vector2 theScale = actor.transform.localScale;
		theScale.x *= -1;
		actor.transform.localScale = theScale;
	}


	public static Vector2 GetRandomStagePosition() {
		Vector2 pos = GetRandomCoordinates ();

		while (!StageManager.isOnStage (pos)) {
			pos = GetRandomCoordinates ();
		}

		return pos;
	} 

	static Vector2 GetRandomCoordinates () {

		System.Random r = new System.Random();

		float randomX = (float)( r.NextDouble() * 9f) - 4.5f;
		float randomY = (float)( r.NextDouble() * 5.5f) - 2.25f;

		return new Vector2 (randomX, randomY);
	}


	public static float Interpolate (GameObject actor, Vector2 start, Vector2 destination, float  pctDone){
		if (pctDone <= 1.0) {
			actor.transform.position = Vector2.Lerp (start, destination, pctDone);
		}
		return pctDone;
	}


	public static float GetAlphaAtPosition (Vector2 position, Texture2D texture){
		Color color = texture.GetPixel ((int)position.x, (int)position.y);
		return color.a;
	}


	public static Vector2 FlipSide (Vector2 input){
		input.x *= -1;
		return input;
	}


	public static StringBuilder ClearStringBuilder ( StringBuilder value)
	{
		value.Length = 0;
		value.Capacity = 0;

		return value;
	}

	public static string Vector2ToString (Vector2 v){
		//Debug.Log ("vector3 to string");
		StringBuilder sb = new StringBuilder();
		sb.Append(v.x).Append(" ").Append(v.y) ;

		//Debug.Log ("vector3 to string done");
		return sb.ToString();
	}

	public static void StringToVector2 (string v, ref Vector2 vectorHolder){
		//Debug.Log ("string to vector3");
		string[] values = v.Split(' ');

		vectorHolder.x = float.Parse(values[0]); 
		vectorHolder.y = float.Parse(values[1]);

		//Debug.Log ("string to vector3 done");
	}

	public static string PrintBytes( byte[] byteArray)
	{
		//Debug.Log ("print bytes");
		var sb = new StringBuilder("new byte[] { ");
		for(var i = 0; i < byteArray.Length;i++)
		{
			var b = byteArray[i];
			sb.Append(b);
			if (i < byteArray.Length -1)
			{
				sb.Append(", ");
			}
		}
		sb.Append(" }");

		//Debug.Log ("print bytes done");
		return sb.ToString();
	}

}

