using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// This is the FollowPath class.  A GameObject with this script, after providing a PathDefinition, will move along the
// path from point to point.  When the final point is reached it follows the path again in reverse order and so on.  This
// is used by the boat in the LandInTheHand scenes as well as the waves in the LandInTheHand and GetThosePics scenes.
// ###############################################################
public class FollowPath : MonoBehaviour {

	public enum FollowType {
		MoveToward,
		Lerp
	}

// ###############################################################
// Variables 
// ###############################################################
	public IEnumerator<Transform> currentPoint;
	public float maxDistanceToGoal = .1f;
	public PathDefinition path;
	public float speed = 1f;
	public FollowType type = FollowType.MoveToward;

// ###############################################################
// Functions 
// ###############################################################
	// Initialization (called once when gameObject is created).
	void Start() {
		currentPoint = path.getPathEnumerator ();
		currentPoint.MoveNext ();
		transform.position = currentPoint.Current.position;
	}
	// Called once per frame.
	void Update() {
		if (type == FollowType.MoveToward) {
			transform.position = Vector3.MoveTowards (
				transform.position, currentPoint.Current.position, Time.deltaTime * speed
			);
		} else if (type == FollowType.Lerp) {
			transform.position = Vector3.Lerp (
				transform.position, currentPoint.Current.position, Time.deltaTime * speed
			);
		}
		float distanceSquared = (transform.position - currentPoint.Current.position).sqrMagnitude;
		if (distanceSquared < maxDistanceToGoal * maxDistanceToGoal) { 
			currentPoint.MoveNext (); 
		}
	}
}
