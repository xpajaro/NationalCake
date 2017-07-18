using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

//massages data to and from the network interfaces
public class Communicator  {

	public static char MESSAGE_TYPE_HELLO = 'H';
	public static char MESSAGE_TYPE_MOVEMENT = 'M';
	public static char MESSAGE_TYPE_ITEM_DROP = 'D';
	public static char MESSAGE_TYPE_ITEM_USE = 'U';
	public static char MESSAGE_TYPE_ACTOR_STATE = 'A';
	public static char MESSAGE_TYPE_GAME_STATE = 'G';
	public static char MESSAGE_TYPE_GONG_STATE = 'g';
	public static char MESSAGE_TYPE_BARREL_HIT = 'B';
	public static char MESSAGE_TYPE_SLIP = 'S';


	public static char MESSAGE_TYPE_SERVER_TIMESTAMP = '1';
	public static char MESSAGE_TYPE_CLIENT_TIMESTAMP = '2';

	public StateUpdates stateUpdates;
	public ItemUpdates itemUpdates;
	public Gong gong;

	bool gameStarted;

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

		for (int i = 0; i < 5; i++) { // make sure it gets there
			NetworkManager.Instance.SendMessage (Serialization.SerializeHello (), true);
		}
		//Debug.Log ("say hello done");
	}

	public void ShareMovement (Vector2 impulse){
		//Debug.Log ("share movement");
		NetworkManager.Instance.SendFastMessage ( Serialization.SerializeMovement (impulse) );
		//Debug.Log ("share movement done");
	}

	public void ShareGameState (){
		//Debug.Log ("share game state");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeGameState (), true );
		//Debug.Log ("share game state");
	}

	public void ShareItemDrop (int item, Vector2 pos){
		//Debug.Log ("share item drop");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeItemDrop (item, pos), true );
		//Debug.Log ("share item drop");
	}

	public void ShareItemUse (int item, Vector2 pos){
		//Debug.Log ("share item use");
		NetworkManager.Instance.SendMessage ( Serialization.SerializeItemUse (item, pos), true );
		//Debug.Log ("share item use");
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

	public void ShareGongSwap (){
		NetworkManager.Instance.SendMessage ( Serialization.SerializeGongSwap (), true );
	}

	public void ShareBarrelhit(Vector2 pos, int hitCount){
		NetworkManager.Instance.SendMessage ( Serialization.SerializeBarrelHit (pos, hitCount), true );
	}

	public void ShareSlip(){
		NetworkManager.Instance.SendMessage ( Serialization.SerializeSlip (), true );
	}

	public void ShareServerGamestamp(char tag){
		NetworkManager.Instance.SendMessage ( 
			Serialization.SerializeGamestamp ( Communicator.MESSAGE_TYPE_SERVER_TIMESTAMP, tag),
			true);
	}

	public void ShareClientGamestamp(char tag){
		NetworkManager.Instance.SendMessage ( 
			Serialization.SerializeGamestamp ( Communicator.MESSAGE_TYPE_CLIENT_TIMESTAMP, tag),
			true);
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
			if (!gameStarted ){
				gameStarted = true;
				GameSetup.ChooseHost (senderID);

				NetworkManager.Instance.PingForLag ();

				GameSetup.StartGame ();
			}
		} else { 
			if (stateUpdates != null) {
				RouteMessage (msgType, msgBytes);
			}
		}

		//Debug.Log ("parse message done");
	}


	public void RouteMessage (char msgType, byte[] dataFields ){//use switch
		//Debug.Log ("route message");
		if (MESSAGE_TYPE_SERVER_TIMESTAMP.Equals (msgType) ) {
			char tag = Deserialization.GetGamestamp (dataFields);
			Communicator.Instance.ShareClientGamestamp (tag);

		} else if (MESSAGE_TYPE_CLIENT_TIMESTAMP.Equals (msgType) ) {
			char tag = Deserialization.GetGamestamp (dataFields);
			NetworkManager.Instance.CalculateLag (tag);

		} else if (MESSAGE_TYPE_MOVEMENT.Equals (msgType) ) {
			Vector2 impulse = Deserialization.GetImpulse (dataFields);
			stateUpdates.MoveEnemy (impulse);

		} else if (MESSAGE_TYPE_ACTOR_STATE.Equals (msgType) ){
			ActorState state = Deserialization.GetActorState (dataFields);
			stateUpdates.UpdateActors (state);

		} else if (MESSAGE_TYPE_ITEM_DROP.Equals (msgType) ){
			Dictionary<string, object> itemDropped = Deserialization.GetItemDrop (dataFields);
			itemUpdates.ShowDroppedItem (itemDropped);

		} else if (MESSAGE_TYPE_ITEM_USE.Equals (msgType) ){
			Dictionary<string, object> itemUsed = Deserialization.GetItemUse (dataFields);
			itemUpdates.UseItem (itemUsed);

		} else if (MESSAGE_TYPE_GAME_STATE.Equals (msgType) ){
			Deserialization.UpdateGameState (dataFields);
			stateUpdates.EndGame ();

		} else if (MESSAGE_TYPE_GONG_STATE.Equals (msgType) ){
			gong.HandleSwap ();

		} else if (MESSAGE_TYPE_BARREL_HIT.Equals (msgType) ){
			Dictionary<string, object> barrelHit = Deserialization.GetBarrelHit (dataFields);
			Barrel.updateClientBarrel (barrelHit);

		} else if (MESSAGE_TYPE_SLIP.Equals (msgType) ){
			Spill.PlaySounds ();
		}
		//Debug.Log ("route message done");
	}
}
