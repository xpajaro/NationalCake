using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TxtWin : MonoBehaviour {

	void Start () {
		Text txt = GetComponent<Text> ();
		txt.text = GetWinnerMessage ();
	}

	string GetWinnerMessage(){
		string winStatus;
		if (GameState.GameWon) {
			winStatus = "You win!";
		} else {
			winStatus = "You lose!";
		}

		return winStatus;
	}
}
