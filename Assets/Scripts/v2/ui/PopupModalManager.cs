using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopupModalManager : MonoBehaviour {

	public GameObject modalPanel;
	public Button btnAction ;
	public Text lblMessage, txtAction;

	public static PopupModalManager Instance;

	void Awake() {
		if (Instance == null) {
			Instance = this;
			DontDestroyOnLoad (gameObject);

		} else if (Instance != this) {
			Destroy (gameObject);
		}

	}

	public void Show (string message, UnityAction action, string actionCaption){

		Debug.Log ("showing");
		modalPanel.SetActive (true);

		lblMessage.text = message;

		txtAction.text = actionCaption;

		btnAction.onClick.RemoveAllListeners();
		btnAction.onClick.AddListener (action);
		btnAction.onClick.AddListener (ClosePanel);
	}


	void ClosePanel () {
		modalPanel.SetActive (false); 
	}
}
