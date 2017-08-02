using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class GameControls : MonoBehaviour {

	public GameObject cake, enemy, cakeEffigy, spillPrefab, holder1, holder2 ;

	public GameObject soundButton, musicButton;
	public Sprite soundOnSprite, soundOffSprite, musicOnSprite, musicOffSprite;
	bool soundOn = true, musicOn = true;

	float HOLDER_DISTANCE = 1.5f;

	GameObject iconTouched;

	public static ItemManager itemManager;

	Rigidbody2D playerBody, enemyBody;

	Moving movePlayer;
	ActivateItem itemActivator;

	Animator animator, enemyAnimator;
	string PLAYER_VELOCITY_PARAMETER = "playerVelocity";
	string ENEMY_VELOCITY_PARAMETER = "enemyVelocity";

	bool moving;

	public AudioClip playerRunningSound;

	void Start () {
		//components
		playerBody = GetComponent<Rigidbody2D> ();
		enemyBody = enemy.GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		enemyAnimator = enemy.GetComponent<Animator> ();

		//classes
		itemActivator = new ActivateItem (cake, cakeEffigy);
		movePlayer = new Moving (gameObject, playerRunningSound);
	}

	void FixedUpdate (){
		HandleTouch ();
		UpdateAnimationParemeters ();
	}

	void UpdateAnimationParemeters (){
		animator.SetFloat (PLAYER_VELOCITY_PARAMETER, playerBody.velocity.magnitude);
		enemyAnimator.SetFloat (ENEMY_VELOCITY_PARAMETER, enemyBody.velocity.magnitude);
	}

	//-------------------------------------------
	// Handle player input -- consider making this screen touch not player touch
	//-------------------------------------------
	// host player decided how movement happens
	// client player sends impulse to host and host returns game state
	// game state contains all positions & velocities
	//-------------------------------------------

	void HandleTouch(){
		if (Input.touchCount > 0) {
			Touch touch = Input.GetTouch (0);

			if (touch.phase == TouchPhase.Began) {
				TouchStarted (touch);

			} else if (touch.phase == TouchPhase.Ended) {
				TouchEnded (touch);

			} else if (touch.phase == TouchPhase.Canceled) {
				TouchCanceled ();
			}
		}
	} 


	void TouchStarted (Touch touch){

		Vector2 touchPosition = Camera.main.ScreenToWorldPoint (touch.position);

		if (iconTouched != null ) { //touched before, activate item
			HandleItemActivation (touchPosition);

		} else if ( TouchingGameObject(soundButton, touchPosition)) {
			soundOn = changeButton (soundButton, soundOn, soundOnSprite, soundOffSprite);
			SoundManager.instance.ToggleAllSound (soundOn);

		} else if  ( TouchingGameObject(musicButton, touchPosition))  {
			musicOn = changeButton (musicButton, musicOn, musicOnSprite, musicOffSprite);
			SoundManager.instance.ToggleMusic (musicOn);

		} else {
			GameObject holderTouched = GetHolderTouched (touchPosition);

			if (holderTouched != null) {
				iconTouched = itemManager.GetIconByHolder (holderTouched); 

				if (iconTouched != null) {
					ChangeIconHighlight (iconTouched, 0.5f);

					if (iconTouched.name.StartsWith (Constants.ICON_GHOST_NAME)) { // do ghost
						HandleItemActivation (touch.position);
					} 
				} 
			} else {
				movePlayer.MovementInputStarted (touch.position);
				moving = true;
			}
		}
	}

	void TouchEnded (Touch touch) {
		if (moving) {
			if (GameSetup.isHost) {
				StartCoroutine (WaitForClient ());
			}
			movePlayer.MovementInputEnded (touch.position);
			moving = false;
		}
	}

	IEnumerator WaitForClient(){
		float lag = NetworkManager.Instance.networkLag / 1000f;
		Debug.Log (string.Format ("xo waiting for lag {0}", lag));
		yield return new WaitForSeconds (lag);
		Debug.Log ("waiting done");
	}

	void TouchCanceled () {
		moving = false;
	}
		


	//-------------------------------------------
	// item/icon logic
	//-------------------------------------------
	void HandleItemActivation (Vector2 position){
		Vector2 scaledPosition = Camera.main.ScreenToWorldPoint (position);
		int itemType = itemActivator.Activate (iconTouched,  scaledPosition);

		if (itemType == Constants.ITEM_JUJU) {
			Invoke ("DeactivateJuju", ActivateItem.JUJU_COOLDOWN);
		} 

		CleanupHolder (itemType);
	}

	void CleanupHolder (int itemType){
		GameObject holder = itemManager.GetItemHolder (iconTouched).holder;

		//itemManager.RemoveHolderHighlight (holder);
		ChangeIconHighlight (iconTouched, 2f);

		if (itemType != ActivateItem.INVALID_ICON) {
			itemManager.RemoveHolderIcon (holder);

		} else {
			SoundManager.instance.PlayWarning ();
		}

		iconTouched = null;
	}

	void DeactivateJuju (){
		itemActivator.DeactivateJuju ();
	}

	GameObject GetHolderTouched (Vector2 touchPosition){
		GameObject holderTouched = null;

		if ( TouchingGameObject(holder1, touchPosition))  {
			holderTouched = holder1;

		} else if  ( TouchingGameObject(holder2, touchPosition)) {
			holderTouched = holder2;
		}

		return holderTouched ;
	}


	bool changeButton (GameObject button, bool audioOn, Sprite onSprite, Sprite offSprite){
		SpriteRenderer btnRenderer = button.GetComponent<SpriteRenderer> ();
		btnRenderer.sprite = audioOn ? onSprite : offSprite; //show on button if we're shutting off

		return !audioOn;
	}


	bool TouchingGameObject(GameObject item, Vector2 touchPosition){
		return (Vector2.Distance (item.transform.position, touchPosition) < HOLDER_DISTANCE);
	}

	GameObject GetIconTouched (GameObject holderTouched){
		return itemManager.GetIconByHolder (holderTouched);
	}

	void ChangeIconHighlight(GameObject icon, float value){
		Color iconColor = iconTouched.GetComponent<SpriteRenderer> ().color;
		iconTouched.GetComponent<SpriteRenderer> ().color = iconColor * value;
	}

}
