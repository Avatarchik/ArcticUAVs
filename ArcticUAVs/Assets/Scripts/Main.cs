using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ###############################################################
// SC: Main
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// Main pulls player prefs from storage, removes player prefs from older versions of the app, sets up music
// ###############################################################
public class Main : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private List<string> missionsInitials = new List<string> { "LTF", "LITH", "GTP", "MTI" };
	private Dictionary<string, int> intInitialValues = new Dictionary<string, int> {
		{"Bird Friend", 0},
		{"Bird Strike", 0},
		{"music pref", 1},
		{"sfx pref", 1}
	};
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// Debug effort
		Application.targetFrameRate = 60;
		// Set up app settings
		setOrientation ("landscape");
		Input.multiTouchEnabled = true;
		Input.simulateMouseWithTouches = true;
		// Set up player prefs on first run
		initializePlayerPrefsValues ();
	}
// ###############################################################
// UI Function
// ###############################################################
	private void setOrientation (string orientation) {
		if (orientation == "portrait") {
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
		} else if (orientation == "landscape") {
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
		}
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
// ###############################################################
// Player Pref functions
// ###############################################################
	private void clearPlayerPrefs() {
		print("Deleting Playprefs");
		PlayerPrefs.DeleteAll();
		PlayerPrefs.SetInt ("PlayerPrefsVersion", 2);
	}
	// ***************************************************************
	private void initializePlayerPrefsValues () {
		// Check playerPrefs Version
		if (PlayerPrefs.HasKey ("PlayerPrefsVersion")) {
			// Increment version number as playerprefs change
			if (PlayerPrefs.GetInt ("PlayerPrefsVersion") != 2) clearPlayerPrefs();
		} else {
			clearPlayerPrefs();
		}
		// Init sound control bools	
		if (PlayerPrefs.HasKey("sfx pref")){
			GameController.sfxStatus = PlayerPrefs.GetInt("sfx pref") == 1;
		} else {
			PlayerPrefs.SetInt("sfx pref", 1);
			GameController.sfxStatus = PlayerPrefs.GetInt("sfx pref") == 1;
		}
		// Init sound control bools	
		if (PlayerPrefs.HasKey("music pref")){
			GameController.musicStatus = PlayerPrefs.GetInt("music pref") == 1;
		} else {
			PlayerPrefs.SetInt("music pref", 1);
			GameController.musicStatus = PlayerPrefs.GetInt("music pref") == 1;
		}
	}
}