using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// ###############################################################
// MarkerController is responsible for the placement of markers 
// in the MTI Missions as well as swapping which whaler is the
// leader of the whaler group.
// ###############################################################
public class MarkerController : MonoBehaviour {

// ###############################################################
// Variables
// ###############################################################
	public int validTouchID;
	private float distance;
	private Plane plane = new Plane(Vector3.up, 0);
	private bool setFirstMarkerIndicator = false;
	private bool valid;
	private Touch validTouch;

// ###############################################################
// Unity Functions
// ###############################################################
	// Initialization (called once when gameObject is created).
	void Start () {
		buildMarker ();
	}
	// Called once per frame
	void Update () {
		bool touchEnded = touchHasEnded ();
		updateMarkerSprite ();
		updateMarkerPosition ();
		handleEndingTouch (touchEnded);
	}

// ###############################################################
// Marker Placement Functions
// ###############################################################
	// determine if the touch has ended
	private bool touchHasEnded () {
		if (validTouchID < Input.touchCount) {
			validTouch = Input.GetTouch (validTouchID);
			return false;
		} else {
			return true;
		}
	}
	// update the position of the potential marker to the position of the touch
	private void updateMarkerPosition () {
		Ray ray = Camera.main.ScreenPointToRay(validTouch.position);
		if (plane.Raycast (ray, out distance)) {
			transform.position = ray.GetPoint (distance);
		}
	}
	// change the sprite for if the position of the marker is valid or not
	private void updateMarkerSprite () {
		SpriteRenderer renderer = GetComponent<SpriteRenderer> ();
		if (legalPlacement () && !valid) {
			renderer.sprite = Resources.Load<Sprite> ("Sprites/ValidMarker");
			valid = true;
		} else if (!legalPlacement () && valid) {
			renderer.sprite = Resources.Load<Sprite> ("Sprites/InvalidMarker");
			valid = false;
		}
	}
	// if the touch ends, try placing the marker, or destroy the potential marker
	private void handleEndingTouch (bool endTouch) {
		if (validTouch.phase == TouchPhase.Ended || validTouch.phase == TouchPhase.Canceled || endTouch) {
			if (legalPlacement ()) {
				destroyPreviousMarker ();
				setWhalerLeader ();
				setupMarker ();
			}
			Destroy (gameObject);
		}
	}
	// if a new marker is set, destroy the old one
	private void destroyPreviousMarker () {
		if (GameObject.Find ("Placed Marker") != null) {
			Destroy (GameObject.Find ("Placed Marker"));
			Destroy (GameObject.Find ("Placed Marker Area"));
		}
	}
	// determine if the potential marker is in a legal position
	private bool legalPlacement () {
		Vector3 headPosition = GameObject.Find ("Whaler").transform.position;
		Vector3 tailPosition = GameObject.Find ("Follower 3").transform.position;
		LayerMask mask = -1;
		bool headThroughWall = Physics.Linecast (
			headPosition, transform.position, mask.value, QueryTriggerInteraction.Ignore
		);
		bool tailThroughWall = Physics.Linecast (
			tailPosition, transform.position, mask.value, QueryTriggerInteraction.Ignore
		);
		headPosition.y = -1;
		tailPosition.y = -1;
		bool headThroughPerson = Physics.Linecast (
			headPosition, 
			new Vector3 (transform.position.x, -1, transform.position.z), 
			mask.value, 
			QueryTriggerInteraction.Ignore
		);
		bool tailThroughPerson = Physics.Linecast (
			tailPosition, 
			new Vector3 (transform.position.x, -1, transform.position.z), 
			mask.value, 
			QueryTriggerInteraction.Ignore
		);
		return allFollowersAreValid () && 
			((!headThroughWall && !headThroughPerson) || 
			(!tailThroughWall && !tailThroughPerson));
	}
	// build the marker sprite
	private void buildMarker () {
		validTouch = Input.GetTouch (validTouchID);
		transform.localScale = new Vector3 (2.5f, 2.5f, transform.localScale.z);
		transform.Rotate (new Vector3 (90, 0, 0));
		Ray ray = Camera.main.ScreenPointToRay(validTouch.position);
		if (plane.Raycast (ray, out distance)) {
			transform.position = ray.GetPoint (distance);
		}
		SpriteRenderer renderer = gameObject.AddComponent<SpriteRenderer> ();
		valid = legalPlacement ();
		renderer.sprite = Resources.Load<Sprite> ("Sprites/" + (valid ? "ValidMarker" : "InvalidMarker"));
		renderer.color = new Vector4 (renderer.color.r, renderer.color.g, renderer.color.b, 0.75f);
	}
// ###############################################################
// Marker Setup Functions
// ###############################################################
	// build the actual marker for the whalers to follow 
	private void setupMarker () {
		setupPlacedMarkerFlag ();
		setupPlacedMarkerArea ();
	}
	// render the marker base circle
	private void setupPlacedMarkerArea () {
		GameObject placedMarkerArea = new GameObject (name: "Placed Marker Area");
		placedMarkerArea.transform.Rotate (new Vector3 (90, 0, 0));
		placedMarkerArea.transform.localScale = new Vector3 (1, 1, placedMarkerArea.transform.localScale.z);
		placedMarkerArea.AddComponent<SpriteRenderer> ();
		placedMarkerArea.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/DottedCircle");
		placedMarkerArea.transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
	}
	// render the marker icon for the center of the marker
	private void setupPlacedMarkerFlag () {
		GameObject placedMarker = new GameObject (name: "Placed Marker");
		placedMarker.transform.Rotate (new Vector3 (45, 0, 0));
		placedMarker.transform.localScale = new Vector3 (2, 2, placedMarker.transform.localScale.z);
		placedMarker.AddComponent<SpriteRenderer> ();
		placedMarker.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/PlacedMarker");
		placedMarker.transform.position = new Vector3 (transform.position.x, 1, transform.position.z);
		placedMarker.AddComponent<PlacedMarkerController> ();
	}
// ###############################################################
// Whaler Functions
// ###############################################################
	// check that all the follows have valid paths to follow
	private bool allFollowersAreValid () {
		bool follower1 = GameObject.Find ("Follower 1").GetComponent<FollowingWhalerController>().validMarker;
		bool follower2 = GameObject.Find ("Follower 2").GetComponent<FollowingWhalerController>().validMarker;
		bool follower3 = GameObject.Find ("Follower 3").GetComponent<FollowingWhalerController>().validMarker;
		return follower1 && follower2 && follower3;
	}
	// set which whaler is the leader, the front whaler or the back whaler
	private void setWhalerLeader () {
		LayerMask mask = -1;
		Vector3 leaderPosition = GameObject.Find ("Whaler").transform.position;
		// If leader's path goes through a wall
		if (Physics.Linecast (leaderPosition, transform.position, mask.value, QueryTriggerInteraction.Ignore)) {
			swapLeadWhaler ("Follower 3");
		} else if (Physics.Linecast (// else if leader's path goes through a person
			new Vector3 (leaderPosition.x, -1, leaderPosition.z), 
			new Vector3 (transform.position.x, -1, transform.position.z), mask.value, QueryTriggerInteraction.Ignore
		    )) {
			swapLeadWhaler ("Follower 3");
		}
	}
	// change which whaler is the leader
	private void swapLeadWhaler (string newLeader) {
		// Function variables
		string[] currentFollowOrder = new string[4] {"Whaler", "Follower 1", "Follower 2", "Follower 3"};
		GameObject[] currentFollowObjects = new GameObject[4] {
			GameObject.Find (currentFollowOrder[0]), 
			GameObject.Find (currentFollowOrder[1]),
			GameObject.Find (currentFollowOrder[2]), 
			GameObject.Find (currentFollowOrder[3])};
		string[] newFollowOrder = new string[4];
		bool leaderSet = false;
		int leaderIdx = 0;
		int n, j;

		// Find new whaler leader
		n = 0;
		for (int i = 3; i >= 0; i--) {
			// if leader is found, set the rest of the followers
			if (leaderSet) {
				newFollowOrder [n] = currentFollowOrder [i];
				n++;
			}
			// if new leader is found
			if (currentFollowOrder [i] == newLeader) {
				newFollowOrder [n] = currentFollowOrder [i];
				n++;
				leaderIdx = i;
				leaderSet = true;
			}
		}
		if (!leaderSet) {
			Debug.LogError ("*ERROR* Leader swap failed\nReason: Could not find new leader");
			return;
		}
		// finish filling the follow order
		j = n;
		for (int i = 3; i >= 0+j; i--) {
			newFollowOrder [n] = currentFollowOrder [i];
			n++;
		}

		// Assign corrected names for following order
		currentFollowObjects[0].name = newFollowOrder[0];
		currentFollowObjects[1].name = newFollowOrder[1];
		currentFollowObjects[2].name = newFollowOrder[2];
		currentFollowObjects[3].name = newFollowOrder[3];

		// reassign correct controllers for whaler and followers
		for (int i = 3; i >= 0; i--) {
			// Setup new leader
			if (i == leaderIdx) {
				if (currentFollowObjects [i].GetComponent<FollowingWhalerController> () != null) {
					Destroy (currentFollowObjects [i].GetComponent<FollowingWhalerController> ());
					currentFollowObjects [i].AddComponent<WhalerController> ();
				}
				leaderSet = true;
			} else {
				if (currentFollowObjects [i].GetComponent<WhalerController> () != null) {
					Destroy (currentFollowObjects [i].GetComponent<WhalerController> ());
					currentFollowObjects [i].AddComponent<FollowingWhalerController> ();
				}
			}
		}

		// reset current order, because it's been changed
		currentFollowObjects = new GameObject[4] {GameObject.Find (currentFollowOrder[0]), 
													GameObject.Find (currentFollowOrder[1]),
													GameObject.Find (currentFollowOrder[2]), 
													GameObject.Find (currentFollowOrder[3])};
		// reassign toFollow orders		
		currentFollowObjects[1].GetComponent<FollowingWhalerController> ().toFollow = "Whaler";
		currentFollowObjects[2].GetComponent<FollowingWhalerController> ().toFollow = "Follower 1";
		currentFollowObjects[3].GetComponent<FollowingWhalerController> ().toFollow = "Follower 2";
	}
}
