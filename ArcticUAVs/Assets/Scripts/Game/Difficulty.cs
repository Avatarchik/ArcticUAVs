using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// Class Aliases
using GC = GameController;
using PC = PointsController;
// ###############################################################
// The Diffculties class is integral to game settings
// the following settings are scaled based of difficulty
//
// objectiveNeeded	: objectives needed for victory
// objectiveValue	: points per objective
// bonusValue		: bonus(coins) point value
// nearMissValue	: value for a near miss on a bird
// polarBears		: number of polar bears that spawn
// weatherIntesity	: ??[FUTURE WORK: jonathan comment]
// clamping			: bool to create the edges for easier gameplay in LTF and LITH
// boatSpeed		: how fast the boat in LITH executes MoveTowards
// birdSpawnTime	: how often birds spawn
// batteryLife		: base battery life
// objectiveTime	: How long it should take to complete the mission [FUTURE WORK: implement]
//
// Difficulty is instantiated by GameController, the buttons in the <Mission>_Difficulties.unity 
// set GameController.missionDiff, which is what this script relies on to populate the values
//
// Difficulty has four functions for the four missions
//	loadLTFDiff() 
// 	loadLITHDiff()
// 	loadGTPDiff()
// 	loadMITDiff()
//
// in order to prevent race conditions these functions should be run in awake() of the scene controller
// ###############################################################
public class Difficulty : MonoBehaviour {

	public static int objectiveNeeded;
	public static int objectiveValue;
	public static int bonusValue;
	public static int nearMissValue;
	public static int polarBears;
	public static int weatherIntesity;
	public static bool clamping;
	public static float boatSpeed;
	public static float birdSpawnTime;
	public static float batteryLife;
	public static float objectiveTime;

// ******************************************************************************************************************
// Load LTF Difficulty Settings      ********************************************************************************
// ******************************************************************************************************************
	// Learn To Fly Difficulties settings
	public static void loadLTFDiff() {
		string diff = GameController.missionDiff;
		print("Difficulty.cs: "+diff);
	// ###############################################################
		Dictionary<string, int> objectiveNeededDict = new Dictionary<string, int> () {
			{ "Tutorial", 6 },
			{ "Easy", 20 },
			{ "Normal", 25 },
			{ "Hard", 30 },
			{ "Insane", 50 }
		};
		objectiveNeeded = objectiveNeededDict[diff];
	// ###############################################################
		Dictionary<string, int> objectiveValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 25 },
			{ "Easy", 25 },
			{ "Normal", 40 },
			{ "Hard", 150 },
			{ "Insane", 200 } 
		};
		objectiveValue = objectiveValueDict[diff];
	// ###############################################################
		Dictionary<string, int> bonusValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 25 },
			{ "Easy", 25 },
			{ "Normal", 50 },
			{ "Hard", 100 },
			{ "Insane", 150 } 
		};
		bonusValue = bonusValueDict[diff];
	// ###############################################################
		// Dictionary<string, int> weatherIntesityDict = new Dictionary<string, int> () {
		// 	{ "Tutorial", 0 },
		// 	{ "Easy", 1 },
		// 	{ "Normal", 2 },
		// 	{ "Hard", 3 },
		// 	{ "Insane", 4 } 
		// };
		// weatherIntesity = weatherIntesityDict[diff];
	// ###############################################################
		Dictionary<string, bool> clampingDict = new Dictionary<string, bool> () {
			{ "Tutorial", true },
			{ "Easy", true },
			{ "Normal", true},
			{ "Hard", true },
			{ "Insane", false } 
		};
		clamping = clampingDict[diff];
	// ###############################################################
		Dictionary<string, float> birdSpawnTimeDict = new Dictionary<string, float> () {
			{ "Tutorial", 100 },
			{ "Easy", 15 },
			{ "Normal", 10 },
			{ "Hard", 8 },
			{ "Insane", 4 } 
		};
		birdSpawnTime = birdSpawnTimeDict[diff];
	// ###############################################################
		Dictionary<string, float> batteryLifeDict = new Dictionary<string, float> () {
			{ "Tutorial", 8 },
			{ "Easy", 35 },
			{ "Normal", 28 },
			{ "Hard", 16 },
			{ "Insane", 12 } 
		};
		batteryLife = batteryLifeDict[diff];
	// ###############################################################
		Dictionary<string, int> nearMissValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 5000 },
			{ "Easy", 5000 },
			{ "Normal", 5000 },
			{ "Hard", 5000 },
			{ "Insane", 5000 } 
		};
		nearMissValue = nearMissValueDict[diff];
	// ###############################################################
		// Dictionary<string, float> objectiveTimeDict = new Dictionary<string, float> () {
		// 	{ "Tutorial", 300 },
		// 	{ "Easy", 300 },
		// 	{ "Normal", 300 },
		// 	{ "Hard", 300 },
		// 	{ "Insane", 300 } 
		// };
		// objectiveTime = objectiveTimeDict[diff];
	}

// ******************************************************************************************************************
// Load LITH Difficulty Settings     ********************************************************************************
// ******************************************************************************************************************
	public static void loadLITHDiff() {
		string diff = GameController.missionDiff;
		print("Difficulty.cs: "+diff);
	// ###############################################################
		Dictionary<string, int> objectiveNeededDict = new Dictionary<string, int> () {
			{ "Tutorial", 4 },
			{ "Easy", 3 },
			{ "Normal", 6 },
			{ "Hard", 10 },
			{ "Insane", 15 }
		};
		objectiveNeeded = objectiveNeededDict[diff];
	// ###############################################################
		Dictionary<string, int> objectiveValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 100 },
			{ "Easy", 100 },
			{ "Normal", 150 },
			{ "Hard", 500 },
			{ "Insane", 1000 } 
		};
		objectiveValue = objectiveValueDict[diff];
	// ###############################################################
		Dictionary<string, int> bonusValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 25 },
			{ "Easy", 25 },
			{ "Normal", 50 },
			{ "Hard", 100 },
			{ "Insane", 150 } 
		};
		bonusValue = bonusValueDict[diff];
	// ###############################################################
		Dictionary<string, int> nearMissValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 5000 },
			{ "Easy", 5000 },
			{ "Normal", 5000 },
			{ "Hard", 5000 },
			{ "Insane", 5000 } 
		};
		nearMissValue = nearMissValueDict[diff];
	// ###############################################################
		Dictionary<string, int> weatherIntesityDict = new Dictionary<string, int> () {
			{ "Tutorial", 0 },
			{ "Easy", 1 },
			{ "Normal", 2 },
			{ "Hard", 3 },
			{ "Insane", 4 } 
		};
		weatherIntesity = weatherIntesityDict[diff];
	// ###############################################################
		Dictionary<string, bool> clampingDict = new Dictionary<string, bool> () {
			{ "Tutorial", true },
			{ "Easy", true },
			{ "Normal", false },
			{ "Hard", false },
			{ "Insane", false } 
		};
		clamping = clampingDict[diff];
	// ###############################################################
		Dictionary<string, float> birdSpawnTimeDict = new Dictionary<string, float> () {
			{ "Tutorial", 100 },
			{ "Easy", 20 },
			{ "Normal", 10 },
			{ "Hard", 5 },
			{ "Insane", 2 }
		};
		birdSpawnTime = birdSpawnTimeDict[diff];
	// ###############################################################
		Dictionary<string, float> batteryLifeDict = new Dictionary<string, float> () {
			{ "Tutorial", 40 },
			{ "Easy", 40 },
			{ "Normal", 25 },
			{ "Hard", 15 },
			{ "Insane", 10 } 
		};
		batteryLife = batteryLifeDict[diff];
	// ###############################################################
		Dictionary<string, float> boatSpeedDict = new Dictionary<string, float> {
			{ "Tutorial", 0 },
			{ "Easy", 0.2f },
			{ "Normal", 0.5f },
			{ "Hard", 1 },
			{ "Insane", 2 }
		};
		boatSpeed = boatSpeedDict[diff];
	// ###############################################################
		// Dictionary<string, float> objectiveTimeDict = new Dictionary<string, float> () {
		// 	{ "Tutorial", 300 },
		// 	{ "Easy", 300 },
		// 	{ "Normal", 300 },
		// 	{ "Hard", 300 },
		// 	{ "Insane", 300 } 
		// };
		// objectiveTime = objectiveTimeDict[diff];
	// ###############################################################
	}

// ******************************************************************************************************************
// Load GTP Difficulty Settings      ********************************************************************************
// ******************************************************************************************************************
	public static void loadGTPDiff() {
		string diff = GameController.missionDiff;
		print("Difficulty.cs: "+diff);
	// ###############################################################
		Dictionary<string, int> objectiveNeededDict = new Dictionary<string, int> () {
			{ "Tutorial", 3 },
			{ "Easy", 3 },
			{ "Normal", 5 },
			{ "Hard", 10 },
			{ "Insane", 15 }
		};
		objectiveNeeded = objectiveNeededDict[diff];
	// ###############################################################
		Dictionary<string, int> objectiveValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 200 },
			{ "Easy", 200 },
			{ "Normal", 500 },
			{ "Hard", 800 },
			{ "Insane", 1500 } 
		};
		objectiveValue = objectiveValueDict[diff];
	// ###############################################################
		Dictionary<string, int> bonusValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 25 },
			{ "Easy", 25 },
			{ "Normal", 50 },
			{ "Hard", 100 },
			{ "Insane", 150 } 
		};
		bonusValue = bonusValueDict[diff];
	// ###############################################################
		Dictionary<string, int> nearMissValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 5000 },
			{ "Easy", 5000 },
			{ "Normal", 5000 },
			{ "Hard", 5000 },
			{ "Insane", 5000 } 
		};
		nearMissValue = nearMissValueDict[diff];
	// ###############################################################
		Dictionary<string, int> weatherIntesityDict = new Dictionary<string, int> () {
			{ "Tutorial", 0 },
			{ "Easy", 1 },
			{ "Normal", 2 },
			{ "Hard", 3 },
			{ "Insane", 4 } 
		};
		weatherIntesity = weatherIntesityDict[diff];
	// ###############################################################
		Dictionary<string, bool> clampingDict = new Dictionary<string, bool> () {
			{ "Tutorial", false },
			{ "Easy", false },
			{ "Normal", false },
			{ "Hard", false },
			{ "Insane", false } 
		};
		clamping = clampingDict[diff];
	// ###############################################################
		Dictionary<string, float> birdSpawnTimeDict = new Dictionary<string, float> () {
			{ "Tutorial", 100 },
			{ "Easy", 20 },
			{ "Normal", 10 },
			{ "Hard", 8 },
			{ "Insane", 4 } 
		};
		birdSpawnTime = birdSpawnTimeDict[diff];
	// ###############################################################
		Dictionary<string, float> batteryLifeDict = new Dictionary<string, float> () {
			{ "Tutorial", 20 },
			{ "Easy", 35 },
			{ "Normal", 25 },
			{ "Hard", 20 },
			{ "Insane", 15 } 
		};
		batteryLife = batteryLifeDict[diff];
	// ###############################################################
		// Dictionary<string, float> objectiveTimeDict = new Dictionary<string, float> () {
		// 	{ "Tutorial", 300 },
		// 	{ "Easy", 300 },
		// 	{ "Normal", 300 },
		// 	{ "Hard", 300 },
		// 	{ "Insane", 300 } 
		// };
		// objectiveTime = objectiveTimeDict[diff];
	// ###############################################################
	}

// ******************************************************************************************************************
// Load MTI Difficulty Settings      ********************************************************************************
// ******************************************************************************************************************
	public static void loadMTIDiff() {
		string diff = GameController.missionDiff;
		print("Difficulty.cs: "+diff);
	// ###############################################################
		Dictionary<string, int> objectiveNeededDict = new Dictionary<string, int> () {
			{ "Tutorial", 1 },
			{ "Easy", 1 },
			{ "Normal", 1 },
			{ "Hard", 1 },
			{ "Insane", 1 }
		};
		objectiveNeeded = objectiveNeededDict[diff];
	// ###############################################################
		Dictionary<string, int> objectiveValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 1500 },
			{ "Easy", 1500 },
			{ "Normal", 3000 },
			{ "Hard", 7500 },
			{ "Insane", 20000 } 
		};
		objectiveValue = objectiveValueDict[diff];
	// ###############################################################
		Dictionary<string, int> bonusValueDict = new Dictionary<string, int> () {
			{ "Tutorial", 250 },
			{ "Easy", 250 },
			{ "Normal", 500 },
			{ "Hard", 1000 },
			{ "Insane", 3000 } 
		};
		bonusValue = bonusValueDict[diff];
	// ###############################################################
		Dictionary<string, int> polarBearsDict = new Dictionary<string, int> () {
			{ "Tutorial", 3 },
			{ "Easy", 3 },
			{ "Normal", 6 },
			{ "Hard", 10 },
			{ "Insane", 15 } 
		};
		polarBears = polarBearsDict[diff];
	// ###############################################################
		Dictionary<string, float> batteryLifeDict = new Dictionary<string, float> () {
			{ "Tutorial", 300 },
			{ "Easy", 600 },
			{ "Normal", 600 },
			{ "Hard", 600 },
			{ "Insane", 600 } 
		};
		batteryLife = batteryLifeDict[diff];
	// ###############################################################
		// Dictionary<string, int> objectiveTimeDict = new Dictionary<string, float> () {
		// 	{ "Tutorial", 300 },
		// 	{ "Easy", 300 },
		// 	{ "Normal", 300 },
		// 	{ "Hard", 300 },
		// 	{ "Insane", 300 } 
		// };
		// objectiveTime = objectiveTimeDict[diff];
	// ###############################################################
	}
}
