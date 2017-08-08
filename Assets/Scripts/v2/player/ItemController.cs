using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public partial class GameController : NetworkBehaviour {

	Vector2 BLOC_DIAGONAL_FROM_CENTER = new Vector2 (.46f, .9f);


	void ActivateSelectedItem (Vector2 position){
		if (IsValidActivation (position)) {
			CmdActivateAttack (selectedItemRef.itemID, position, selectedItemRef.btnPosition, isServer);
			ResetSelection ();

		} else {
			Deselect ();
		}
	}

	bool IsValidActivation(Vector2 position){
		bool valid = Stage.Instance.IsOnStage (position);

		if (selectedItemRef.itemID == Constants.ITEM_BLOC){
			valid = BlocSizeAreaAvailable (position);
		}

		return valid;
	}

	void Deselect (){
		SoundManager.Instance.PlayWarning ();
		selectedItemRef.Normalize ();
		selectedItemRef = null;
	}

	void ResetSelection (){
		Destroy (selectedItemRef.gameObject);
		selectedItemRef = null;
	}

	[Command]
	void CmdActivateAttack(int itemID, Vector2 spawnPosition, Vector2 btnPosition, bool sender){
		GameSpawner.serverInstance.DropAttackItem (itemID, spawnPosition);
		ItemManager.Instance.UpdateUsedItems (sender, btnPosition);
	}

	public void ActivateGhost(Vector2 position){
		CmdGhostCake (isServer, position);
	}

	[Command]
	void CmdGhostCake (bool sender, Vector2 position){
		ItemManager.Instance.UpdateUsedItems (sender, position);
		RpcGhostCake ();
	}

	[ClientRpc]
	void RpcGhostCake (){
		Cake.LocalInstance.ActivateGhost ();
	}


	bool BlocSizeAreaAvailable(Vector2 point){
		Vector2 startPos = point + BLOC_DIAGONAL_FROM_CENTER;
		Vector2 endPos = point - BLOC_DIAGONAL_FROM_CENTER;

		return (Physics2D.OverlapArea (startPos, endPos) == null);
	}
}
