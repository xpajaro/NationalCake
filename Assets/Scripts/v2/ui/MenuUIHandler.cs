using UnityEngine;    
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class MenuUIHandler : MonoBehaviour {

	public void JoinGame(){
		SceneManager.LoadScene (Constants.STAGING_SCENE);
	}

	public void InviteFriends() {
		Debug.Log("Sending an invitation...");

		var invite = new Firebase.Invites.Invite () {
			TitleText = "Share the Cake",
			MessageText = "Challenge the cabal on the best game in Africa!",
			CallToActionText = "Download National Cake",
			DeepLinkUrl = new System.Uri("http://google.com/abc"),
		};

		Firebase.Invites.FirebaseInvites
		.SendInviteAsync(invite).ContinueWith(HandleSentInvite);
	}

	private void HandleSentInvite( Task < Firebase.Invites.SendInviteResult > sendTask) {
		if (sendTask.IsCanceled) {
			Debug.Log("Invitation canceled.");

		} else if (sendTask.IsFaulted) {
			Debug.Log("Invitation encountered an error:");
			Debug.Log(sendTask.Exception.ToString());

		} else if (sendTask.IsCompleted) {
			Debug.Log("SendInvite: " + (new List<string>(sendTask.Result.InvitationIds)).Count +
				" invites sent successfully.");
			
			foreach (string id in sendTask.Result.InvitationIds) {
				Debug.Log("SendInvite: Invite code: " + id);
			}
		}
	}

	public void LoginToFacebook(){
		FirebaseLogin.Instance.facebookLogin.CallFBLogin ();
	}

}

