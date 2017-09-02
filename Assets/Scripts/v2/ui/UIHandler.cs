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
		if (SceneManager.GetActiveScene ().name.Equals (Constants.STAGING_SCENE_NAME)) {
			MatchMaker.Instance.DestroyCurrentMatch ();

			SceneManager.LoadScene (Constants.MENU_SCENE);
		}

		SceneManager.LoadScene (Constants.MENU_SCENE);
		
	}

	public void GotoStagingScene (){
		SceneManager.LoadScene (Constants.STAGING_SCENE);
	}

	public void GotoMainScene (){
		SceneManager.LoadScene (Constants.MAIN_SCENE);
	}



}
