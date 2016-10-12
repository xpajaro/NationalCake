using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Serialization
{
	static byte PROTOCOL_VERSION = 1;

	//byte message lengths
	static int HELLO_MESSAGE_LENGTH = 2;
	static int MOVEMENT_MESSAGE_LENGTH = 10;
	static int STATE_MESSAGE_LENGTH = 46;

	//order of message sent (ignore if expired)
	static int stateMessageNo = 0 ;

	static List<byte> helloMessage  = new List<byte> (HELLO_MESSAGE_LENGTH);
	static List<byte> movementMessage  = new List<byte> (MOVEMENT_MESSAGE_LENGTH);
	static List<byte> stateMessage  = new List<byte> (STATE_MESSAGE_LENGTH);

	//-------------------------------------------
	// Serialize
	//-------------------------------------------


	public static byte[] SerializeHello (){
		Debug.Log ("Serialize hello done");
		helloMessage.Clear ();
		helloMessage.Add (PROTOCOL_VERSION);
		helloMessage.Add ((byte)Communicator.MESSAGE_TYPE_HELLO);
		return helloMessage.ToArray ();
	}
		

	public static byte [] SerializeMovement (Vector3 impulse){
		Debug.Log ("Serialize movement");

		movementMessage.Clear ();
		//meta
		movementMessage.Add (PROTOCOL_VERSION);
		movementMessage.Add ((byte)Communicator.MESSAGE_TYPE_MOVEMENT);
		//data
		movementMessage.AddRange (System.BitConverter.GetBytes (impulse.x));  
		movementMessage.AddRange (System.BitConverter.GetBytes (impulse.y)); 

		Debug.Log ("Serialize movement done");
		return movementMessage.ToArray ();
	}


	public static byte [] SerializeState (Rigidbody2D playerBody, Rigidbody2D enemyBody, Rigidbody2D cakeBody){
		Debug.Log ("Serialize state");

		stateMessage.Clear ();
		//meta
		stateMessage.Add (PROTOCOL_VERSION);
		stateMessage.Add ((byte)Communicator.MESSAGE_TYPE_STATE);
		//data
		stateMessage.AddRange (BitConverter.GetBytes (++stateMessageNo)); 
		stateMessage.AddRange (BitConverter.GetBytes (playerBody.position.x));  
		stateMessage.AddRange (BitConverter.GetBytes (playerBody.position.y)); 
		stateMessage.AddRange (BitConverter.GetBytes (playerBody.velocity.x));  
		stateMessage.AddRange (BitConverter.GetBytes (playerBody.velocity.y)); 
		stateMessage.AddRange (BitConverter.GetBytes (enemyBody.position.x));  
		stateMessage.AddRange (BitConverter.GetBytes (enemyBody.position.y)); 
		stateMessage.AddRange (BitConverter.GetBytes (enemyBody.velocity.x));  
		stateMessage.AddRange (BitConverter.GetBytes (enemyBody.velocity.y));
		stateMessage.AddRange (BitConverter.GetBytes (cakeBody.position.x));  
		stateMessage.AddRange (BitConverter.GetBytes (cakeBody.position.y)); 

		Debug.Log ("player deets " + playerBody.position.ToString("G4") + " / " +  playerBody.velocity.ToString("G4") + " \n " +
		"enemy deets " + enemyBody.position.ToString("G4") + " / " +  enemyBody.velocity.ToString("G4"));

		Debug.Log ("Serialize state done");
		return stateMessage.ToArray ();
	}


}


