using UnityEngine;
using System.Collections;
// ###############################################################
// Tutorial controller handles tutorials. Attach this script to the parent of the
// tutorial prefab, and name the children "Tutorial Popup #". The default is to 
// iterate over all the children once GC.started is true. 
//
// Alternitively you can connect to the instance and automaticTutorialSetting(False)
// and manually trigger the popups via manualTutorial(#) for "Tutorial Popup #"
// ###############################################################
public class TutorialController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private int tutorialCount = 1;
	private int startChildCount;
	private bool automaticTutorial = true;
	private GameObject currentPopup;
// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		// We handle the UI adjusting here so that the function call can be "Instantiate()"
		transform.parent = GameObject.Find("Canvas").transform;
		GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);
		// Need to know how many popups there are
		startChildCount = transform.childCount;
		// hide all of the children
		for (int i = 0; i < transform.childCount; i++) {
			// collect popup
			// store it
			// hide it
			transform.GetChild (i).gameObject.SetActive (false);
		}
	}
	void Update () {
		// if we are automatic, control the tutorial
		if (automaticTutorial) controlTutorial ();
		// Hide ourselves if the pause menu is displayed
		if (GameObject.Find("PauseMenu(Clone)")) {
			if (transform.Find("Current Popup")) currentPopup.SetActive(false);
		// Show ourselves if the pause menu is not displaying
		} else {
			if (transform.Find("Current Popup")) currentPopup.SetActive(true);
		}
	}
// ###############################################################
// TutorialController functions
// ###############################################################
	private void controlTutorial () {
		if (GameController.started && GameController.inPlay && tutorialCount < startChildCount && !GameController.popupDisplaying) {
			GameController.inPlay = false;
			string popup = "Tutorial Popup " + tutorialCount.ToString();
			transform.Find(popup).gameObject.SetActive(true);
			transform.Find(popup).name = "Current Popup";
			currentPopup = transform.Find(popup).gameObject;
			tutorialCount++;
			GameController.popupDisplaying = true;
		}
	}
	// sometimes, you don't want to automatically increase the tutorial
	public void automaticTutorialSetting (bool tutorialSetting) {
		automaticTutorial = tutorialSetting;
	}
	// [FUTURE WORK: Burst popup idea]
	// Displays a single popup based on the name "Tutorial Popup <popupIndex>"
	public void manualTutorial(int popupIndex, int burst = 1) {
		string popup = "Tutorial Popup " + popupIndex.ToString();
		if (transform.Find(popup)) {
			GameController.popupDisplaying = true;
			transform.Find(popup).gameObject.SetActive(true);
			transform.Find(popup).name = "Current Popup";
			currentPopup = transform.Find("Current Popup").gameObject;
			tutorialCount++;
			GameController.inPlay = false;
		}
	}
}