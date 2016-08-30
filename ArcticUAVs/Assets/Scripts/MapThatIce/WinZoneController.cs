using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// WinZoneController is responsible for determining when 
// the whalers reach the end of the maze in the MTI missions.
// The win zone sets a local won before the Global won, to allow
// points controller to an extra frame to update the mission score
// before "Ending" calculates and displays the final mission score.
// ###############################################################
public class WinZoneController : MonoBehaviour {

	private Collider winZone;
	private bool won;

	void Start () {
		winZone = GetComponent<BoxCollider> ();
	}

	// Called once per frame
	void Update () {
		// check if local win
		if (won) {
			// if local won is set, set global won
			GameController.message = "YOU MADE IT!";
			GameController.won = true;
			GameController.inPlay = false;
		}
		// if the whalers have reached the win zone
		if (winZone.bounds.Contains (GameObject.Find ("Whaler").transform.position) && !GameController.won) {
			// increase objective gained and set local won
			GameController.objectiveGained += 1;
			won = true;
		}
	}
}
