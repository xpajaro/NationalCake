using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//massages data to and from the network interfaces
public class Communicator  {

	public static string MESSAGE_TYPE = "MESSAGE_TYPE" ;
	public static string IMPULSE = "CONTENT" ;
	public static string ACTOR_STATE = "ACTOR_STATE" ;

	public enum MessageTypes {HELLO, MOVEMENT, ITEM, STATE};

	GameUpdates gameUpdates;

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
		Debug.Log ("say hello");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeHello () );
		Debug.Log ("say hello done");
	}

	public void ShareMovement (Vector3 impulse){
		Debug.Log ("share movement");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeMovement (impulse) );
		Debug.Log ("share movement done");
	}

	//actors are player, enemy and cake (movable game elements with physics)
	public void ShareState (Rigidbody2D playerBody, Rigidbody2D enemyBody, Rigidbody2D cakeBody ){
		Debug.Log ("share state");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeState ( playerBody, enemyBody, cakeBody ) );
		Debug.Log ("share state done");
	}


	//-------------------------------------------
	// Receiving
	//-------------------------------------------


	public void ParseMessage (string senderID, byte[] msgBytes){
		Debug.Log ("parse message");
		string msg = Serialization.Deserialize (msgBytes);
		Debug.Log (msg);

		string[] dataFields = Serialization.GetDataFields (msg);

		MessageTypes msgType = Serialization.GetMessageType (dataFields);
		Debug.Log ("message type");
		Debug.Log (msgType.ToString());

		if ( msgType == MessageTypes.HELLO) {
			GameManager.ChooseHost (senderID);
			GameManager.StartGame ();
		} else { //game has started
			gameUpdates = new GameUpdates (); //we can load game updates now, since stage is set
			RouteMessage (msgType, dataFields);
		}

		Debug.Log ("parse message done");
	}

	public void RouteMessage (MessageTypes msgType, string[] dataFields ){
		Debug.Log ("route message");

		if (  msgType == MessageTypes.MOVEMENT ) {
			Vector2 impulse = Serialization.GetImpulse (dataFields);
			gameUpdates.MoveEnemy (impulse);

		} else if (msgType == MessageTypes.STATE ){
			ActorState state = Serialization.GetState (dataFields);
			gameUpdates.UpdateActors (state);
		}
		Debug.Log ("route message done");
	}
}
