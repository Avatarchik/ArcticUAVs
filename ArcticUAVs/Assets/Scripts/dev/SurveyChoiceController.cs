using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
	This is the SurveyChoiceController class, responsible for allowing the player to answer survey questions and storing
	the answers to those questions.
*/
public class SurveyChoiceController : MonoBehaviour {

/*
	Class Variables 
*/
	private List<string> floatPlayerPrefKeys = new List<string> {
		"Lames", "Okays", "Awesomes", "Not At Alls", "Sort Ofs", "Absolutelys"
	};

/*
	Functions Callable By Buttons 
*/
	// updateStats()
	// Used by:
	//     Done Button in Survey
	public void updateStats() {
		initializePlayerPrefValues ();
		for (int i = 1; i < 4; i++) {
			getAndStoreAnswers (i);
		}
		if (GameObject.Find ("Suggestions").GetComponent<InputField> ().text != "") {
			PlayerPrefs.SetString (
				"Suggestions", 
				PlayerPrefs.GetString ("Suggestions") + GameObject.Find ("Suggestions").GetComponent<InputField> ().text
					+ "\n"
			);
		}
	}
	// incrementPlayCount()
	// Used by:
	//     Left Panel in MissionsMenu
	//     Middle Panel in MissionsMenu
	//     Right Panel in MissionsMenu
	public void incrementPlayCount () {
		if (PlayerPrefs.GetInt ("Demo Mode") == 1) {
			if (!PlayerPrefs.HasKey ("Plays")) {
				PlayerPrefs.SetInt ("Plays", 0);
			}
			PlayerPrefs.SetInt ("Plays", PlayerPrefs.GetInt ("Plays") + 1);
		}
	}
	// onClicked()
	// Used by:
	//     Lame Button in Survey
	//     Okay Button in Survey
	//     Awesome Button in Survey
	//     Not At All Button in Survey
	//     Sort Of Button in Survey
	//     Absolutely Button in Survey
	public void onClicked (Button button) {
		button.GetComponent<Image> ().sprite = Resources.Load ("Sprites/SelectedBubble", typeof(Sprite)) as Sprite;
		unselectOthers (button.gameObject);
	}

/*
	Content and Helper Functions 
*/
	private void getAndStoreAnswers (int idx) {
		GameObject question1Choice = GameObject.Find ("Question 1 Panel").transform.GetChild (idx).gameObject;
		GameObject question2Choice = GameObject.Find ("Question 2 Panel").transform.GetChild (idx).gameObject;
		if (question1Choice.transform.GetChild (0).GetComponent<Image> ().sprite.name == "SelectedBubble") {
			PlayerPrefs.SetFloat (
				question1Choice.transform.GetChild (1).name + "s", 
				PlayerPrefs.GetFloat (question1Choice.transform.GetChild (1).name + "s") + 1
			);
			PlayerPrefs.SetInt ("q1Answers", PlayerPrefs.GetInt ("q1Answers") + 1);
		}
		if (question2Choice.transform.GetChild (0).GetComponent<Image> ().sprite.name == "SelectedBubble") {
			PlayerPrefs.SetFloat (
				question2Choice.transform.GetChild (1).name + "s", 
				PlayerPrefs.GetFloat (question2Choice.transform.GetChild (1).name + "s") + 1
			);
			PlayerPrefs.SetInt ("q2Answers", PlayerPrefs.GetInt ("q2Answers") + 1);
		}
	}

	private void initializePlayerPrefValues () {
		if (!PlayerPrefs.HasKey ("q1Answers")) {
			PlayerPrefs.SetInt ("q1Answers", 0);
		}
		if (!PlayerPrefs.HasKey ("q2Answers")) {
			PlayerPrefs.SetInt ("q2Answers", 0);
		}
		foreach (string key in floatPlayerPrefKeys) {
			if (!PlayerPrefs.HasKey (key)) {
				PlayerPrefs.SetInt (key, 0);
			}
		}
		if (!PlayerPrefs.HasKey ("Suggestions")) {
			PlayerPrefs.SetString ("Suggestions", "");
		}
	}

	private void unselectOthers (GameObject selectedObject) {
		for (int i = 1; i < 4; i++) {
			if (selectedObject.transform.parent.transform.parent.transform.GetChild (i).gameObject.name !=
			    selectedObject.transform.parent.name) {
				selectedObject.transform.parent.transform.parent.transform.GetChild (i).transform.GetChild (0)
					.gameObject.GetComponent<RectTransform> ().GetComponent<Image> ().sprite = 
						Resources.Load ("Sprites/UnselectedBubble", typeof(Sprite)) as Sprite;
			}
		}
	}
}

