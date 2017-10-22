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

	public void GotoStagingScene (){
		SceneManager.LoadScene (Constants.STAGING_SCENE);
	}

	public void GotoMainScene (){
		SceneManager.LoadScene (Constants.MAIN_SCENE);
	}

	public void GoToGameScoresScene(){
		SceneManager.LoadScene (Constants.GAME_SCORES_SCENE);
	}

	public void GoToBragScene(){
		SceneManager.LoadScene (Constants.BRAG_SCENE);
	}

	public void GoToLobbyScene(){
		SceneManager.LoadScene (Constants.LOBBY_SCENE);
	}


}
