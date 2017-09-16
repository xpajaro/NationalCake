using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirebaseLogin : MonoBehaviour {
	const float SPLASH_SCREEN_ANIMATION_DURATION = 5f;

	Firebase.Auth.FirebaseAuth auth ;
	Firebase.DependencyStatus dependencyStatus ;
	Firebase.Auth.FirebaseUser user = null;

	FacebookLogin facebookLogin;

	public static FirebaseLogin Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}

	// Use this for initialization
	void Start () {
		Invoke ("StartLogin", SPLASH_SCREEN_ANIMATION_DURATION);
	}

	void StartLogin(){
		dependencyStatus = Firebase.FirebaseApp.CheckDependencies();

		if (dependencyStatus != Firebase.DependencyStatus.Available) {
			LoadFirebaseDependencies ();
		} else {
			InitializeFirebase ();
		}
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

		facebookLogin = new FacebookLogin ();
		facebookLogin.Init ();

		Debug.Log("fireb init");
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
				Debug.Log("Signed in " + user.UserId +  " " + user.DisplayName + " " );
			}
		}
	}


	public void Login(string accessToken){

		Debug.Log("logging into firebase");
		Firebase.Auth.Credential credential =
			Firebase.Auth.FacebookAuthProvider.GetCredential(accessToken);

		auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("Firebase Facebook login was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("Firebase Facebook login encountered an error: " + task.Exception);
				return;
			}

			Firebase.Auth.FirebaseUser newUser = task.Result;

			Debug.LogFormat("User signed in successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
			
			HandleLoginSuccess();

		});
	}

	private void HandleLoginSuccess(){
		if (SceneManager.GetActiveScene ().Equals (Constants.WELCOME_SCENE_NAME)) {
			SceneManager.LoadScene (Constants.MENU_SCENE);
		
		} else if (SceneManager.GetActiveScene ().Equals (Constants.MENU_SCENE_NAME)) {
			//load stuff
			//hide login panel
		}
	}
}
