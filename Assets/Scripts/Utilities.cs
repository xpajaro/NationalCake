﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Utilities : MonoBehaviour{

	static BinaryFormatter binFormatter = new BinaryFormatter ();
	static MemoryStream mStream = new MemoryStream ();

	//-------------------------------------------
	// Serialization
	//-------------------------------------------


	public static byte[] Serialize (object obj){
		binFormatter.Serialize (mStream, obj);
		return mStream.ToArray ();
	}

	public static Dictionary<string, object> Deserialize (byte[] input){	
		mStream.Write (input, 0, input.Length);
		mStream.Position = 0;

		Dictionary<string, object> data = binFormatter.Deserialize(mStream) as Dictionary<string, object> ;
		return data;
	}
}

