using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class GameController : NetworkBehaviour {

	public void ActivateGhost(Vector2 position){
		CmdGhostCake (isServer, position);
	}

	[Command]
	void CmdGhostCake (bool sender, Vector2 position){
		// Vector2 positionInWorld = 
		ItemManager.Instance.UpdateUsedItems (sender, position);
		RpcGhostCake ();
	}

	[ClientRpc]
	void RpcGhostCake (){
		Cake.LocalInstance.ActivateGhost ();
	}
}
