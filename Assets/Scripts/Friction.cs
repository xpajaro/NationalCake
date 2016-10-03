using UnityEngine;
using System.Collections;

public class Friction : MonoBehaviour {
	
	public float MOVT_DAMPING = 0.5f;

	public GameObject stage;

	Rigidbody2D rb;


	void Start () {
		rb = GetComponent<Rigidbody2D>();
	}


	void FixedUpdate () {

		if (MovementHandler.isOnStage (transform.position, stage)) {
			MovementHandler.dampMovement (rb, MOVT_DAMPING);
		} 
	}


}
