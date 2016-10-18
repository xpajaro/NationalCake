﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//massages data to and from the network interfaces
public class Communicator  {

	public static string MESSAGE_TYPE = "MESSAGE_TYPE" ;
	public static string IMPULSE = "CONTENT" ;
	public static string ACTOR_STATE = "ACTOR_STATE" ;

	public enum MessageTypes {HELLO, MOVEMENT, ITEM, STATE};
	public static char MESSAGE_TYPE_HELLO = 'H';
	public static char MESSAGE_TYPE_MOVEMENT = 'M';
	public static char MESSAGE_TYPE_ITEM = 'I';
	public static char MESSAGE_TYPE_WINE = 'W';
	public static char MESSAGE_TYPE_STATE = 'S';

	public StateUpdates stateUpdates;
	public ItemUpdates itemUpdates;

	//make singleton
	public static Communicator _instance;
	public static Communicator Instance {
		get {
			if (_instance == null) {
				_instance = new Communicator();
			}
			return _instance;
		}
	}

	//-------------------------------------------
	// Sending
	//-------------------------------------------

	public void SayHello (){
		//Debug.Log ("say hello");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeHello (), true );
		//Debug.Log ("say hello done");
	}

	public void ShareMovement (Vector3 impulse){
		//Debug.Log ("share movement");
		NetworkManager.Instance.SendFastMessage ( Serialization.SerializeMovement (impulse) );
		//Debug.Log ("share movement done");
	}

	//actors are player, enemy and cake (movable game elements with physics)
	public void ShareActorState (Rigidbody2D playerBody, bool pFalling, 
		Rigidbody2D enemyBody, bool eFalling,
		Rigidbody2D cakeBody, bool cFalling ){
		//Debug.Log ("share state");
		NetworkManager.Instance.SendFastMessage ( Serialization.SerializeActorState ( playerBody, pFalling,
			enemyBody, eFalling,
			cakeBody, cFalling ) );
		//Debug.Log ("share state done");
	}

	public void ShareWineState (int tagNo, int action){
		NetworkManager.Instance.SendMessage (Serialization.SerializeWineState (tagNo, action), true);
	}


	//-------------------------------------------
	// Receiving
	//-------------------------------------------


	public void ParseMessage (string senderID, byte[] msgBytes){
		//Debug.Log ("parse message");

		char msgType = Deserialization.GetMessageType (msgBytes);
		//Debug.Log ("message type " + msgType.ToString());

		//implement some checking to make sure they start at the same time
		if ( MESSAGE_TYPE_HELLO.Equals (msgType) ) {
			GameSetup.ChooseHost (senderID);
			GameSetup.StartGame ();
		} else { 
			if (stateUpdates != null) {
				RouteMessage (msgType, msgBytes);
			}
		}

		//Debug.Log ("parse message done");
	}


	public void RouteMessage (char msgType, byte[] dataFields ){
		//Debug.Log ("route message");
		if (MESSAGE_TYPE_MOVEMENT.Equals (msgType) ) {
			Vector3 impulse = Deserialization.GetImpulse (dataFields);
			stateUpdates.MoveEnemy (impulse);

		} else if (MESSAGE_TYPE_WINE.Equals (msgType) ) {
			WineState wineState = Deserialization.GetWineState (dataFields);
			itemUpdates.UpdateWine (wineState);

		} else if (MESSAGE_TYPE_STATE.Equals (msgType) ){
			ActorState state = Deserialization.GetActorState (dataFields);
			stateUpdates.UpdateActors (state);
		}
		//Debug.Log ("route message done");
	}
}
