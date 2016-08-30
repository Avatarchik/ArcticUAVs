using UnityEngine;
using System.Collections;
// ###############################################################
// ScaredZone controller tells the otter when the UAV hovers too low
// ###############################################################
public class ScaredController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Connection to this scared zone's Otter
	private OtterController thisOtter;
	private SpriteRenderer scaredZone;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// OtterGenerator has a dictionary of scared zones to otters
		thisOtter = OtterGenerator.scaredZoneToOtterController[gameObject];
		// Connect to the SpriteRenderer so we can set the alpha = 0 on higher diffs
		scaredZone = gameObject.GetComponent<SpriteRenderer>();
		setAlphaToDiff ();
	}
	void Update () {
		// if our otter got it's picture taken, we can destroy the scared zone.
		if (thisOtter.picTaken) Destroy(gameObject);
	}
	// When the uav enters the scared zone, tell the otter it got scared (Starts SendOtterToTheDeeps animation)
	void OnTriggerEnter2D (Collider2D collider) {
		thisOtter.scared = true;
	}
// ###############################################################
// Scared Contoller Functions
// ###############################################################
	void setAlphaToDiff () {
		if (GameController.missionDiff == "Tutorial" || GameController.missionDiff == "Easy") {
			scaredZone.color = new Vector4 (1f,1f,1f,75f/255f);
		}
	}
}