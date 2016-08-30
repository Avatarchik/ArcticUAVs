using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// ###############################################################
// This is the CameraController class, responsible for handling the movement of the camera in any scene in which it is 
// used.  Depending on the values set in the Inspector, the camera will follow the UAV or lock in place when appropriate.
// If the UAV leaves the set boundary (goes off screen), the game is ended as a loss.
// ###############################################################
public class CameraController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	public bool freeX;
	public bool freeY;
	public bool freeZ;
	public bool noXMax;
	public bool noXMin;
	public bool noYMax;
	public bool noYMin;
	public bool noZMax;
	public bool noZMin;
	public GameObject toFollow;
	public float xMax;
	public float xMin;
	public float yMax;
	public float yMin;
	public float zMax;
	public float zMin;
// ###############################################################
// Unity Functions
// ###############################################################
	void Update () {
		Vector3 coordinates = transform.position;
		if (toFollow) {
			coordinates.x = determineNewX ();
			coordinates.y = determineNewY ();
			coordinates.z = determineNewZ ();
		}
		transform.position = coordinates;
	}
	// Called when the UAV exits the boundary
	void OnTriggerStay2D (Collider2D other) {
		// Show failing message
		if (!GameController.lost) {
			GameController.message = "UAV OUT OF RANGE!";
			GameController.lost = true;
			GameController.inPlay = false;
		}
	}
// ###############################################################
// Camera Functions
// ###############################################################
	private float determineNewX () {
		if (freeX) {
			bool notAtMin = true;
			bool notAtMax = true;
			// Check (if there is a min) if the object to follow is past it
			if (!noXMin && toFollow.transform.position.x < xMin) { 
				notAtMin = false; 
			}
			// Check (if there is a max) if the object to follow is past it
			if (!noXMax && toFollow.transform.position.x > xMax) { 
				notAtMax = false; 
			}
			// If not past either...
			if (notAtMin && notAtMax) { 
				return toFollow.transform.position.x; // Move the camera
			}
		}
		return transform.position.x; // Don't move the camera, it's locked in X
	}
	// ***************************************************************
	private float determineNewY () {
		if (freeY) {
			bool notAtMin = true;
			bool notAtMax = true;
			// Check (if there is a min) if the object to follow is past it
			if (!noYMin && toFollow.transform.position.y < yMin) { 
				notAtMin = false; 
			}
			// Check (if there is a max) if the object to follow is past it
			if (!noYMax && toFollow.transform.position.y > yMax) { 
				notAtMax = false; 
			}
			// If not past either...
			if (notAtMin && notAtMax) { 
				return toFollow.transform.position.y; // Move the camera
			}
		}
		return transform.position.y; // Don't move the camera, it's locked in Y
	}
	// ***************************************************************
	private float determineNewZ () {
		if (freeZ) {
			bool notAtMin = true;
			bool notAtMax = true;
			// Check (if there is a min) if the object to follow is past it
			if (!noZMin && toFollow.transform.position.z < zMin) { 
				notAtMin = false; 
			}
			// Check (if there is a max) if the object to follow is past it
			if (!noZMax && toFollow.transform.position.z > zMax) { 
				notAtMax = false; 
			}
			// If not past either...
			if (notAtMin && notAtMax) { 
				return toFollow.transform.position.z; // Move the camera
			}
		}
		return transform.position.z; // Don't move the camera, it's locked in Z
	}
}
