using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// ###############################################################
// ScoutController is responsible for handling the controls of the
// UAV in the LTF, LITH, and GTP missions.  It also holds the
// messages for the failed endgames involving the UAV.
// ###############################################################
public class ScoutController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Static Lost messages
	public static string birdStrike = "Bird Strike!";
	public static string lightningStrike = "UAV Shocked!";
	public static string drowned = "UAV Drowned!";
	public static string deadBattery = "Battery Drained!";
	public static string crashed = "UAV Crashed!";
	public static string lostUAV = "UAV Out Of Range!";

	// set in Editor
	public bool clampToSides;
	public float speedLimit;

	private GameObject leftEdge;
	private GameObject topEdge;
	private GameObject rightEdge;
	private float angle = 0;
	private Vector3 screenDimensions;
	private Rigidbody2D uavBody;
	private float upForce;

	// sound stuff
	public static AudioSource propeller;
	private AudioSource crash;
	private bool playing = false;

	private PolygonCollider2D uavCollider;
	private BoxCollider2D landing;

	private GameObject catcher;

// ###############################################################
// Unity Functions 
// ###############################################################
	// Initialization (called once when gameObject created).
	void Start () {
		GetComponent<Rigidbody2D> ().interpolation = RigidbodyInterpolation2D.Interpolate;
		uavBody = transform.GetComponent<Rigidbody2D> ();
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
	
		// creating the UAV's propeller
		AudioClip soundClip = (AudioClip)Resources.Load("Sounds/plane-propeller", typeof(AudioClip));
		propeller = gameObject.AddComponent<AudioSource> ();
		propeller.clip = soundClip;
		propeller.volume = .1f;
		propeller.loop = true;
	
		// creating the UAV's crash sound
		soundClip = (AudioClip)Resources.Load("Sounds/uav-crash", typeof(AudioClip));
		crash = gameObject.AddComponent<AudioSource> ();
		crash.clip = soundClip;
		crash.volume = .3f;
	
		// Create clamping
		if (Difficulty.clamping) {
			print("clamping");
			leftEdge = new GameObject();
			leftEdge.name = "leftEdge";
			leftEdge.AddComponent<BoxCollider2D>();
			leftEdge.transform.position = new Vector2 (-screenDimensions.x, 0f);
			leftEdge.transform.localScale = new Vector3 (0.25f,2f*screenDimensions.y,1f);
	
			topEdge = new GameObject();
			topEdge.name = "topEdge";
			topEdge.AddComponent<BoxCollider2D>();
			topEdge.transform.position = new Vector2 (0f, screenDimensions.y);
			topEdge.transform.localScale = new Vector3 (2*screenDimensions.x,0.25f,1f);
	
			rightEdge = new GameObject();
			rightEdge.name = "rightEdge";
			rightEdge.AddComponent<BoxCollider2D>();
			rightEdge.transform.position = new Vector2 (screenDimensions.x, 0f);
			rightEdge.transform.localScale = new Vector3 (0.25f,2f*screenDimensions.y,1f);
		}
	
		//connecting to the UAV's PolygonCollider2D
		//connecting to the landing BoxCollider2D
		//We use a try because LandInTheHand* doesn't have a "landing" element
		try {
		uavCollider = transform.GetComponent<PolygonCollider2D> ();
		landing = GameObject.Find("landing").GetComponent<BoxCollider2D> ();}
		catch (Exception e){}
	
		catcher = GameObject.Find("catcher");
	}
	// ********************************************************************************************
	// Called on collision
	void OnCollisionEnter2D (Collision2D collision) {
		// If you hit something
		// and it's the landing
		if (collision.collider.gameObject.name == "landing") {
			GameController.landed = true;
		// else if you hit clamping colliders
		} else if (collision.collider.gameObject.name.Contains("Edge")) {
			if (clampToSides) print("Collided with the edges.");
		// else you've crashed into something
		} else {
			if (collision.collider.gameObject.name.Contains("bird")) {
				GameController.message = birdStrike;
			} else {
				GameController.message = crashed;
			}
			GameController.lost = true;
			GameController.inPlay = false;
			propeller.Stop();
			crash.pitch = UnityEngine.Random.Range(0.4f,1.5f);
			if (GameController.sfxStatus) crash.Play();
			if (collision.collider.gameObject.name == "boat") {
				GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, 150));
			}
		}
	}
	// ********************************************************************************************
	// Called once per physics time step
	void FixedUpdate () {
		// Handle Land in the hand special cases
		bool caught = false;
		if (catcher != null) {
			caught = catcher.GetComponent<CatcherController>().uavCatch;
			if (caught) GetComponent<Rigidbody2D>().isKinematic = true;
		}
		
		if (GameController.inPlay) {
			if (!caught) GetComponent<Rigidbody2D>().isKinematic = false;
			if (uavCollider.IsTouching(landing)) {
				propeller.Pause();
				GameController.landed = true; //you landed
				playing = false;
			} else {
				if (!playing) {
					if (GameController.sfxStatus) propeller.Play ();
					playing = true; //prop is now playing
					GameController.landed = false; //you took off
				}
			}
				handleControls ();
	
		} 
		if (!GameController.lost && !GameController.inPlay) {
			GetComponent<Rigidbody2D>().isKinematic = true;
			propeller.Pause();
			playing = false;
		}
	
		//If the game ended, stop the propeller
		if (GameController.lost || GameController.won) {
			propeller.Stop();
		}
	}

// ###############################################################
// ScoutController Functions 
// ###############################################################
	private void handleControls () {
		if (GameController.inPlay) {
				if (Input.GetMouseButton (0)) {
					GameController.landed = false;
					Vector3 rawPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					determineAngle (rawPosition);
					doSpeedLimiting (transform.GetComponent<Rigidbody2D> ().velocity);
					applyForce (rawPosition);
				}
		}
	}
	// ********************************************************************************************
	private void determineAngle (Vector3 rawPosition) {
		if (rawPosition.x < transform.position.x - 0.5f) {
			if (angle < 28) {
				angle = angle + 4f;
			} else if (angle > 28) {
				angle = angle - 4f;
			}
		} else if (rawPosition.x > transform.position.x + 0.5f) {
			if (angle > -28) {
				angle = angle - 4f;
			} else if (angle < -28) {
				angle = angle + 4f;
			}
		} else { 
			if (angle < 0) {
				angle = angle + 4f;
			} else if (angle > 0) {
				angle = angle - 4f;
			}
		}
	}
	// ********************************************************************************************
	private void doSpeedLimiting (Vector3 velocity) {
		speedLimit = 4f;
		if (velocity.x > speedLimit) {
			uavBody.velocity = new Vector2 (speedLimit, velocity.y);
		} else if (velocity.x < -speedLimit) {
			uavBody.velocity = new Vector2 (-speedLimit, velocity.y);
		}
		if (velocity.y > speedLimit) {
			uavBody.velocity = new Vector2 (velocity.x, speedLimit);
		}
	}
	// ********************************************************************************************
	private void applyForce (Vector3 rawPosition) {
		uavBody.MoveRotation (angle);
		Vector2 targetV = new Vector2 (
			rawPosition.x - transform.position.x, (rawPosition.y - transform.position.y) + (9.8f * 0.3f)
		);
		// Debug line in scene view
		// Debug.DrawLine(transform.position, rawPosition, Color.blue);
		uavBody.AddForce (targetV);
		uavBody.angularVelocity = 0f;
	}
}
