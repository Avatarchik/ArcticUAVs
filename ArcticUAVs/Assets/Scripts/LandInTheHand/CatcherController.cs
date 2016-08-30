using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

// ###############################################################
// CatcherController is responsible for the movement and behavior 
// of the catcher in the LandInTheHand scenes. It also resets the
// UAV's position and rotation if you catch a UAV 
// ###############################################################
public class CatcherController : MonoBehaviour {

// ###############################################################
// Class Variables 
// ###############################################################
	private bool collided = false;
	public bool uavCatch = false;
	private float lastBoatY;
	private AudioSource sound;
	private GameObject uav;

// ###############################################################
// Unity Functions 
// ###############################################################
	// Initialization (called once when gameObject is created).
	void Start () {
		uav = GameObject.Find("uav");
		sound = GetComponent<AudioSource> ();
		startCatcherMovement ();
		lastBoatY = GameObject.Find ("boat").transform.position.y;
	}
	// Called once per frame
	void Update () {
		if (!collided) { 
			updateCatcherPosition (); 
		}
	}
	// Called when BoxCollider2D around hand is triggered
	void OnTriggerEnter2D(Collider2D collider) {
		if (!GameController.lost && !uavCatch) {
			uavCatch = true;
			GameController.landed = true;
			GameController.message = "UAV ACQUIRED!";
			GameController.objectiveGained += 1;
			uav.transform.position = GameObject.Find ("Hand").transform.position;
			uav.transform.parent = GameObject.Find ("Hand").transform;
			StartCoroutine(resetUAV(uav));
		}
	}
	// Called on collision
	void OnCollisionEnter2D (Collision2D collision) {
		if (!collided) {
			collided = true;
			handleFirstCollision ();
		}
		handlePhysicalReaction (collision);
	}
// ###############################################################
// CatcherController Functions 
// ###############################################################
	// Put the UAV back in the sky
	private IEnumerator resetUAV (GameObject uav) {
		float timePoint = GameController.gameClock;
		yield return new WaitUntil (() => GameController.gameClock - timePoint >= 1f);
		if (!GameController.won) {
			yield return waitForSafeReset();
			uav.transform.parent = null;
			uav.transform.position = new Vector3(0f, 2.5f, 0f);
			uav.GetComponent<Rigidbody2D>().rotation = 0f;
			GameObject.Find ("Battery Bar").GetComponent<BatteryBarController> ().gainPower ();
			uavCatch = false;
			yield return new WaitForSeconds (0.5f);
			GameController.landed = false;
		}
	}
	// double check that there are no birds where the UAV is resetting
	private IEnumerator waitForSafeReset() {
		bool safe = false;
		if (GameObject.Find("Birds")) {
			while (!safe){
				safe = true;
				foreach (Transform bird in GameObject.Find("Birds").transform) {
					if (bird.position.y > 2 && bird.position.y < 3) {
						if (bird.position.x > -4 && bird.position.x < 4) {
							print("bird not safe");
							safe = false;
						}
					}
				}
				yield return new WaitForEndOfFrame();
			}
		}
	}
	// Move the catcher around on the boat
	private void updateCatcherPosition () {
		float deltaY = GameObject.Find ("boat").transform.position.y - lastBoatY;
		float newX = transform.position.x;
		if (transform.position.x < GameObject.Find ("Boat Left Edge").transform.position.x) {
			newX = GameObject.Find ("Boat Left Edge").transform.position.x; 
		} else if (transform.position.x > GameObject.Find ("Boat Right Edge").transform.position.x) { 
			newX = GameObject.Find ("Boat Right Edge").transform.position.x; 
		}
		transform.position = new Vector3 (newX, transform.position.y + deltaY);
		lastBoatY = GameObject.Find ("boat").transform.position.y;
	}
	// If the catcher is hit by the UAV, add a rigidbody to the catcher
	private void handleFirstCollision () {
		if (GameController.sfxStatus)	sound.Play();
		if (GetComponent<Rigidbody2D> () == null) {
			gameObject.AddComponent<Rigidbody2D> ();
		}
		GetComponent<SpriteRenderer> ().sortingLayerName = "Foreground";
		GetComponent<SpriteRenderer> ().sortingOrder = 4;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/man_handsup");
		gameObject.AddComponent<BoxCollider2D> (); // To make a splash :)
	}
	// handle the physics reaction of 
	private void handlePhysicalReaction (Collision2D collision) {
		int horizontalForce = collision.collider.transform.position.x > transform.position.x ? -80 : 80;
		int rotation = collision.collider.transform.position.x > transform.position.x ? 30 : -30;
		Vector2 catcherImpulse = new Vector2 (horizontalForce, 80);
		Vector2 uavImpulse = new Vector2 (-horizontalForce, 80);

		GetComponent<Rigidbody2D> ().AddForce (catcherImpulse);
		if (collision.collider.gameObject.name == "uav") {
			GameObject.Find ("uav").GetComponent<Rigidbody2D> ().AddForce (uavImpulse);
		}
		transform.Rotate (0, 0, rotation);
	}
	// initialize the catcher "wobble"
	private void startCatcherMovement () {
		// Initialize rotation as far right
		transform.Rotate (0, 0, -2.5f);
		InvokeRepeating ("rotateLeft", 0, 0.14f);
		InvokeRepeating ("rotateRight", 0.07f, 0.14f);
	}
	private void rotateLeft() {
		transform.Rotate (0, 0, 5);
	}
	private void rotateRight() {
		transform.Rotate (0, 0, -5);
	}
}
