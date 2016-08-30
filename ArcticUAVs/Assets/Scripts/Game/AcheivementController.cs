using UnityEngine;
using System.Collections;
// ###############################################################
// deprecated as of 7/18/2016 [REMOVE?]
// ###############################################################
public class AcheivementController : MonoBehaviour {

	private GameObject trophyIndicator;
	// Use this for initialization
	void Start () {
		trophyIndicator = GameObject.Find ("Trophy Indicator");

	}
	
	// Update is called once per frame
	void Update () {
		checkForNewTrophies ();
		if (trophyIndicator != null) { 
			trophyIndicator.SetActive (false); 
		}
	}

		private void checkAlbinoOtter (string difficulty) {
		if (GameController.albinoOtter && PlayerPrefs.GetInt ("Albino Otter") == 0) {
			PlayerPrefs.SetInt ("Albino Otter", 1);
			trophyIndicator.SetActive (true);
		}
	}

	private void checkBeatEmAll (string difficulty) {
		if (GameController.won && PlayerPrefs.GetInt ("Beat 'Em All") == 0) {
			if ((GameController.missionInitials == "LITH" &&
				(PlayerPrefs.GetInt ("GTPBasic") == 1 ||
					PlayerPrefs.GetInt ("GTPEasy") == 1 ||
					PlayerPrefs.GetInt ("GTPNormal") == 1 ||
					PlayerPrefs.GetInt ("GTPHard") == 1 ||
					PlayerPrefs.GetInt ("GTPInsane") == 1) &&
				(PlayerPrefs.GetInt ("MTIBasic") == 1 ||
					PlayerPrefs.GetInt ("MTIEasy") == 1 ||
					PlayerPrefs.GetInt ("MTINormal") == 1 ||
					PlayerPrefs.GetInt ("MTIHard") == 1 ||
					PlayerPrefs.GetInt ("MTIInsane") == 1)) ||
				(GameController.missionInitials == "GTP" &&
					(PlayerPrefs.GetInt ("LITHBasic") == 1 ||
						PlayerPrefs.GetInt ("LITHEasy") == 1 ||
						PlayerPrefs.GetInt ("LITHNormal") == 1 ||
						PlayerPrefs.GetInt ("LITHHard") == 1 ||
						PlayerPrefs.GetInt ("LITHInsane") == 1) &&
					(PlayerPrefs.GetInt ("MTIBasic") == 1 ||
						PlayerPrefs.GetInt ("MTIEasy") == 1 ||
						PlayerPrefs.GetInt ("MTINormal") == 1 ||
						PlayerPrefs.GetInt ("MTIHard") == 1 ||
						PlayerPrefs.GetInt ("MTIInsane") == 1)) ||
				(GameController.missionInitials == "MTI" &&
					(PlayerPrefs.GetInt ("LITHBasic") == 1 ||
						PlayerPrefs.GetInt ("LITHEasy") == 1 ||
						PlayerPrefs.GetInt ("LITHNormal") == 1 ||
						PlayerPrefs.GetInt ("LITHHard") == 1 ||
						PlayerPrefs.GetInt ("LITHInsane") == 1) &&
					(PlayerPrefs.GetInt ("GTPBasic") == 1 ||
						PlayerPrefs.GetInt ("GTPEasy") == 1 ||
						PlayerPrefs.GetInt ("GTPNormal") == 1 ||
						PlayerPrefs.GetInt ("GTPHard") == 1 ||
						PlayerPrefs.GetInt ("GTPInsane") == 1))) {
				PlayerPrefs.SetInt ("Beat 'Em All", 1);
				trophyIndicator.SetActive (true);
			}
		}
	}

	private void checkBeInsane (string difficulty) {
		if (GameController.won && PlayerPrefs.GetInt ("Be Insane") == 0 && difficulty == "Insane") {
			if ((GameController.missionInitials == "LITH" &&
				PlayerPrefs.GetInt ("GTPInsane") == 1 &&
				PlayerPrefs.GetInt ("MTIInsane") == 1) ||
				(GameController.missionInitials == "GTP" &&
					PlayerPrefs.GetInt ("LITHInsane") == 1 &&
					PlayerPrefs.GetInt ("MTIInsane") == 1) ||
				(GameController.missionInitials == "MTI" &&
					PlayerPrefs.GetInt ("LITHInsane") == 1 &&
					PlayerPrefs.GetInt ("GTPInsane") == 1)) {
				PlayerPrefs.SetInt ("Be Insane", 1);
				trophyIndicator.SetActive (true);
			}
		}
	}

	private void checkBirdFriend (string difficulty) {
		if (GameController.birdsMissed >= 10 && PlayerPrefs.GetInt ("Bird Friend") == 0) {
			PlayerPrefs.SetInt ("Bird Friend", 1);
			trophyIndicator.SetActive (true);
		}
	}

	private void checkBirdStrike (string difficulty) {
		if (GameController.birdsHit >= 3 && PlayerPrefs.GetInt ("Bird Strike") == 0) {
			PlayerPrefs.SetInt ("Bird Strike", 1);
			trophyIndicator.SetActive (true);
		}
	}

	private void checkForNewTrophies () {
		if (GameObject.Find ("Points Controller") != null) {
			string difficulty = PlayerPrefs.GetString ("Difficulty");
			checkBeatEmAll (difficulty);
			checkBirdFriend (difficulty);
			checkBirdStrike (difficulty);
			checkAlbinoOtter (difficulty);
			checkBeInsane (difficulty);
			checkGotEmAll ();

			// check100000 (difficulty);
			// check500000 (difficulty);
			// check1000000 (difficulty);
			// checkImproved (difficulty);
		}
	}



	private void checkGotEmAll () {
		if (PlayerPrefs.GetInt ("Improved") == 1 &&
			PlayerPrefs.GetInt ("Beat 'Em All") == 1 &&
			PlayerPrefs.GetInt ("Bird Friend") == 1 &&
			PlayerPrefs.GetInt ("Bird Strike") == 1 &&
			PlayerPrefs.GetInt ("1,000,000") == 1 &&
			PlayerPrefs.GetInt ("Albino Otter") == 1 &&
			PlayerPrefs.GetInt ("Be Insane") == 1 &&
			PlayerPrefs.GetInt ("Got 'Em All") == 0) {
			PlayerPrefs.SetInt ("Got 'Em All", 1);
			trophyIndicator.SetActive (true);
		}
	}
}