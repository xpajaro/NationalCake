using UnityEngine;    
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MenuUIHandler : MonoBehaviour {

	public void JoinGame(){
		SceneManager.LoadScene (Constants.STAGING_SCENE);
	}

}

