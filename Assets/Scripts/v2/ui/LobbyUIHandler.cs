using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUIHandler : MonoBehaviour {

	
	// Update is called once per frame
	public void previous () {
		LobbySceneManager.Instance.PreviousRoom ();
	}

	public void next (){
		LobbySceneManager.Instance.NextRoom ();
	}

}
