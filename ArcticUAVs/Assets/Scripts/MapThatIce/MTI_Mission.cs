using UnityEngine;

// ###############################################################
// SC: MTI_Mission
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
// ###############################################################
public class MTI_Mission : MonoBehaviour {
	void Awake() {
		Difficulty.loadMTIDiff();
	}
	void Start () {
		GameController.missionInitials = "MTI";
		GameController.challenge = "Get the whalers to the sea edge!";
	}
}