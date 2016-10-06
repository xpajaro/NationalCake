using System;
using System.Text;
using UnityEngine;

public class Serialization
{
	static string FIELD_DIVIDER = ">";


	//-------------------------------------------
	// Serialize
	//-------------------------------------------

	public static byte[] SerializeHello (){
		Debug.Log ("Serialize hello done");
		return Encoding.ASCII.GetBytes (Communicator.MessageTypes.HELLO.ToString());
	}


	static StringBuilder sbMovement = new StringBuilder ();
	public static byte [] SerializeMovement (Vector3 impulse){
		Debug.Log ("Serialize movement");
		Utilities.ClearStringBuilder (sbMovement);

		sbMovement.Append (Communicator.MessageTypes.MOVEMENT.ToString());
		sbMovement.Append (FIELD_DIVIDER);
		sbMovement.Append (Utilities.Vector3ToString (impulse));

		Debug.Log ("Serialize movement done");
		return Encoding.ASCII.GetBytes (sbMovement.ToString ());
	}


	static StringBuilder sbState = new StringBuilder ();
	public static byte [] SerializeState (Rigidbody2D playerBody, Rigidbody2D enemyBody, Rigidbody2D cakeBody){
		Debug.Log ("Serialize state");
		Utilities.ClearStringBuilder (sbState);

		sbState.Append (Communicator.MessageTypes.STATE.ToString());
		sbState.Append (FIELD_DIVIDER);
		sbState.Append (Utilities.Vector3ToString (playerBody.position));
		sbState.Append (FIELD_DIVIDER);
		sbState.Append (Utilities.Vector3ToString (playerBody.velocity));
		sbState.Append (FIELD_DIVIDER);
		sbState.Append (Utilities.Vector3ToString (enemyBody.position));
		sbState.Append (FIELD_DIVIDER);
		sbState.Append (Utilities.Vector3ToString (enemyBody.velocity));
		sbState.Append (FIELD_DIVIDER);
		sbState.Append (Utilities.Vector3ToString (cakeBody.position));

		Debug.Log ("Serialize state done");
		return Encoding.ASCII.GetBytes (sbState.ToString ());
	}


	//-------------------------------------------
	// Deserialize
	//-------------------------------------------

	public static string Deserialize (byte[] rawData){
		Debug.Log ("Deserialize done");
		return System.Text.Encoding.Default.GetString (rawData);
	}

	public static string[] GetDataFields(String deserializedData){
		Debug.Log ("Get data fields done");
		return deserializedData.Split (new string[]{ FIELD_DIVIDER }, StringSplitOptions.None);
	}

	static Vector3 impulse = new Vector3 ();
	public static Vector3 GetImpulse (string[] dataFields){
		Debug.Log ("Get Impulse done");
		Utilities.StringToVector3 (dataFields [1], ref impulse);
		return impulse;
	}

	public static ActorState GetState (string[] dataFields){
		ActorState state = new ActorState ();
		Debug.Log ("get state");
		Vector3 valueHolder = new Vector3 ();

		Utilities.StringToVector3 (dataFields [1], ref valueHolder);
		state.PlayerPosition = valueHolder; 

		Utilities.StringToVector3 (dataFields [2], ref valueHolder);
		state.PlayerVelocity = valueHolder;

		Utilities.StringToVector3 (dataFields [3], ref valueHolder);
		state.EnemyPosition = valueHolder;

		Utilities.StringToVector3 (dataFields [4], ref valueHolder);
		state.EnemyVelocity = valueHolder;

		Utilities.StringToVector3 (dataFields [5], ref valueHolder);
		state.CakePosition = valueHolder;

		Debug.Log ("position player " + state.PlayerPosition.ToString("G4"));
		Debug.Log ("position enemy " +  state.EnemyPosition.ToString("G4"));

		Debug.Log ("get state done");
		return state;
	}

	public static Communicator.MessageTypes GetMessageType(string[] dataFields){
		Debug.Log ("get message type");
		Communicator.MessageTypes msgType = (Communicator.MessageTypes) 
											Enum.Parse(typeof(Communicator.MessageTypes), dataFields[0]);

		Debug.Log ("get message type done");
		return msgType;
	}



}


