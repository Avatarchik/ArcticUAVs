using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// ###############################################################
// SC: LITH_Tutorial
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// LITH_Tutorial manually spawns Birds and controls the tutorial
//
// Tutorial Control Variables 
//	tutorialState		: Used to navigate the switch to linearly advance the tutorial
// ###############################################################
public class LITH_Tutorial : MonoBehaviour {

// ###############################################################
// Variables
// ###############################################################
	private Vector3 screenDimensions;

	// Used GameObjects
	private Rigidbody2D uav;
	private GameObject landingCircle;
	private GameObject batteryPanel;

	// scene settings
	private BirdGenerator birdGenerator;
	private TutorialController tutorialScript;
	private int tutorialState;

	// bird spawning stuff
	private bool spawningBirds;
	private float lastSpawn;

// ###############################################################
// Unity Functions
// ###############################################################
	// Use this for loading difficulty
	void Awake () {
		Difficulty.loadLITHDiff();
	}

	// Use this for initialization
	void Start () {
		GameController.isTutorial = true;
		print("LTIH_Tut Tutorial Set");
		GameController.missionDiff = "Tutorial";
		GameController.missionInitials = "LITH";
		tutorialState = 0;
		spawningBirds = false;
		lastSpawn = 0f;

		GameObject.Find("UAVs Needed").GetComponent<Text>().text = Difficulty.objectiveNeeded.ToString();
		birdGenerator = GameObject.Find("Bird Generator").GetComponent<BirdGenerator>();
		birdGenerator.automaticBirdSetting(false);

		Instantiate(Resources.Load<GameObject>("UIPrefabs/LITHTutorial"));
		GameController.challenge = "Learn how to catch!";
		tutorialScript = GameObject.Find("LITHTutorial(Clone)").GetComponent<TutorialController>();
		tutorialScript.automaticTutorialSetting(false);

		batteryPanel = GameController.buildGameObject("UIPrefabs/BatteryPanel", new Vector2 (-25f,-35f), new Vector2(25f,35f));
		batteryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2 (-25f,-35f);
		batteryPanel.transform.SetAsFirstSibling();

		landingCircle = new GameObject ();
		SpriteRenderer renderer = landingCircle.AddComponent<SpriteRenderer>();
		renderer.sprite = Resources.Load<Sprite>("Sprites/white-dotted-circle");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 10;
		renderer.color = new Vector4 (0f/255f,197f/255f,58f/255f,1f);
		renderer.transform.parent = GameObject.Find("Hand").transform;
		landingCircle.transform.localPosition = new Vector2 (0f,0f);
		landingCircle.transform.localScale = new Vector2 (2f,2f);
		landingCircle.name = "landingCircle";
	}

	// Update is called once per frame
	void Update () {
		if (GameObject.Find("UAVs Caught")) GameObject.Find ("UAVs Caught").GetComponent<Text> ().text = GameController.objectiveGained.ToString ();
		if (GameController.started) controlLITHTutorial ();
		if (spawningBirds) {
			if (GameController.gameClock - lastSpawn >= 7f) {
				lastSpawn = GameController.gameClock;
				birdGenerator.makeBird("right");
			}
		}
		if (GameObject.Find("landingCircle")) {
			landingCircle.transform.Rotate(new Vector3(0f,0f,0.75f));
		}
	}
// ###############################################################
// Control the LITH Tutorial using a switch on tutorial state
// ###############################################################
	void controlLITHTutorial () {
	// case: Tutorial States | message
	// 0: Tutorial Popup 1 | Catch the UAV
	// 0: Tutorial Popup 2 | point to Man's hand
	// 1: Tutorial Popup 3 | Great! Let's do it again!
	// 2: Tutorial Popup 4 | Also, You will want to watch out for birds that spawn 
	// 3: Tutorial Popup 5 | Great! now one last time, with feeling now!
		if (GameController.objectiveGained > tutorialState) tutorialState++;
		switch (tutorialState) {
			// Try and land the UAV safely in the Man's hand
			case 0:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(1);
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(2);
				break;
			// Do It Again
			case 1:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(3);
				break;
			// Birds
			case 2: 
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(4);
				if (!spawningBirds){
					spawningBirds = true;
					lastSpawn = GameController.gameClock;
					birdGenerator.makeBird("left");
				}
				break;
			// last time
			case 3:
				if (!GameObject.Find("Current Popup") && !GameController.popupDisplaying) tutorialScript.manualTutorial(5);
				break;
			case 4:
				GameController.message = "Now you can catch!";
				GameController.won = true;
				GameController.inPlay = false;
				break;
		}
	}

	private void spawnBird () {
		GameObject bird = new GameObject (name: "bird");
		bird.transform.parent = GameObject.Find ("Birds").transform;
		string side = "";
		if (Random.value > 0.5f) {
			side = "Left";
		}
		side = "Right";

		SpriteRenderer birdRenderer = bird.AddComponent<SpriteRenderer> ();
		birdRenderer.sprite = Resources.Load ("Sprites/bird mid wing " + side, typeof(Sprite)) as Sprite;
		birdRenderer.sortingLayerName = "Foreground";
		birdRenderer.sortingOrder = 4;

		bird.transform.localScale = new Vector3 (0.14f, 0.14f, bird.transform.localScale.z);
		bird.transform.position = new Vector3 (GameObject.Find (side + " Cloud Limit").transform.position.x,
			Random.Range (-screenDimensions.y + 2f, screenDimensions.y - 1f));
		bird.AddComponent<PolygonCollider2D> ();
		bird.AddComponent<BirdController> ();
	}
}