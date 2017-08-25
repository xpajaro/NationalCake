using UnityEngine;    
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class MenuUIHandler : MonoBehaviour {

	public void JoinGame(){
		MatchMaker.Instance.StartNewMatch ();
	}

}

