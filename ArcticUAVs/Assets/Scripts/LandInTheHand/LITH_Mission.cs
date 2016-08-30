using UnityEngine;
using UnityEngine.UI;
// ###############################################################
// SC: LITH_Mission
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// LITH Sets the UAV as not landed, and freezes it in the air for the opening
// ###############################################################
public class LITH_Mission : MonoBehaviour {
	// Initialization (called once when gameObject is created).
	void Start () {
		// Default Start Settings
		GameController.challenge = "Land all the UAVs!";
		GameController.missionInitials = "LITH";

		Difficulty.loadLITHDiff();

		// UAV defaults
		GameObject.Find("uav").GetComponent<Rigidbody2D> ().isKinematic = true;
		GameController.landed = false;
		// Set UI
		GameObject.Find ("UAVs Needed").GetComponent<Text> ().text = Difficulty.objectiveNeeded.ToString();
	}
	void Update () {
		if (GameController.objectiveGained == Difficulty.objectiveNeeded && !GameController.lost) {
			GameController.message = "UAVs ACQUIRED!";
			GameController.won = true;
		}
		GameObject.Find ("UAVs Caught").GetComponent<Text> ().text = GameController.objectiveGained.ToString ();
	}
}