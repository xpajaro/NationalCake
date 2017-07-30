using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignIn : MonoBehaviour {

	Firebase.Auth.FirebaseAuth auth ;
	Firebase.DependencyStatus dependencyStatus ;
	Firebase.Auth.FirebaseUser user = null;

	//TODO:
	//save sign in and sign out to DB

	// Use this for initialization
	void Start () {
		dependencyStatus = Firebase.FirebaseApp.CheckDependencies();

		if (dependencyStatus != Firebase.DependencyStatus.Available) {
			LoadFirebaseDependencies ();
		} else {
			InitializeFirebase ();
		}

		BeginOnboarding ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LoadFirebaseDependencies(){
		Debug.Log("Loading dependencies");
		Firebase.FirebaseApp.FixDependenciesAsync().ContinueWith(task => {
			dependencyStatus = Firebase.FirebaseApp.CheckDependencies();

			if (dependencyStatus == Firebase.DependencyStatus.Available) {
				Debug.Log("dep status av");
				InitializeFirebase();
			} else {
				Debug.LogError(
					"Could not resolve all Firebase dependencies: " + dependencyStatus);
			}
		});
	}

	void InitializeFirebase() {
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
		Debug.Log("fb init");
	}

	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		Debug.Log("auth changed");

		if (auth.CurrentUser != user) {
			Debug.Log("user changed");
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;

			if (!signedIn && user != null) {
				Debug.Log("Signed out " + user.UserId);
			}

			user = auth.CurrentUser;

			if (signedIn) {
				Debug.Log("Signed in " + user.UserId);
			}
		}
	}


	void BeginOnboarding() {
		Debug.Log("on boarding");
		if (LocalStorage.Instance.GetUserID () == string.Empty) {

			Debug.Log ("sign in");
			SignInAnonymously ();
		} else {
			Debug.Log ("you're cleared");
		}
	}

	void SignInAnonymously(){

		Debug.Log("anon login");
		auth.SignInAnonymouslyAsync().ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("SignInAnonymouslyAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;
			SaveUserDetails(newUser);
			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});
	}

	void SaveUserDetails(Firebase.Auth.FirebaseUser user){

		Debug.Log("save user");
		LocalStorage.Instance.SaveUserID (user.UserId);
		LocalStorage.Instance.SaveUserDisplayName (user.DisplayName);
	}
}
