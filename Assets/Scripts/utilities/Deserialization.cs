﻿using UnityEngine;
using System.Collections;

public class Deserialization {



	static Vector3 impulse = new Vector3 ();
	public static Vector3 GetImpulse (byte[] dataFields){
		Debug.Log ("Get Impulse ");
		impulse.x = System.BitConverter.ToSingle(dataFields, 2);
		impulse.y = System.BitConverter.ToSingle(dataFields, 6);

		Debug.Log ("Get Impulse done");
		return impulse;
	}


	public static ActorState GetState (byte[] dataFields){
		ActorState state = new ActorState ();
		Debug.Log ("get state");
		Vector3 val = new Vector3 ();

		state.StateNumber = System.BitConverter.ToInt32(dataFields, 2);

		val.x = System.BitConverter.ToSingle(dataFields, 6);
		val.y = System.BitConverter.ToSingle(dataFields, 10);
		state.PlayerPosition = new Vector3(val.x, val.y, 0); 

		val.x = System.BitConverter.ToSingle(dataFields, 14);
		val.y = System.BitConverter.ToSingle(dataFields, 18);
		state.PlayerVelocity = new Vector3(val.x, val.y, 0);

		val.x = System.BitConverter.ToSingle(dataFields, 22);
		val.y = System.BitConverter.ToSingle(dataFields, 26);
		state.EnemyPosition = new Vector3(val.x, val.y, 0); 

		val.x = System.BitConverter.ToSingle(dataFields, 30);
		val.y = System.BitConverter.ToSingle(dataFields, 34);
		state.EnemyVelocity = new Vector3(val.x, val.y, 0);

		val.x = System.BitConverter.ToSingle(dataFields, 38);
		val.y = System.BitConverter.ToSingle(dataFields, 42);
		state.EnemyVelocity = new Vector3(val.x, val.y, 0);


		Debug.Log ("player deets " + state.PlayerPosition.ToString("G4") + " / " +  state.PlayerVelocity.ToString("G4") + " \n " +
		"enemy deets " + state.PlayerPosition.ToString("G4") + " / " +  state.EnemyVelocity.ToString("G4"));

		Debug.Log ("get state done");
		return state;
	}

	public static char GetMessageType(byte[] data){
		Debug.Log ("get message type done");
		return (char) data[1];;
	}



}