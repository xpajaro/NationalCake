using UnityEngine;
using System.Collections;

public class ItemUpdates : MonoBehaviour {

	public GameObject[]  wines  = new GameObject[4];
	public GameObject testObj;
	SpriteRenderer [] wineRenderers;


	void Start (){
		Communicator.Instance.itemUpdates = this;

		if (!GameSetup.isHost) {
			LoadRenderers ();
		}
	}

	void LoadRenderers (){
		wineRenderers[0] = wines[0].GetComponent<SpriteRenderer> ();
		wineRenderers[1] = wines[1].GetComponent<SpriteRenderer> ();
		wineRenderers[2] = wines[2].GetComponent<SpriteRenderer> ();
		wineRenderers[3] = wines[3].GetComponent<SpriteRenderer> ();

		Presenter.Detach (wines [0], wineRenderers [0]);
		Presenter.Detach (testObj, testObj.GetComponent<SpriteRenderer> ());
	}


	public void UpdateWine (WineState wineState){
		int i = wineState.TagNo - 1;
		if (wineState.TagNo.Equals( Wine.HIDE_WINE)) {
			Debug.Log ("wine state hide" + i + " action " + wineState.Action);
			Presenter.Detach (wines [i], wineRenderers [i]);
		} else {
			Debug.Log ("wine state show" + wineState.TagNo + " action " + wineState.Action);
			Presenter.Attach (wines [i], wineRenderers [i]);
		}
	}



}
