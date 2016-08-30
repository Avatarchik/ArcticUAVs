using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ###############################################################
// deprecated as of 7/18/2016 [REMOVE?]
// ###############################################################

// Scene controller for deprecated scene trophies
// [FUTURE WORK : update to new trophies workshopped early june 2016]
// new trophies (~11)
	// {improved: beat all tutorials} 
	// {bird strike: 50 strikes}
	// {Catcher: catch 50 UAVs} 
	// {photographer: 50 pics taken} 
	// {albino otter: photo an alibino otter}
	// {bird friend: 10 near misses} 

	// {Beat 'em all: easy peasy, <title>,<title>,Be Insane}
		// {Easy Peasy: beat all 4 on easy}
		// {<title>:beat all on normal} 
		// {<title>: beat all on hard}
		// {be insane: beat all 4 on insane} 

public class TrophiesController : MonoBehaviour {
	private List<string> trophies = new List<string> { 
		"Improved", "Beat 'Em All", "Bird Friend", "Bird Strike", "100,000", 
		"500,000", "1,000,000", "Albino Otter", "Be Insane", "Got 'Em All"
	};
	void Start () {
		foreach (string trophy in trophies) {
			if (PlayerPrefs.GetInt (trophy + " New") == 0) {
				GameObject.Find (trophy).transform.GetChild (2).gameObject.SetActive (false);
			}
			if (PlayerPrefs.GetInt (trophy) == 0) {
				fadeTrophy (trophy);
			} else {
				PlayerPrefs.SetInt (trophy + " New", 0);
			}
		}
	}
	void fadeTrophy (string trophyName) {
		Vector4 fadedColor = new Vector4 (1, 1, 1, 0.25f);
		GameObject panel = GameObject.Find (trophyName);
		panel.GetComponent<Image> ().color = fadedColor;
		panel.transform.GetChild (1).gameObject.GetComponent<Text> ().color = fadedColor;
		GameObject.Find (trophyName).transform.GetChild (2).gameObject.SetActive (false);
	}
}
