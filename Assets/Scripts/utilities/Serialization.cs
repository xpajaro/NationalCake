﻿using System;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

public class Serialization
{
	static byte PROTOCOL_VERSION = 1;

	//byte message lengths
	static int HELLO_MESSAGE_LENGTH = 2;
	static int MOVEMENT_MESSAGE_LENGTH = 10;
	static int WINE_STATE_MESSAGE_LENGTH = 10;
	static int ACTOR_STATE_MESSAGE_LENGTH = 49;

	//order of message sent (ignore if expired)
	static int stateMessageNo = 0 ;

	static List<byte> helloMessage  = new List<byte> (HELLO_MESSAGE_LENGTH);
	static List<byte> movementMessage  = new List<byte> (MOVEMENT_MESSAGE_LENGTH);
	static List<byte> wineStateMessage  = new List<byte> (WINE_STATE_MESSAGE_LENGTH);
	static List<byte> actorStateMessage  = new List<byte> (ACTOR_STATE_MESSAGE_LENGTH);

	//-------------------------------------------
	// Serialize
	//-------------------------------------------


	public static byte[] SerializeHello (){
		//Debug.Log ("Serialize hello done");
		helloMessage.Clear ();
		helloMessage.Add (PROTOCOL_VERSION);
		helloMessage.Add ((byte)Communicator.MESSAGE_TYPE_HELLO);
		return helloMessage.ToArray ();
	}
		

	public static byte [] SerializeMovement (Vector3 impulse){
		//Debug.Log ("Serialize movement");

		movementMessage.Clear ();
		//meta
		movementMessage.Add (PROTOCOL_VERSION);
		movementMessage.Add ((byte)Communicator.MESSAGE_TYPE_MOVEMENT);
		//data
		movementMessage.AddRange (System.BitConverter.GetBytes (impulse.x));  
		movementMessage.AddRange (System.BitConverter.GetBytes (impulse.y)); 

		//Debug.Log ("Serialize movement done");
		return movementMessage.ToArray ();
	}


	public static byte [] SerializeWineState (int tagNo, int action){
		//Debug.Log ("Serialize wine state");

		wineStateMessage.Clear ();
		//meta
		wineStateMessage.Add (PROTOCOL_VERSION);
		wineStateMessage.Add ((byte)Communicator.MESSAGE_TYPE_WINE);
		//data
		wineStateMessage.AddRange (System.BitConverter.GetBytes (tagNo)); 
		wineStateMessage.AddRange (System.BitConverter.GetBytes (action)); 

		//Debug.Log ("Serialize wine state done");
		return wineStateMessage.ToArray ();
	}


	public static byte [] SerializeActorState (Rigidbody2D playerBody, bool pFalling,
		Rigidbody2D enemyBody, bool eFalling,
		Rigidbody2D cakeBody, bool cFalling){
		//Debug.Log ("Serialize state");

		actorStateMessage.Clear ();
		//meta
		actorStateMessage.Add (PROTOCOL_VERSION);
		actorStateMessage.Add ((byte)Communicator.MESSAGE_TYPE_STATE);
		//data
		actorStateMessage.AddRange (BitConverter.GetBytes (++stateMessageNo)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (playerBody.position.x));  
		actorStateMessage.AddRange (BitConverter.GetBytes (playerBody.position.y)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (playerBody.velocity.x));  
		actorStateMessage.AddRange (BitConverter.GetBytes (playerBody.velocity.y)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (enemyBody.position.x));  
		actorStateMessage.AddRange (BitConverter.GetBytes (enemyBody.position.y)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (enemyBody.velocity.x));  
		actorStateMessage.AddRange (BitConverter.GetBytes (enemyBody.velocity.y));
		actorStateMessage.AddRange (BitConverter.GetBytes (cakeBody.position.x));  
		actorStateMessage.AddRange (BitConverter.GetBytes (cakeBody.position.y)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (pFalling)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (eFalling)); 
		actorStateMessage.AddRange (BitConverter.GetBytes (cFalling)); 

		Debug.Log ("player deets " + playerBody.position.ToString("G4") + " / " +  playerBody.velocity.ToString("G4") + " \n " +
			"enemy deets " + enemyBody.position.ToString("G4") + " / " +  enemyBody.velocity.ToString("G4") + " \n " +
			"cake deets " + cakeBody.position.ToString("G4") +  " \n " +
			"falling deets " + pFalling.ToString() + " / " + cFalling.ToString() + " / " + eFalling.ToString());

		//Debug.Log ("Serialize state done");
		return actorStateMessage.ToArray ();
	}


}


