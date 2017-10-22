using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuSceneManager : MonoBehaviour {
	public Text  txtReserves;


	void Start () {
		SoundManager.Instance.waterAmbienceSource.Play ();
		ShowRevenue ();
	}

	void ShowRevenue(){
		txtReserves.text = SessionManager.Instance.playerData.formattedRevenue ;
	}


}
