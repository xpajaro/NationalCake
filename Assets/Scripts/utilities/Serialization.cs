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
	static int ITEM_DROP_MESSAGE_LENGTH = 14;
	static int ITEM_USE_MESSAGE_LENGTH = 14;
	static int ACTOR_STATE_MESSAGE_LENGTH = 49;
	static int GAME_STATE_MESSAGE_LENGTH = 4;

	//order of message sent (ignore if expired)
	static int stateMessageNo = 0 ;

	static List<byte> helloMessage  = new List<byte> (HELLO_MESSAGE_LENGTH);
	static List<byte> movementMessage  = new List<byte> (MOVEMENT_MESSAGE_LENGTH);
	static List<byte> itemDropMessage  = new List<byte> (ITEM_DROP_MESSAGE_LENGTH);
	static List<byte> itemUseMessage  = new List<byte> (ITEM_USE_MESSAGE_LENGTH);
	static List<byte> actorStateMessage  = new List<byte> (ACTOR_STATE_MESSAGE_LENGTH);
	static List<byte> gameStateMessage  = new List<byte> (GAME_STATE_MESSAGE_LENGTH);

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

	public static byte [] SerializeItemDrop (int itemID, Vector3 pos){
		//Debug.Log ("Serialize item drop");

		itemDropMessage.Clear ();
		//meta
		itemDropMessage.Add (PROTOCOL_VERSION);
		itemDropMessage.Add ((byte)Communicator.MESSAGE_TYPE_ITEM_DROP);
		//data
		itemDropMessage.AddRange (System.BitConverter.GetBytes (itemID));  
		itemDropMessage.AddRange (System.BitConverter.GetBytes (pos.x));  
		itemDropMessage.AddRange (System.BitConverter.GetBytes (pos.y)); 

		//Debug.Log ("Serialize item drop done");
		return itemDropMessage.ToArray ();
	}

	public static byte [] SerializeItemUse (int itemID, Vector3 pos){
		//Debug.Log ("Serialize item use");

		itemUseMessage.Clear ();
		//meta
		itemUseMessage.Add (PROTOCOL_VERSION);
		itemUseMessage.Add ((byte)Communicator.MESSAGE_TYPE_ITEM_USE);
		//data
		itemUseMessage.AddRange (System.BitConverter.GetBytes (itemID));  
		itemUseMessage.AddRange (System.BitConverter.GetBytes (pos.x));  
		itemUseMessage.AddRange (System.BitConverter.GetBytes (pos.y)); 

		//Debug.Log ("Serialize item use done");
		return itemUseMessage.ToArray ();
	}



	public static byte [] SerializeGameState (){
		//Debug.Log ("Serialize movement");

		gameStateMessage.Clear ();
		//meta
		gameStateMessage.Add (PROTOCOL_VERSION);
		gameStateMessage.Add ((byte)Communicator.MESSAGE_TYPE_GAME_STATE);
		//data
		gameStateMessage.AddRange (System.BitConverter.GetBytes (GameState.GameEnded));  
		gameStateMessage.AddRange (System.BitConverter.GetBytes (GameState.GameWon)); 

		//Debug.Log ("Serialize movement done");
		return gameStateMessage.ToArray ();
	}


	public static byte [] SerializeActorState (Rigidbody2D playerBody, bool pFalling,
		Rigidbody2D enemyBody, bool eFalling,
		Rigidbody2D cakeBody, bool cFalling){
		//Debug.Log ("Serialize state");

		actorStateMessage.Clear ();
		//meta
		actorStateMessage.Add (PROTOCOL_VERSION);
		actorStateMessage.Add ((byte)Communicator.MESSAGE_TYPE_ACTOR_STATE);
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


