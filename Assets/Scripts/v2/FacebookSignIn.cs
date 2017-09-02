using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;

public class FacebookSignIn : MonoBehaviour {

	Firebase.Auth.FirebaseAuth auth ;

	public void Init(Firebase.Auth.FirebaseAuth _auth){
		this.auth = _auth;

		FB.Init(this.OnInitComplete, this.OnHideUnity);
		Debug.Log( "FB.Init() called with " + FB.AppId);
	}

	private void CallFBLogin(){
		FB.LogInWithReadPermissions(
			new List<string>() { "public_profile", "email", "user_friends" }, 
			this.HandleResult);
	}

	public void CallFBLogout() {
		FB.LogOut();
	}

	private void OnInitComplete() {
		string logMessage = string.Format(
			"OnInitCompleteCalled IsLoggedIn='{0}' IsInitialized='{1}'",
			FB.IsLoggedIn,
			FB.IsInitialized);
		
		if (AccessToken.CurrentAccessToken != null) {
			Debug.Log(AccessToken.CurrentAccessToken.ToString());
		}
	}

	private void OnHideUnity(bool isGameShown) {
		Debug.Log("Is game shown: " + isGameShown);
	}


	protected void HandleResult(IResult result) {
		if (result == null) {
			Debug.Log("Null Response\n");
			return;
		}


		// Some platforms return the empty string instead of null.
		if (!string.IsNullOrEmpty(result.Error)) {
			Debug.Log("Error Response:\n" + result.Error);

		} else if (result.Cancelled) {
			Debug.Log("Cancelled Response:\n" + result.RawResult);

		} else if (!string.IsNullOrEmpty(result.RawResult)) {
			Debug.Log("Success Response:\n" + result.RawResult);

		} else {
			Debug.Log("Empty Response\n");
		}

		Debug.Log(result.ToString());
	}


	void FirebaseFacebookLogin(string accessToken){

		Debug.Log("anon login");
		Firebase.Auth.Credential credential =
			Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("SignInAnonymouslyAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;

			LocalStorage.Instance.SaveUserDetails(newUser);

			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});
	}

}
