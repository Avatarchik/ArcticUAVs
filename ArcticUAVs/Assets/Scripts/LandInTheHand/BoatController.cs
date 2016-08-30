using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// This is the BoatController, responsible for the movement of the 
// boat in the LITH Missions. The boat's y position is simply 
// updated to that of the middle waves while it follows a path in X.
// ###############################################################
public class BoatController : MonoBehaviour {

// ###############################################################
// Class Variables 
// ###############################################################
	public float maxDistanceToGoal = .1f;
	public PathDefinition path;
	public float speed;
	private IEnumerator<Transform> currentPoint;
	private float lastWaveY;
	private float screenWidth;

// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		// find the position of the middle wave's y position
		lastWaveY = GameObject.Find ("Middle Waves").transform.position.y;
		// set boat speed
		speed = Difficulty.boatSpeed;

		Vector3 rightBorder = new Vector3 (Screen.width, 0.0f, 0.0f);

		screenWidth = (Camera.main.ScreenToWorldPoint (rightBorder)).x;
		// set up the path point to move the boat around
		currentPoint = path.getPathEnumerator ();
		currentPoint.MoveNext ();
		if (currentPoint.Current == null) { 
			return; 
		}
		transform.position = currentPoint.Current.position;
	}

	void Update () {
		handleYPosition ();
		followXPath ();
	}
// ###############################################################
// Boat Controller Functions
// ###############################################################
	// move the boat along the y positions of the follow path
	private void handleYPosition () {
		var deltaY = GameObject.Find ("Middle Waves").transform.position.y - lastWaveY;
		lastWaveY = GameObject.Find ("Middle Waves").transform.position.y;
		transform.position = new Vector3(transform.position.x, transform.position.y + deltaY);
	}
	// move the boat along the x positions of the follow path
	private void followXPath () {
		if (currentPoint == null || currentPoint.Current == null) { 
			return; 
		}
		transform.position = Vector3.MoveTowards (
			transform.position, currentPoint.Current.position, Time.deltaTime * speed
		);
		var distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
		if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal) { 
			handleAtTarget (); 
		}
	}
	// if the boat has reached the target, get the next target
	private void handleAtTarget () {
		var lastXValue = currentPoint.Current.position.x;
		currentPoint.MoveNext ();
		if (currentPoint.Current.position.x > lastXValue) {
			currentPoint.Current.position = new Vector3 (
				Random.Range (lastXValue, screenWidth / 2), currentPoint.Current.position.y
			);
		} else if (currentPoint.Current.position.x < lastXValue) {
			currentPoint.Current.position = new Vector3 (
				Random.Range (-screenWidth / 2, currentPoint.Current.position.x), currentPoint.Current.position.y
			);
		}
	}
}
