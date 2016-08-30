using UnityEngine;
using System.Collections;
// ###############################################################
// BoostsGenerator generates boosts in the scouts missions
// By default, you can just add a GameObject with this script attached And the left and right spawn game objects
// This script will automatically start generating boosts on screen
//
// You can connect to this instance of boosts generator and disable the auto boosts with automaticBoostsSetting(bool)
// You can manually spawn boosts with spawnScoutBoosts (Vector3)
// ###############################################################
public class BoostsGenerator : MonoBehaviour {
// ###############################################################
// Variables
// ###############################################################
	private bool scoutMission = false; 	//generic scout mission (LTF & LITH & GTP)
	private bool automaticBoosts = true; // should boosts automatically spawn, inits to true
	private float boostsSpawnTime = 0;	// When the boost spawned
	private float spawnInterval = 6f;	// How often boosts spawn
	private float leftBound;  // left spawn bound
	private float rightBound; // right spawn bound
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// In this implementation, MTI means nothing, see top boosts spawner or something
		if (GameController.missionInitials == "MTI") {
			automaticBoostsSetting(false);
		} else if (GameController.missionInitials == "LITH") {
			print("no boosts for lith");
		} else{
			scoutMission = true;
		}
		// if it's a tutorial, stop the autoboosts
		if (GameController.isTutorial) automaticBoostsSetting(false);
		// first coins spawned at gameClock
		boostsSpawnTime = GameController.gameClock;	
	}
	void Update () {
		// Update left and right bounds to the current camera position
		leftBound = GameObject.Find("LeftSpawnEdge").gameObject.transform.position.x + 5f;
		rightBound = GameObject.Find("RightSpawnEdge").gameObject.transform.position.x - 5f;
		// Handle spawning coins
		if (automaticBoosts) {
			if (scoutMission) {
				if (GameController.gameClock - boostsSpawnTime > spawnInterval) {
					Vector3 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
					// get random coordinates from the camera view
					Vector3 spawnLocation = new Vector3 (Random.Range (leftBound,rightBound), Random.Range (-screenDimensions.y + 2f, screenDimensions.y - 1f));
					spawnScoutBoosts(spawnLocation);
				}
			}	 
		}
	}
// ###############################################################
// Public Functions
// ###############################################################
	public void automaticBoostsSetting (bool setting) {automaticBoosts = setting;}
	// ***************************************************************
	public void spawnScoutBoosts (Vector3 position) {
		boostsSpawnTime = GameController.gameClock;
		GameObject coins = Instantiate(Resources.Load<GameObject>("GamePrefabs/ScoutBoost"));
		coins.transform.position = position;
	}
}