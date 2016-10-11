using UnityEngine;    
using System.Collections;
using GooglePlayGames;
using System;

public class UIHandler : MonoBehaviour {

	public void InviteUser (){
		Debug.Log ("invite player");
		NetworkManager.Instance.InvitePlayer ();
		Debug.Log ("invite player done");
	}

	public void CheckInbox (){
		Debug.Log ("check inbox");
		NetworkManager.Instance.CheckInbox ();
		Debug.Log ("check inbox done");
	}


}
