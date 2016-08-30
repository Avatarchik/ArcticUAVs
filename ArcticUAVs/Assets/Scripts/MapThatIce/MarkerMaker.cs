using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// ###############################################################
// MarkerMaker is responsible for the creation of new markers 
// after a long touch in the MTI Missions.
// ###############################################################
public class MarkerMaker : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	private bool assigned = false;
	private Vector3 joyMax;
	private Vector3 joyMin;
	private int joyStickTouchID;
	private bool touched = false;
	private float touchTime;
	private int validTouchID;

// ###############################################################
// Functions 
// ###############################################################
	// Initialization (called once when gameObject is created).
	void Start () {
		Vector3[] bounds = new Vector3[4];
		GameObject.Find ("Joystick").GetComponent<RectTransform>().GetWorldCorners(bounds);
		joyMin = bounds[0];
		joyMax = bounds[2];
    }
	// Called once per frame
	void Update () {
		joyStickTouchID = -1;
		findTouches ();
	}
	// detect touches on the screen
	private void findTouches () {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				handleNewTouch (touch);
			}
			if (touched && (Time.time - touchTime) > 0.2) {
				handleLongTouch (touch);
			}
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) {
				touched = false;
				assigned = false;
			}
		}
	}
	// check if the touch is in the joystick region or not
	private void handleNewTouch (Touch touch) {
		bool outJoyStick = !((touch.position.x > joyMin.x && touch.position.x < joyMax.x) && (touch.position.y > joyMin.y && touch.position.y < joyMax.y));
		
		if (outJoyStick) {
			validTouchID = touch.fingerId;
			if (GameController.inPlay) {
				touchTime = Time.time;
				touched = true;
			}
		} else {
			joyStickTouchID = touch.fingerId;
		}
	}
	// if the touch is a long press, Place a marker at the finger position
	private void handleLongTouch (Touch touch) {
		if (!assigned) {
			if (joyStickTouchID != touch.fingerId) {
				GameObject potentialMarker = new GameObject (name: "Potential Marker");
				MarkerController newMarker = potentialMarker.AddComponent<MarkerController> ();
				newMarker.validTouchID = validTouchID;
				assigned = true;
			}
		}
	}

}
