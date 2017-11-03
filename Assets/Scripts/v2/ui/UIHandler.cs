using UnityEngine;    
using UnityEngine.Networking;  
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class UIHandler : MonoBehaviour {

	public void GotoMenuScene (){
		ReturnToMenu ();
	}

	public static void ReturnToMenu(){
		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME) ||
			SceneManager.GetActiveScene ().name.Equals (Constants.MAIN_SCENE_NAME) ) {
			MatchMaker.Instance.DestroyCurrentMatch ();

		} 

		SceneManager.LoadScene (Constants.MENU_SCENE);
		
	}

	public static void GoToWinnerScreen(){
		MatchMaker.Instance.DestroyCurrentMatch ();
		SceneManager.LoadScene (Constants.GAME_WON_SCENE);
	}

	public static void GoToLoserScreen(){
		MatchMaker.Instance.DestroyCurrentMatch ();
		SceneManager.LoadScene (Constants.GAME_LOST_SCENE);
	}

	public void GotoStagingScene (){
		int reserves = SessionManager.Instance.playerData.Revenue;
		int cost = SessionManager.Instance.currentRoom.Budget;

		if (cost <= reserves) {
			SceneManager.LoadScene (Constants.STAGING_SCENE);

		} else {
			PopupModalManager.Instance.Show ("Insufficient funds", DoNothing, "okay");
		}
	}

	public void DoNothing(){
	}

	public void GotoMainScene (){
		SceneManager.LoadScene (Constants.MAIN_SCENE);
	}

	public void GoToLobbyScene(){
		SceneManager.LoadScene (Constants.LOBBY_SCENE);
	}

	public void GoToInformationScene(){
		SceneManager.LoadScene (Constants.INFORMATION_SCENE);
	}


}
