using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuSceneManager : MonoBehaviour {
	public GameObject loginPanel;
	public Text  txtReserves, txtHello;
	public Image meterFiller;

	public static MenuSceneManager Instance;

	Player player;


	void Start () {
		SoundManager.Instance.waterAmbienceSource.Play ();
	}


}
