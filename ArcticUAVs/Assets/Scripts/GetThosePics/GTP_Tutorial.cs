using System.Collections;
using UnityEngine;
using UnityEngine.UI;
// ###############################################################
// SC: GTP_Tutorial
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// GTP_Tutorial manually spawns otters, controls the tutorial, and programatically extends the map
//
// Extend Map Variables
//	copies 				: The amount of instantiated backgrounds, used in positioning the background
// Tutorial Control Variables
//	tempCounter 		: Used to acheive a burst of 3 popups
//	tutorialState		: Used to navigate the switch to linearly advance the tutorial
//	coinsSpawned		: Used to only spawn a single coin
//	batteryDisplayed	: Used to only spawn a signle battery
// ###############################################################
public class GTP_Tutorial : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Victory Condition variables
	private bool picturesCaptured = false;
	private bool localWon;
	// Extend Map Variables
	private int copies = 0;
	// Tutorial Control Variables
	private int tempCounter = 1;
	private int tutorialState = 0;
	private bool coinsSpawned = false;
	private bool batteryDisplayed = false;
	// UI Elements and Scripts
	private GameObject batteryPanel;
	private TutorialController tutorialScript;
	private BonusGenerator bonusGenerator;
	private BoostsGenerator boostsGenerator;
	private GameObject landingCircle;
// ###############################################################
// Unity Functions
// ###############################################################
	void Awake () {
		// init diffs
		Difficulty.loadGTPDiff();
	}
	// ***************************************************************
	void Start () {
		// Game Controller variables init
		GameController.isTutorial = true;
		GameController.missionInitials = "GTP";
		GameController.challenge = "Learn how to take pics!";
		// Connect with UI
		GameObject.Find ("Needed Pics").GetComponent<Text> ().text = Difficulty.objectiveNeeded.ToString();
		// Create intial otters
		OtterGenerator.createOtter(new Vector3(4f,-3.8f));
		OtterGenerator.createOtter(new Vector3(13f,-3.8f));
		OtterGenerator.createOtter(new Vector3(26f,-3.8f));
		// Bring in the tutorial, set it to manual
		Instantiate(Resources.Load<GameObject>("UIPrefabs/GTPTutorial"));
		tutorialScript = GameObject.Find("GTPTutorial(Clone)").GetComponent<TutorialController>();
		tutorialScript.automaticTutorialSetting(false);
		// Tutorial Vars
		tempCounter = 1;
		tutorialState = 0;
		// Generate the background only for the space we need
		for (int i = 0; i <2; i++) {
			copies++;
			makeNewBackground ();
			widenWaves ();
		}
	}
	// ***************************************************************
	void Update () {
		// Sometimes Bonus and Boosts aren't there for Start (), so we connect with them in Update ()
		if (bonusGenerator == null) bonusGenerator = GameObject.Find("Bonus Generator").GetComponent<BonusGenerator> ();
		if (boostsGenerator == null) boostsGenerator = GameObject.Find("Boosts Generator").GetComponent<BoostsGenerator> ();
		// Once the Opening.cs animation concludes, start the tutorial
		if (GameController.started) controlGTPTutorial ();
		// Check if the pics are all taken, if so put up the "return to dock icon"
		checkIfPictureGoalAchieved ();
		// If picstaken and landed on the dock, you won!
		checkForLevelCompletion ();
		// Update UI
		GameObject.Find ("Captured Pics").GetComponent<Text> ().text = GameController.objectiveGained.ToString();
		if (GameObject.Find("landingCircle(Clone)")) {
			landingCircle.transform.Rotate(new Vector3(0f,0f,0.75f));
		}
		// Account for scared otters
		if (OtterGenerator.otterCount < 3) OtterGenerator.createOtter(new Vector3(Random.Range(6f,25f),-3.8f));
	}
// ###############################################################
// Victory condidition Functions
// ###############################################################
// Checks needed vs. gained and if it's true creates the green landing circle and the "return to dock" panel
	private void checkIfPictureGoalAchieved () {
		if (!picturesCaptured) {
			if (GameController.objectiveGained >= Difficulty.objectiveNeeded) {
				picturesCaptured = true;
				GameObject.Find("Canvas").transform.Find("Message Panel").gameObject.SetActive(true);
				landingCircle = Instantiate(Resources.Load<GameObject>("GamePrefabs/landingCircle"), new Vector3(-1.08f,-2.22f),Quaternion.identity) as GameObject;
			}
		}
	}
	// ***************************************************************
	// If we haven't won(locally), && we have all the pics && the UAV isn't moving anymore, local win, bring in the view pics panel
	private void checkForLevelCompletion () {
		if (!localWon && !GameController.lost && picturesCaptured && GameController.landed && GameObject.Find ("uav").GetComponent<Rigidbody2D> ().velocity.Equals (Vector2.zero)) {
			GameController.message = "Great Pics!";
			localWon = true;
			GameController.inPlay = false;
			GameController.buildGameObject("UIPrefabs/viewPicturesPanel",new Vector2(0f,0f), new Vector2(0f,0f));
		}
	}
// ###############################################################
// Extend Map Functions: If you're close to the boundry, copy the background and widen the waves
// ###############################################################
			private void makeNewBackground () {
				GameObject newBackground = Instantiate (Resources.Load<GameObject>("GamePrefabs/GTPBackground"));
				newBackground.transform.parent = GameObject.Find("Homer").transform;
				newBackground.transform.localPosition = new Vector3 (20 * copies, 2.9f);
				newBackground.SetActive(true);
				for (int i = 0; i < newBackground.transform.childCount-2; i++) { // For each object in background (excluding the base objects), hide or show randomly
					newBackground.transform.GetChild (i).gameObject.SetActive (Random.value > 0.5f);
				}
			}
			private void widenWaves () {
				for (int i = 0; i < 3; i++) {
					for (int j = 0; j < 4; j++) {
						GameObject original = GameObject.Find ("Waves").transform.GetChild (i).transform.GetChild (j).gameObject;
						GameObject newObject = Instantiate (original);
						newObject.transform.parent = original.transform.parent;
						newObject.transform.position = new Vector3 (original.transform.position.x + (20 * copies),
							original.transform.position.y);
						newObject.transform.localScale = original.transform.localScale;
					}
				}
			}
// ###############################################################
// Control the GTP Tutorial using a switch on GC.objectiveGained
// ###############################################################
	void controlGTPTutorial () {
	// case: Tutorial States | message
	// ____________________________________
	// 0: 	Tutorial Popup 1 | This shows how many photos you need
	//   	Tutorial Popup 2 | To capture a photo, hover in the region
	//   	Tutorial Popup 3 | Be careful to not scare the otter by flying too low
	// 1: 	Tutorial Popup 4 | You can get bonus points for collecting shells
	// 2: 	Tutorial Popup 5 | Great!! UAVs run off of batteries, if your charge gets low, pick up a boost!
	// [FUTURE WORK: add in weather mechanics]
		if (GameController.objectiveGained > tutorialState) tutorialState++;
		switch (tutorialState) {
			case 0:
			// Display the first 3 popups
				if (!GameObject.Find("Current Popup")) {
					if (!GameController.popupDisplaying && tempCounter < 4) {
						tutorialScript.manualTutorial(tempCounter);
						tempCounter ++;
						}
				}
				break;
			case 1:
			// When the user gets the first otter, remind them about coins
				if (!GameObject.Find("Current Popup") && GameObject.Find("Canvas").GetComponent<Image>().color.a < 0.1f) {
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(4);
					if (!coinsSpawned) {
						bonusGenerator.spawnGTPCoins(new Vector3(8f,2f));
						coinsSpawned = true;
						bonusGenerator.automaticCoinsSetting(true);
					}
				}
				break;
			case 2:
			// When the user gets teh second otter, remind them about batteries
				if (!GameObject.Find("Current Popup") && GameObject.Find("Canvas").GetComponent<Image>().color.a < 0.1f) {
					if (!GameController.popupDisplaying) tutorialScript.manualTutorial(5);
					if (!batteryDisplayed) {
						batteryPanel = GameController.buildGameObject("UIPrefabs/BatteryPanel", new Vector2(-25f,-35f), new Vector2(25f,35f));
						batteryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2 (-25f,-35f);
						batteryDisplayed = true;
						boostsGenerator.spawnScoutBoosts(new Vector3(16f,1.5f));
						boostsGenerator.automaticBoostsSetting(true);
					}
				}
				break;
			// The scene handles victory conditions
		}
	}
}