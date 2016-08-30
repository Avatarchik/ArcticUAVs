using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// ###############################################################
// PolarBearController is resposible for the movement of the
// polar bear in the MTI missions.
// The polar bear's movements are chosen at random via the 
// Random function.
// ###############################################################
public class PolarBearController : MonoBehaviour {

// ###############################################################
// Variable
// ###############################################################
	private List<string> sprites = new List<string> { "polarBearBody", "polarBearLeftFoot", "polarBearBody", "polarBearRightFoot" };
	private float timePerAnimation = 0.5f;
	private float timeOnAnimation = 0;
	private bool lookedLeft = false;
	private bool lookedRight = false;
	private bool lookedCenter = false;
	private int currentIdx = 0;
	private Vector3 target;
	private Vector3 forward;
	private Vector3 NORTH;
	private Vector3 SOUTH;
	private Vector3 EAST;
	private Vector3 WEST;
	private List<Vector3> directions;
	private LayerMask mask = -1;

// ###############################################################
// Unity Functions
// ###############################################################
	// Use this for initialization
	void Start () {
		float scale = 6;
		NORTH = new Vector3(0, 0, scale);
		SOUTH = new Vector3(0, 0, -scale);
		EAST = new Vector3(scale, 0, 0);
		WEST = new Vector3(-scale, 0, 0);
		forward = NORTH;

		directions = new List<Vector3>{ NORTH, SOUTH, EAST, WEST };

		decidePath (); // Start the cycle
	}

	void Update () {
		// check if polar bear is close.
		if (GameController.inPlay) {
			List<Vector3> whalerPositions = new List<Vector3> () { GameObject.Find ("Whaler").transform.position, 
				GameObject.Find ("Follower 1").transform.position, 
				GameObject.Find ("Follower 2").transform.position, 
				GameObject.Find ("Follower 3").transform.position
			};
			int scale = 6;
			foreach (Vector3 position in whalerPositions) {
				if (Vector3.Distance (transform.position, position) < scale*1.5) {
					GameController.message = "POLAR BEAR SCARE!";
					GameController.lost = true;
				}
			}
		}
	}

// ###############################################################
// PolarBearController Functions
// ###############################################################
	// this function decides and sets the next direction for the
	// polar baer to walk towards
	void decidePath() {
		// check if the path forward is blocked (i.e. by an ice block in the maze)
		bool blocked = Physics.Linecast (transform.position, transform.position + forward, mask.value);

		// start building a list of valid directions to choose from
		List<Vector3> validTargets = new List<Vector3> ();

		// check available directions except directly behind the polar bear
		foreach (Vector3 direction in directions) {
			Vector3 newTarget = transform.position + direction;
			if (!Physics.Linecast (transform.position, newTarget, mask.value) && (newTarget != transform.position - forward)) {
				validTargets.Add (newTarget);
			}
		}

		// if no other directions are available, go backwards.
		if (validTargets.Count < 1) {
			target = transform.position - forward;
			forward = -forward;
		// else pick a random available direction
		} else {
			int idx = Random.Range (0, validTargets.Count);
			target = validTargets [idx];
			forward = target - transform.position;
		}

		// if the path ahead is blocked, start the head movement animation
		if (blocked) {
			StartCoroutine (lookAnimation ());
		// else, move to target direction
		} else {
			StartCoroutine (moveToTarget ());
		}
	}

	// move the polar bear towards the target
	IEnumerator moveToTarget() {
		// if the game is in play, and the path ahead is not blocked
		if (GameController.inPlay && !Physics.Linecast (transform.position, target, mask.value)) {
			// center head as the polarbear starts moving
			if (!lookedCenter) {
				transform.GetChild (0).transform.localEulerAngles = new Vector3 (0, 0, transform.GetChild (0).transform.localEulerAngles.z - 75 * Time.deltaTime);
				lookedCenter = (transform.GetChild (0).transform.localEulerAngles.z > 0 && transform.GetChild (0).transform.localEulerAngles.z < 5) ||
				(transform.GetChild (0).transform.localEulerAngles.z > 355 && transform.GetChild (0).transform.localEulerAngles.z < 360);
			} else {
				transform.GetChild (0).transform.localEulerAngles = new Vector3 (0, 0, 0);
			}

			// if time on animation
			timeOnAnimation += Time.deltaTime;
			if (timeOnAnimation > timePerAnimation) {
				timeOnAnimation = 0;
				currentIdx++;
				if (currentIdx == 4)
					currentIdx = 0;
			}
			// change the body sprite to animate walking
			GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + sprites [currentIdx]);
			// change the head position based on the different body sprites
			if (currentIdx == 0 || currentIdx == 2)
				transform.GetChild (0).transform.localPosition = new Vector3 (0.27f,-3.5f,0);
			else if (currentIdx == 1)
				transform.GetChild (0).transform.localPosition = new Vector3 (-0.17f,-3.5f,0);
			else
				transform.GetChild (0).transform.localPosition = new Vector3 (0.44f,-3.5f,0);
				
			// if the polar bear is not at the target
			// move the polar bear towards the target location
			if (transform.position != target) {
				// rotate towards the target
				var newRotation = Quaternion.LookRotation (target - transform.position).eulerAngles;
				newRotation.x = Mathf.Clamp (-90, -90, -90);
				newRotation.z = 0;
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 2);
				// move towards the target
				transform.position = Vector3.MoveTowards (transform.position, target, 3f * Time.deltaTime);
				yield return new WaitForEndOfFrame ();
				StartCoroutine (moveToTarget ());
			// else, the polar bear has reached the target
			// and needs to decide a new path
			} else {
				decidePath ();
			}

		// check for blocked path
		} else if (GameController.inPlay && Physics.Linecast (transform.position, target, mask.value)) {
			if (!lookedCenter) {
				transform.GetChild (0).transform.localEulerAngles = new Vector3 (0, 0, transform.GetChild (0).transform.localEulerAngles.z - 75 * Time.deltaTime);
				lookedCenter = (transform.GetChild (0).transform.localEulerAngles.z > 0 && transform.GetChild (0).transform.localEulerAngles.z < 5) ||
				(transform.GetChild (0).transform.localEulerAngles.z > 355 && transform.GetChild (0).transform.localEulerAngles.z < 360);
			} else {
				transform.GetChild (0).transform.localEulerAngles = new Vector3 (0, 0, 0);
			}
			yield return new WaitForEndOfFrame ();
			StartCoroutine (moveToTarget ());
		} else {
			yield return new WaitForEndOfFrame ();
			StartCoroutine (moveToTarget ());
		}
	}

	// animate the polar bear's head to show it is deciding the path to follow
	IEnumerator lookAnimation() {
		if (GameController.inPlay) {
			lookedCenter = false;
			if (!lookedLeft) {
				// adjust head angle
				transform.GetChild (0).transform.localEulerAngles = new Vector3 (0, 0, transform.GetChild (0).transform.localEulerAngles.z - 75 * Time.deltaTime);
				// check if head is looking totally left
				lookedLeft = transform.GetChild (0).transform.localEulerAngles.z < 290 && transform.GetChild (0).transform.localEulerAngles.z > 280;
				yield return new WaitForEndOfFrame ();
				StartCoroutine (lookAnimation ());
			} else if (!lookedRight) {
				transform.GetChild (0).transform.localEulerAngles = new Vector3 (0, 0, transform.GetChild (0).transform.localEulerAngles.z + 75 * Time.deltaTime);
				lookedRight = transform.GetChild (0).transform.localEulerAngles.z > 60 && transform.GetChild (0).transform.localEulerAngles.z < 70;
				yield return new WaitForEndOfFrame ();
				StartCoroutine (lookAnimation ());
			} else {
				lookedLeft = false;
				lookedRight = false;
				StartCoroutine (moveToTarget ());
			}
		} else {
			yield return new WaitForEndOfFrame ();
			StartCoroutine (lookAnimation ());
		}
	}
}