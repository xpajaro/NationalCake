using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PopupModalManager : MonoBehaviour {

	public GameObject modalPanel, modalPanelConfirm;
	public Button btnAction, btnOK, btnCancel;
	public Text lblMessage,  txtAction, lblQuestion;

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
		modalPanel.SetActive (true);

		lblMessage.text = message;

		txtAction.text = actionCaption;

		btnAction.onClick.RemoveAllListeners();
		btnAction.onClick.AddListener (action);
		btnAction.onClick.AddListener (ClosePanel);
	}

	public void Confirm (string question, UnityAction action){
		modalPanelConfirm.SetActive (true);

		lblQuestion.text = question;

		btnOK.onClick.RemoveAllListeners();
		btnOK.onClick.AddListener (action);
		btnOK.onClick.AddListener (ClosePanel);

		btnCancel.onClick.RemoveAllListeners ();
		btnCancel.onClick.AddListener (ClosePanel);
	}


	void ClosePanel () {
		modalPanel.SetActive (false); 
		modalPanelConfirm.SetActive (false); 
	}
}
