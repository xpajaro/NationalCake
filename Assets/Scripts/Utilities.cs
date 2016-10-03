using System;
using UnityEngine;
using System.Collections;
using System.Text;

public class Utilities : MonoBehaviour{
		
	public static string SerializeVector3 (Vector3 v)
	{
		StringBuilder sb = new StringBuilder();
		sb.Append(v.x).Append(" ").Append(v.y).Append(" ").Append(v.z) ;

		return sb.ToString();
	}

	public static Vector3 DeserializeVector3 (string v)
	{
		string[] values = v.Split(' ');
		Vector3 result = new Vector3(float.Parse(values[0]), 
			float.Parse(values[1]), 
			float.Parse(values[2]));

		return result;
	}
}

