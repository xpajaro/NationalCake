using UnityEngine;    
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class MenuUIHandler : MonoBehaviour {

	public void JoinGame(){
		SceneManager.LoadScene (Constants.STAGING_SCENE);
	}


}

