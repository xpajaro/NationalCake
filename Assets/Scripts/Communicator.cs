using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Communicator : MonoBehaviour {

	public static string MESSAGE_TYPE = "MESSAGE_TYPE" ;
	public static string IMPULSE = "CONTENT" ;

	public static void SayHello (){
		Dictionary<string, object> msg = new Dictionary<string, object> ();
		msg.Add (MESSAGE_TYPE, NetworkManager.MessageTypes.hello);

		NetworkManager.SendMessage ( Utilities.Serialize(msg) );
	}

	public static void ShareMovement (Vector3 impulse){
		Dictionary<string, object> msg = new Dictionary<string, object> ();
		msg.Add (MESSAGE_TYPE, NetworkManager.MessageTypes.mvmt);
		msg.Add (IMPULSE, impulse);

		NetworkManager.SendMessage ( Utilities.Serialize(msg) );
	}

	public static void ShareState (Rigidbody2D playerBody, Rigidbody2D enemyBody, Rigidbody2D princessBody ){

		/*string msg = NetworkManager.MessageTypes.state.ToString () +
					 NetworkManager.HEADER_DIVIDER +
					 Utilities.SerializeVector3 (impulse);*/
	}
}
