using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ###############################################################
// GameController is responsible for a large majority of the game's global states and variables
// Setting the variables as public and static allows any other script active in the scene to connect to 
// 	  GC without having to instance GC. These variables are set by a combination of the scenes and the uav
//
//	UI Global elements
//		challenge		: UI String for Start Panel
//		message			: UI String for End Panel
//		popupDisplaying	: Bool for Tutorial
//	uavStatus Global Vars
//		landed			: Bool for UAVs flying status
//		onGround		: Bool for UAVs flying status (MTI)
//	gameStatus Global Vars
//		started			: Bool for Start Panel finished and gameplay okay
//		inPlay			: Bool for gameplay okay
//		won				: Bool for victory condition
//		lost			: Bool for loss condition
//		isTutorial		: Bool for tutorial scene (declared by scene)
//	Difficulty(sp) Global Vars
//		missionInitials	: LTF | LITH | GTP | MTI
//		missionDiff		: Tutorial | Easy | Normal | Hard | Insane
//	Objective Globals Vars
//		objectiveGained	: Int of objectives gained
//		objectiveTime	: float of time to objective
//		circleOnScreen	: Bool for LTF for circle
//	Game Clock Global Var
//		gameClock		: float for time inPlay
//	Other Global Vars
//		albinoOtter		: deprecated ?
//		birdsMissed		: deprecated ?
//		birdsHit		: deprecated ?
//	Audio Global Vars
//		musicStatus		: bool for music status [BLOCKED BECAUSE REASONS]
//		sfxStatus		: bool for sound effects(SFX) status
//
// ###############################################################
public class GameController : MonoBehaviour {
// ###############################################################
// Varables 
// ###############################################################
	// UI Global elements
		public static string challenge;
		public static string message;
		public static bool popupDisplaying = false;
	// uavStatus Global Vars
		public static bool landed = true;
		public static bool onGround = false;
	// gameStatus Global Vars
		public static bool started = false;
		public static bool inPlay = false;
		public static bool won = false;
		public static bool lost = false;
		public static bool isTutorial = false;
	// Private gameStatus vars to view in editor (Right click Inspector + debug)
		private bool _started_ = false;
		private bool _inPlay_ = false;
		private bool _won_ = false;
		private bool _lost_ = false;
		private bool _isTutorial_ = false;
	// Difficulty(sp) Global Vars
		public static string missionInitials = "no";
		public static string missionDiff = "LEMONADE";
	// Objective Globals Vars
		public static int objectiveGained = 0;
		public static float objectiveTime = 0;
		public static bool circleOnScreen = false;
	// Game Clock Global Var
		public static float gameClock;
	// Other Global Vars
		public static bool albinoOtter = false;
		public static int birdsMissed = 0;
		public static int birdsHit = 0;
	// Scripts
		private GameObject openingScript;
		private GameObject endingScript;
		private GameObject bonusGenerator;
		private GameObject boostsGenerator;
		private GameObject musicController;
	// Audio Global Vars
		public static bool musicStatus;
		public static bool sfxStatus;

	// Difficulties Stuff
		public static GameObject difficulty;
// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		// Reset our statics again
		resetStaticVariables();	
		// Instantiate difficulty
		difficulty = new GameObject ();
		difficulty.AddComponent<Difficulty>();
		difficulty.name = "Difficulty";
		difficulty.transform.parent = GameObject.Find("Game Controller").transform;
		// Instantiate Opening
		openingScript = new GameObject ();
		openingScript.AddComponent<Opening>();
		openingScript.name = "openingScript";
		openingScript.transform.parent = GameObject.Find("Game Controller").transform;
		// Instantiate Ending
		endingScript = new GameObject ();
		endingScript.AddComponent<Ending>();
		endingScript.name = "endingScript";
		endingScript.transform.parent = GameObject.Find("Game Controller").transform;
		// Instantiate BonusGenerator
		bonusGenerator = new GameObject ();
		bonusGenerator.AddComponent<BonusGenerator>();
		bonusGenerator.name = "Bonus Generator";
		// Instantiate BoostsGenerator
		boostsGenerator = new GameObject ();
		boostsGenerator.AddComponent<BoostsGenerator>();
		boostsGenerator.name = "Boosts Generator";
		// Instantiate Music
		musicController = new GameObject ();
		musicController.AddComponent<MusicController>();
		musicController.name = "Music Controller";
	}
	// Right click on inspector in unity and select debug to view these variables
	void Update () {
		_started_ = started;
		_inPlay_ = inPlay;
		_won_ = won;
		_lost_ = lost;
		_isTutorial_ = isTutorial;

		// set the "isTutorial" bool if missiondiff equals "Tutorial"
		isTutorial = missionDiff == "Tutorial";

		// update gameClock
		if (inPlay && !lost) {
			gameClock += Time.deltaTime;
		}
	}
// ###############################################################
// GameController Functions 
// ###############################################################
	// Function for instantiating UI Elements directly onto the canvas
	// Accepts:
	//
	//	resource 	:	String 	: location in resources of prefab
	//  offsetMin	:	Vector2 : offset from bottom left corner of anchor
	//  offsetMax	:	Vector2	: offset from top right corner of anchor
	public static GameObject buildGameObject (string resource, Vector2 offsetMin, Vector2 offsetMax) {
		GameObject newGameObject = Instantiate(Resources.Load<GameObject>(resource));
		newGameObject.transform.parent = GameObject.Find("Canvas").transform;
		newGameObject.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		newGameObject.GetComponent<RectTransform>().offsetMin = offsetMin;
		newGameObject.GetComponent<RectTransform>().offsetMax = offsetMax;
	
		return newGameObject;
	}
	// [FUTURE WORK: REMOVE STATICS]
	// Debugging effort, sovles race condidtions, see also: ButtonController.resetStaticVariables ()
	public void resetStaticVariables () {
		print("GameController: Reset Static Variables");
		// Other resets
		Time.timeScale = 1;
		// uavStatus Global Vars
		GameController.landed = true;
		GameController.onGround = true;
		// gameStatus Global Vars
		GameController.started = false;
		GameController.inPlay = false;
		GameController.won = false;
		GameController.lost = false;
		print("GC Tutorial reset");
		GameController.isTutorial = false;
		GameController.popupDisplaying = false;
		// Objective Globals Vars
		GameController.objectiveGained = 0;
		GameController.objectiveTime = 0;
		GameController.circleOnScreen = false;
		// Game Clock Global Var
		GameController.gameClock = 0;
		// Other Global Vars
		GameController.albinoOtter = false;
		GameController.birdsMissed = 0;
		GameController.birdsHit = 0;
		// Points Controller Variables
		PointsController.missionScore = 0;
		PointsController.objectiveScore = 0;
		PointsController.bonusGained = 0;
		PointsController.bonusScore = 0;
	}
}
