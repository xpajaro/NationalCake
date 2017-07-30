using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();

		Color color = renderer.color;
		color.a = 0.7f;

		renderer.color = color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
