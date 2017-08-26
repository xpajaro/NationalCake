using UnityEngine;    
using UnityEngine.SceneManagement;
using System.Collections;
//using GooglePlayGames;
using System;

public class UIHandler : MonoBehaviour {
	int INVITE_SCENE = 0;
	static int MENU_SCENE = 1;
	int MAIN_SCENE = 2;
	int TUTORIAL_SCENE_1 = 4;
	int TUTORIAL_SCENE_2 = 5;
	int TUTORIAL_SCENE_3 = 6;



	public static void GotoMenuScene (){
		SceneManager.LoadScene (MENU_SCENE);
	}

	public void GoToMenu (){
		//SoundManager.Instance.PlayClick ();
		UIHandler.GotoMenuScene ();
	}

	public void GotoMainScene (){
		SceneManager.LoadScene (MAIN_SCENE);
	}

	public void GotoTutorialScene1 (){
		//SoundManager.Instance.PlayClick ();
		SceneManager.LoadScene (TUTORIAL_SCENE_1);
	}

	public void GotoTutorialScene2 (){
		//SoundManager.Instance.PlayClick ();
		SceneManager.LoadScene (TUTORIAL_SCENE_2);
	}

	public void GotoTutorialScene3 (){
		//SoundManager.Instance.PlayClick ();
		SceneManager.LoadScene (TUTORIAL_SCENE_3);
	}



}
