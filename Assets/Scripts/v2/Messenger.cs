using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class Messenger : NetworkBehaviour {

	NetworkConnection connection;

	public void Init(NetworkConnection _connection)
	{
		connection = _connection;
		NetworkServer.RegisterHandler(NetworkMessages.NC_MSG_ID, ReceiveIntMessage);
	}

	public void SendIntMessage(int msgContent){
		var msg = new IntegerMessage(msgContent);
		connection.Send(NetworkMessages.NC_MSG_ID, msg);
	}

	void ReceiveIntMessage(NetworkMessage netMsg)
	{
		var beginMessage = netMsg.ReadMessage<IntegerMessage>();
		Debug.Log("received intmsg " + beginMessage.value);
	}
}
