using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class StagingUIHandler : MonoBehaviour {

	public static bool isAutomatch = true;

	public Text captionText;
	ArrayList captions = new ArrayList();


	void Start () {
		LoadCaptionOptions ();
		Invoke ("GoBackToMenu", 60f);
		Invoke ("StartMatch", 0f);

		if (isAutomatch) {
			InvokeRepeating ("RotateCaptions", 0f, 3f);

		} else {
			captionText.text = "";
		}
	}

	void LoadCaptionOptions (){
		captions.Add("searching for national troublemakers");
		captions.Add("looking for corrupt officials");
		captions.Add("watching out for cake collectors");
		captions.Add("investigating money choppers");
		captions.Add("seeking evildoers");
		captions.Add("looking for enemies of progress");
		captions.Add("locating principalities and powers");
		captions.Add("inquiring after selfish interests");
		captions.Add("hunting for nonsense elements");
		captions.Add("fishing out cabal members");
		
	}


	void RotateCaptions (){
		System.Random r = new System.Random();
		int captionIndex = r.Next (0, captions.Count);

		if (captionText != null){
			captionText.text =  captions [captionIndex] as string;
		} 
	}

	void StartMatch(){
		MatchMaker.Instance.StartNewMatch ();
	}

	public void GoBackToMenu(){
		SceneManager.LoadScene (Constants.MENU_SCENE);
	}
}
