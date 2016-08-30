using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// ###############################################################
// SC: LTF_Tutorial
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// LTF_Tutorial controls the tutorial and makes the circles, coin, and battery
// ###############################################################
public class LTF_Tutorial : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Tutorial variables
	private bool coinsDisplayed = false;
	private bool batteryDisplayed = false;
	private int tutorialState = 0;
	// UI Elements and Scripts
	private GameObject batteryPanel;
	private TutorialController tutorialScript;
	private BonusGenerator bonusGenerator;
	private BoostsGenerator boostsGenerator;
// ###############################################################
// Unity Functions
// ###############################################################
	void Awake () {
		// init diffs
		Difficulty.loadLTFDiff();
	}
	// ***************************************************************
	void Start () {
		// GameController variables init
		GameController.isTutorial = true;
		GameController.missionInitials = "LTF";
		GameController.challenge = "Learn how to fly!";
		// Update UI
		GameObject.Find("Circles Needed").GetComponent<Text>().text = Difficulty.objectiveNeeded.ToString();
		GameObject.Find("Circles Panel").SetActive(false);
		GameObject.Find("Points Panel").SetActive(false);
		// Drop in the tutorial prefab, connect to it, and set it to manual
		Instantiate(Resources.Load<GameObject>("UIPrefabs/LTFTutorial"));
		tutorialScript = GameObject.Find("LTFTutorial(Clone)").GetComponent<TutorialController>();
		tutorialScript.automaticTutorialSetting(false);
		// Connect to bonus generatory and boost generator
		bonusGenerator = GameObject.Find("Bonus Generator").GetComponent<BonusGenerator>();
		boostsGenerator = GameObject.Find("Boosts Generator").GetComponent<BoostsGenerator>();
		// Make the first circle
		makeCircle(new Vector3(4.98f,-0.08f,0f));
	}
	// ***************************************************************
	void Update () {
		// Once Circles Panel is active, updated it with the current objectiveGained value
		if (GameObject.Find("Circles Panel")) GameObject.Find ("Circles Hovered").GetComponent<Text> ().text = GameController.objectiveGained.ToString ();
		// Once the opening panel is done, start the tutorial
		if (GameController.started) controlLTFTutorial ();
	}
// ###############################################################
// LTF Tutorial Functions
// ###############################################################
	void controlLTFTutorial () {
	// case: Tutorial States | message
	// ____________________________________
	// 0: 	Tutorial Popup 1 | The UAV will fly to where you tap
	// 1: 	Tutorial Popup 2 | The objective is to hover in 6 circles
	//  	Tutorial Popup 3 | Fly to the second circle
	// 2: 	Tutorial Popup 4 | This shows your points, you get points for picking up coins and completing the mission
	// 3: 	
	// 4: 	Tutorial Popup 5 | Great!! UAVs run off of batteries, if your charge gets low, pick up a boost!
	// 5: 	
	// 6: 	End panel
		if (GameController.objectiveGained > tutorialState) tutorialState++;
		switch (tutorialState) {
			//......................................................................
			case 0:
				if (!GameController.popupDisplaying) tutorialScript.manualTutorial(1);
				break;
			//......................................................................
			case 1:
				if (!GameObject.Find("Dotted Circle")) {
					makeCircle(new Vector3 (-5.45f,-1.8f,0f));
					GameObject.Find("Canvas").transform.Find("Circles Panel").gameObject.SetActive (true);

					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(2);
				} 
				if (!GameObject.Find("Current Popup")) {
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(3);
				}
				break;
			//......................................................................
			case 2:
				if (!GameObject.Find("Dotted Circle")) {
					GameObject.Find("Canvas").transform.Find("Points Panel").gameObject.SetActive(true);
					coinsDisplayed = true;
					makeCircle(new Vector3 (1.92f,1.2f,0f));
					bonusGenerator.spawnScoutCoins(new Vector3 (1.92f,1.2f,0f));
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(4);
				} 
				break;
			//......................................................................
			case 3:
				if (!GameObject.Find("Dotted Circle")) {
					makeCircle(new Vector3 (-5.33f,2.39f,0f));
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(4);
				} 
				break;
			//......................................................................
			case 4:
				if (!GameObject.Find("Dotted Circle")) {
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(5);
					batteryPanel = GameController.buildGameObject("UIPrefabs/BatteryPanel", new Vector2(-25f,-35f), new Vector2(25f,35f));
					batteryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2 (-25f,-35f);
					batteryDisplayed = true;
					makeCircle (new Vector3(4.74f,-2.67f,0f));
					boostsGenerator.spawnScoutBoosts(new Vector3(4.74f,-2.67f,0f));
				}
				break;
			//......................................................................
			case 5:
				if (!GameObject.Find("Dotted Circle")) {
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(5);
					makeCircle (new Vector3(0f,3.41f,0f));
				}
				break;
			//......................................................................
			case 6:
				GameController.won = true;
				GameController.inPlay = false;
				GameController.message = "Now you can fly!";
				break;
		}
	}
// ###############################################################
	void makeCircle (Vector3 position) {
	    GameObject dottedCircle = new GameObject ("Dotted Circle");
        SpriteRenderer renderer = dottedCircle.AddComponent<SpriteRenderer> ();
        renderer.sprite = Resources.Load<Sprite> ("Sprites/DottedCircle");
        renderer.sortingLayerName = "Foreground";
        renderer.sortingOrder = 1;
        dottedCircle.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
		dottedCircle.transform.position = position;
        dottedCircle.AddComponent<CircleCollider2D> ();
        dottedCircle.GetComponent<CircleCollider2D> ().isTrigger = true;
		dottedCircle.GetComponent<CircleCollider2D> ().radius = 2f ;
        dottedCircle.AddComponent<CircleController> ();
        GameController.circleOnScreen = true;
	}
}