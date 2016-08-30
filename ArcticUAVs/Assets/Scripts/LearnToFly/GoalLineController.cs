using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalLineController : MonoBehaviour {
	// Called when UAV touches the collider
	void OnTriggerEnter2D (Collider2D collider) {
		doActionsIndependantOfInstructional ();
		int instructional = GameObject.Find ("Scene Controller").GetComponent<LearnToFlyController> ().instructional - 1;
		if (instructional < 4) {
			handleInstructionalCompleted (instructional);
		} else {
			Destroy (GameObject.Find ("Dotted Circle"));
		}
	}
	private void destroyLeftThumb () {
		Destroy (GameObject.Find ("Goal Line"));
		Destroy (GameObject.Find ("Left Thumb"));
		Destroy (GameObject.Find ("Left Thumb Ripple 1"));
		Destroy (GameObject.Find ("Left Thumb Ripple 2"));
	}
	private void destroyRightThumb () {
		Destroy (GameObject.Find ("Goal Line"));
		Destroy (GameObject.Find ("Right Thumb"));
		Destroy (GameObject.Find ("Right Thumb Ripple 1"));
		Destroy (GameObject.Find ("Right Thumb Ripple 2"));
	}
	private void doActionsIndependantOfInstructional () {
		GameObject.Find ("Scene Controller").GetComponent<LearnToFlyController> ().completedInstructional = true;
		GameObject.Find ("uav").GetComponent<Rigidbody2D> ().isKinematic = true;
	}
	private void handleInstructionalCompleted (int instructional) {
		if (instructional == 1) {
			GameObject.Find ("Scene Controller").GetComponent<LearnToFlyController> ().startedRight = 
				GameObject.Find ("uav").transform.position.x > 0;
		}
		bool startedRight = GameObject.Find ("Scene Controller").GetComponent<LearnToFlyController> ().startedRight;
		if ((startedRight && instructional == 2) || (!startedRight && instructional != 2)) {
			destroyLeftThumb ();
		}
		if ((startedRight && instructional != 2) || (!startedRight && instructional == 2)) {
			destroyRightThumb ();
		}
	}
}
