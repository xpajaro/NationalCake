using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;

public class FacebookLogin {

	public void Init(){
		FB.Init(this.OnInitComplete, this.OnHideUnity);
		Debug.Log( "FB.Init() called with " + FB.AppId);
	}


	public void CallFBLogin(){
		Debug.Log ("Logging into facebook");

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

		Debug.Log (logMessage);

		if (AccessToken.CurrentAccessToken != null) {
			FirebaseLogin.Instance.Login (AccessToken.CurrentAccessToken.ToString ());

		} else {
			SceneManager.LoadScene (Constants.MENU_SCENE_NAME);
			Debug.Log ("facebook access token is null");
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

			LocalStorage.Instance.SaveAccessToken (AccessToken.CurrentAccessToken.ToString ());
			FirebaseLogin.Instance.Login (AccessToken.CurrentAccessToken.ToString ());

		} else {
			Debug.Log("Empty Response\n");
		}

		Debug.Log(result.ToString());
	}


}
