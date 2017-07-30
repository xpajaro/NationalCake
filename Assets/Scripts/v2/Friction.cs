using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friction : MonoBehaviour {

	const float LEG_HEIGHT = 0.4f;
	const float MOVT_DAMPING = 0.5f;

	Rigidbody2D rigidBody;


	void Start () {
		rigidBody = GetComponent<Rigidbody2D> ();
	}


	void FixedUpdate () {
		Vector2 standingPosition = GetFeetPosition (gameObject);
		bool onStage = Stage.Instance.IsOnStage (standingPosition);

		if (onStage) {
			dampMovement (rigidBody, MOVT_DAMPING);
		} else {
			// rigidBody.velocity = Vector2.zero;
		}
	}

	Vector2 GetFeetPosition (GameObject _gameObject){
		Vector2 feetPosition = new Vector2 ();
		feetPosition.x = _gameObject.transform.position.x;
		feetPosition.y = _gameObject.transform.position.y - LEG_HEIGHT;

		return feetPosition;
	}

	void dampMovement (Rigidbody2D rb, float damping){
		if (rigidBody.velocity.x != 0 || rigidBody.velocity.y != 0) {
			rigidBody.velocity = rigidBody.velocity * damping;

		} else if (rb.velocity.magnitude < 0.0001) {
			rigidBody.velocity = new Vector2 (0, 0);
		}
	}

}
