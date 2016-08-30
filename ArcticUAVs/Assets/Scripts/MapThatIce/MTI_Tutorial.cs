using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ###############################################################
// SC: MTI_Tutorial
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// MTI_Tutorial manually creates the maze and controls the tutorial
//
// Tutorial Control Variables
// 		tutorialState : Used to navigate the switch to linearly advance the tutorial
// ###############################################################
public class MTI_Tutorial : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	// UAV Vars
	private Transform uav;
	private float targetX;
	private float uavAltitude = 20;

	// Used GameObjects
	private GameObject batteryPanel;
	private GameObject pointsPanel;
	private GameObject mobileControl;
	private CameraController cameraController;
	private Transform camera;
	private LandingZoneController LZController;
	private Transform polarBear;
	private Transform bonus;
	private Transform whaler;

	// Tutorial Stuff
	private TutorialController tutorialScript;
	private int tutorialState;

	// Other Vars
	private float timePoint = 0f;
	private Vector3 adjTarget;

// ###############################################################
// Unity Functions 
// ###############################################################
	void Awake () {
		GameController.isTutorial = true;
		GameController.missionDiff = "Tutorial";
		Difficulty.loadMTIDiff();
	}

	void Start () {
		GameController.missionInitials = "MTI";
		GameController.challenge = "Solve the maze by UAV.";
		GameController.message = "You made it!";
		GameController.landed = true;
		GameController.onGround = true;
		tutorialState = 0;

		// Instantiate(Resources.Load<GameObject>("UIPrefabs/MTITutorial"));
		tutorialScript = GameObject.Find("MTITutorial(Clone)").GetComponent<TutorialController>();
		tutorialScript.automaticTutorialSetting(false);

		batteryPanel = GameController.buildGameObject("UIPrefabs/BatteryPanel", new Vector2 (-25f,-35f), new Vector2(25f,35f));
		batteryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2 (-25f,-35f);
		batteryPanel.transform.SetAsFirstSibling();

		pointsPanel = GameObject.Find("Points Panel");
		pointsPanel.SetActive(false);

		mobileControl = GameObject.Find("MobileSingleStickControl");

		uav = GameObject.Find("uav").transform;
		whaler = GameObject.Find("Whaler").transform;

		camera = GameObject.Find("Camera Controller").transform;
		cameraController = GameObject.Find("Camera Controller").GetComponent<CameraController>();

		LZController = GameObject.Find("MTILandingZone").GetComponent<LandingZoneController>();
		LZController.showLandingPanel = false;

		polarBear = GameObject.Find("Nanook").transform;
		bonus = GameObject.Find("MTICoins").transform;

		targetX = GameObject.Find ("uav").transform.position.x;

		// static maze
		int[,] maze = {
		//   W 1 2 3 4 5 6 7 8 9 E
			{1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1},
			{1,1,0,0,0,0,1,1,1,1,1},
			{1,1,0,1,1,0,1,1,1,1,1},
			{1,1,0,1,1,0,1,1,1,1,1},
			{1,1,0,0,1,0,0,1,1,1,1},
			{1,1,1,0,1,1,0,1,1,1,1},
			{1,1,1,0,1,0,0,0,0,1,1},
			{1,1,0,0,1,0,0,1,0,1,1},
			{1,1,0,1,1,1,0,0,0,1,1},
			{1,0,0,1,1,0,0,0,0,0,1},
			{1,1,0,1,0,0,1,1,0,1,1},
			{1,1,0,1,1,0,1,1,0,1,1},
			{1,1,0,0,1,0,0,0,0,1,1},
			{1,1,0,0,1,1,0,0,0,0,1},
			{1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1},
			{1,1,1,1,1,1,1,1,1,1,1}
		};
		GameObject.Find("Maze Generator").GetComponent<MazeGenerator>().setMaze(maze);
		GameObject.Find("Maze Generator").GetComponent<MazeGenerator>().assignTerrainBlocks();
	}

	void Update() {
		if (GameController.started) controlMTITutorial ();		
	}

// ###############################################################
// Tutorial Functions 
// ###############################################################
	void controlMTITutorial () {
	// case: Tutorial States | message
	// 0: Tutorial Popup 1 | Objective: Solve the maze
	//    Tutorial Popup 2 | Use the UAV to solve the maze
	// 1: Tutorial Popup 3 | launch and land the UAV using the Launch/Land button
	// 2: Tutorial Popup 4 | Move the UAV with the Joystick
	// 3: Tutorial Popup 5 | The UAV has a battery, don't let it run out before the whalers reach the sea edge
	// 4: Tutorial Popup 6 | You can set markers for the Whalers by tapping and holding on the screen.
	// 6: Tutorial Popup 7 | (pan camera) Tap and hold to set a marker here
	//    Tutorial Popup 8 | Keep in mind, markers can only be set where the whalers can see them.
	// 7: Tutorial Popup 9 | (pan camera) There are fish in the maze that you can pick up for bonus points
	// 8: Tutorial Popup 10 | (pan camera) There are also polar bears in the maze, so watch out!
	// 9: Tutorial Popup 11 | (pan camera back to UAV) Now lead the whalers through the maze
		switch (tutorialState) {
			// Introduce the MTI Controls
			case 0:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					print("test 1"); tutorialScript.manualTutorial(1);
				}
				// flying into the circle increases objective gained, which increases tutorialState
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					print("test 2"); tutorialScript.manualTutorial(2);
				}
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					LZController.showLandingPanel = true;
					if (checkForLaunchButton()) tutorialState++;
				}
				break;
			// Launch UAV
			case 1:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(3);
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					LZController.showLandingPanel = true;
					if (uav.position.y >= 20f) tutorialState++;
				}
				break;
			// Test UAV
			case 2: 
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					tutorialScript.manualTutorial(4);
					timePoint = GameController.gameClock;
					tutorialState++;
				}
				break;
			// point out battery
			case 3:
				if (GameController.gameClock - timePoint >= 7.5f) {
					if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
						if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(5);
						tutorialState++;
					}
				}
				break;
			// Move the Whalers
			case 4:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					LZController.showLandingPanel = false;
					mobileControl.SetActive(false);
					adjTarget = new Vector3(whaler.position.x, camera.position.y, whaler.position.z);
					cameraController.toFollow = null;
					if (camera.position != adjTarget) {
						camera.position = Vector3.MoveTowards(camera.position, adjTarget, 10f * Time.deltaTime);
						break;
					}
					tutorialScript.manualTutorial(6);
				}
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialState++;
				break;
			case 5:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					createTargetZone(new Vector3(66f,0f,11f));
					tutorialState++;
				}
				break;
			// place a marker here in the maze
			case 6:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					adjTarget = new Vector3(66f, camera.position.y, 12f);
					if (camera.position != adjTarget) {
						camera.position = Vector3.MoveTowards(camera.position, adjTarget, 10f * Time.deltaTime);
						break;
					}
					tutorialScript.manualTutorial(7);
					LZController.showLandingPanel = false;
				}
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(8);
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialState++;
				break;
			// check for a marker
			case 7:
				if (GameObject.Find("Placed Marker") && !GameController.popupDisplaying) {
					Destroy(GameObject.Find("Target Circle"));
					tutorialState++;
				}
				break;
			// bonuses
			case 8:
				if (!GameObject.Find("Placed Marker") && !GameController.popupDisplaying) {
					adjTarget = new Vector3(bonus.position.x, camera.position.y, bonus.position.z);
					if (camera.position != adjTarget) {
						camera.position = Vector3.MoveTowards(camera.position, adjTarget, 10f * Time.deltaTime);
						break;
					}
					tutorialScript.manualTutorial(9);
					tutorialState++;
				}
				break;
			case 9:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					adjTarget = new Vector3(polarBear.position.x, camera.position.y, polarBear.position.z);
					if (camera.position != adjTarget) {
						camera.position = Vector3.MoveTowards(camera.position, adjTarget, 10f * Time.deltaTime);
						break;
					}
					tutorialScript.manualTutorial(10);
				}
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialState++;
				break;
			case 10:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) {
					adjTarget = new Vector3(uav.position.x, camera.position.y, uav.position.z);
					if (camera.position != adjTarget) {
						camera.position = Vector3.MoveTowards(camera.position, adjTarget, 10f * Time.deltaTime);
						break;
					}
					cameraController.toFollow = uav.gameObject;
					tutorialScript.manualTutorial(11);
					LZController.showLandingPanel = true;
					pointsPanel.SetActive(true);
					mobileControl.SetActive(true);
				}
				break;
		}
	}

	private bool checkForLaunchButton() {
		return GameObject.Find("Land Launch Button").transform.position == GameObject.Find("LandOnPosition").transform.position;
	}

	private void createTargetZone (Vector3 position) {
		GameObject landingCircle = new GameObject (name: "Target Circle");
		SpriteRenderer renderer = landingCircle.AddComponent<SpriteRenderer>();
		renderer.sprite = Resources.Load<Sprite>("Sprites/white-dotted-circle");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 10;
		renderer.color = new Vector4 (0f/255f,197f/255f,58f/255f,1f);
		landingCircle.transform.localScale = new Vector2 (1.5f,1.5f);
		landingCircle.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
		landingCircle.transform.position = position;
	}
}