using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Utilities : MonoBehaviour{


	public static void FaceCorrectDirection (GameObject actor, Vector3 impulse, ref bool facingHomeBase, bool leftIsHome){

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
		Vector3 theScale = actor.transform.localScale;
		theScale.x *= -1;
		actor.transform.localScale = theScale;
	}


	public static Vector3 GetRandomStagePosition() {
		Vector3 pos = GetRandomCoordinates ();

		while (!StageManager.isOnStage (pos)) {
			pos = GetRandomCoordinates ();
		}

		return pos;
	} 

	static Vector3 GetRandomCoordinates () {

		System.Random r = new System.Random();

		float randomX = (float)( r.NextDouble() * 10) - 5;
		float randomY = (float)( r.NextDouble() * 6) - 3;

		return new Vector3 (randomX, randomY, 0f);
	}


	public static float Interpolate (GameObject actor, Vector3 start, Vector3 destination, float  pctDone){
		if (pctDone <= 1.0) {
			actor.transform.position = Vector3.Lerp (start, destination, pctDone);
		}
		return pctDone;
	}


	public static float GetAlphaAtPosition (Vector2 position, Texture2D texture){
		Color color = texture.GetPixel ((int)position.x, (int)position.y);
		return color.a;
	}


	public static Vector3 FlipSide (Vector3 input){
		input.x *= -1;
		return input;
	}


	public static StringBuilder ClearStringBuilder ( StringBuilder value)
	{
		value.Length = 0;
		value.Capacity = 0;

		return value;
	}

	public static string Vector3ToString (Vector3 v){
		//Debug.Log ("vector3 to string");
		StringBuilder sb = new StringBuilder();
		sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z) ;

		//Debug.Log ("vector3 to string done");
		return sb.ToString();
	}

	public static void StringToVector3 (string v, ref Vector3 vectorHolder){
		//Debug.Log ("string to vector3");
		string[] values = v.Split(' ');

		vectorHolder.x = float.Parse(values[0]); 
		vectorHolder.y = float.Parse(values[1]);
		vectorHolder.z = float.Parse(values[2]);

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

