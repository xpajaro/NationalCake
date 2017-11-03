using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIHandler : MonoBehaviour {

	public void ExitGame(){
		GameState.gameEnded = true;
		GameState.gameWon = false;

		SessionManager.Instance.UpdateReserves (false);

		UIHandler.GoToLoserScreen ();
	}


}

