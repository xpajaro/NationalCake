using UnityEngine;    
using UnityEngine.SceneManagement;
using System.Collections;
using GooglePlayGames;
using System;

public class UIHandler : MonoBehaviour {
	int INVITE_SCENE = 0;
	static int MENU_SCENE = 1;
	int MAIN_SCENE = 2;
	int TUTORIAL_SCENE = 3;


	public void SetupGame(){
		GameSetup.setupGame ();
	}

	public void InviteUser (){
		//Debug.Log ("invite player");
		NetworkManager.Instance.InvitePlayer ();
		//Debug.Log ("invite player done");
	}

	public void CheckInbox (){
		//Debug.Log ("check inbox");
		NetworkManager.Instance.CheckInbox ();
		//Debug.Log ("check inbox done");
	}

	public void GotoInviteScene (){
		SceneManager.LoadScene (INVITE_SCENE);
	}

	public static void GotoMenuScene (){
		SceneManager.LoadScene (MENU_SCENE);
	}

	public void GotoMainScene (){
		SceneManager.LoadScene (MAIN_SCENE);
	}

	public void GotoTutorialScene (){
		SceneManager.LoadScene (TUTORIAL_SCENE);
	}


}
