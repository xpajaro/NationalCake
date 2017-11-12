using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameConclusionSceneManager : MonoBehaviour {
	public Text txtScore;

	// Use this for initialization
	void Start () {
		int gameScore = GameState.gameWon ? SessionManager.Instance.currentRoom.Recovery :
			SessionManager.Instance.currentRoom.Budget;
		
		txtScore.text = gameScore + " billion dolarsss";


		MatchMaker.Instance.DestroyCurrentMatch ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
