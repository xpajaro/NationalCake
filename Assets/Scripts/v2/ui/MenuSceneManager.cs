using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuSceneManager : MonoBehaviour {
	public Text  txtReserves;
	const int REVENUE_REFRESH_RATE = 2;

	void Start () {
		SoundManager.Instance.waterAmbienceSource.Play ();
		InvokeRepeating("ShowRevenue", 0, REVENUE_REFRESH_RATE);
	}

	void ShowRevenue(){
		if (SessionManager.Instance.playerData != null) {
			SessionManager.Instance.LoadPlayerData ();
		} 

		txtReserves.text = SessionManager.Instance.playerData.formattedRevenue;
	}


}
