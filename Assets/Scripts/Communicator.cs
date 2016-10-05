using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//massages data to and from the network interfaces
public class Communicator : MonoBehaviour {

	public static string MESSAGE_TYPE = "MESSAGE_TYPE" ;
	public static string IMPULSE = "CONTENT" ;
	public static string ACTOR_STATE = "ACTOR_STATE" ;

	enum MessageTypes {hello, mvmt, item, state};

	NetworkManager networkManager;

	public Communicator(){
		networkManager = new NetworkManager();
	}

	//-------------------------------------------
	// Sending
	//-------------------------------------------

	public void SayHello (){
		Dictionary<string, object> msg = new Dictionary<string, object> ();
		msg.Add (MESSAGE_TYPE, MessageTypes.hello.ToString() );

		Debug.Log ("hello sent");
		networkManager.SendMessage ( Utilities.Serialize(msg) );
	}

	public void ShareMovement (Vector3 impulse){
		Dictionary<string, object> msg = new Dictionary<string, object> ();
		msg.Add (MESSAGE_TYPE, MessageTypes.mvmt);
		msg.Add (IMPULSE, impulse);

		networkManager.SendMessage ( Utilities.Serialize(msg) );
	}

	//actors are player, enemy and cake (movable game elements with physics)
	public void ShareActorState (Rigidbody2D playerBody, Rigidbody2D enemyBody, Rigidbody2D cakeBody ){
		Dictionary<string, object> msg = new Dictionary<string, object> ();

		ActorState actorState = new ActorState ();
		actorState.preparePayload (playerBody, enemyBody, cakeBody);

		msg.Add (MESSAGE_TYPE, MessageTypes.mvmt);
		msg.Add (ACTOR_STATE, actorState);

		networkManager.SendMessage ( Utilities.Serialize(msg) );
	}


	//-------------------------------------------
	// Receiving
	//-------------------------------------------

	public void ParseMessage (string senderID, byte[] msgBytes){
		Dictionary<string, object> msg = Utilities.Deserialize(msgBytes);
		Debug.Log ("parsing message");
		Debug.Log (msg);

		string msgType = msg [Communicator.MESSAGE_TYPE].ToString ();
		Debug.Log ("message type");
		Debug.Log (msgType);

		if ( msgType.Equals (MessageTypes.hello.ToString()) ) {
			GameManager.ChooseHost (senderID);
			GameManager.StartGame ();
		} else {
			RouteMessage (msg);
		}

		Debug.Log ("parsing message done");
	}

	public void RouteMessage (Dictionary<string, object> msg){
		Debug.Log ("route message");
		string msgType = msg [Communicator.MESSAGE_TYPE].ToString ();

		if ( msgType.Equals (MessageTypes.mvmt.ToString()) ) {
			GameControls.MoveEnemy (msg);
		} else if (msgType.Equals (MessageTypes.state.ToString()) ){
			GameControls.UpdateActors (msg);
		}
		Debug.Log ("route message done");
	}
}
