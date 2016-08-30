using UnityEngine;
using System.Collections;
// ###############################################################
// Bonus generator spawns the correct bonus for each mission between 3 and 7 seconds
// for MTI it generates a set number of coins
//
// Additionally you can automaticCoinSettings(false) and discretely spawn coins
// ###############################################################
public class BonusGenerator : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Timing variables
	private float coinSpawnTime = 0;
	private float spawnInterval = 5f;
	private float randomInterval;
	// MTI Vars
	private MazeGenerator maze;
	// Mission types
	private bool scoutMission = false; 	//generic scout mission coins (LTF & LITH)
	private bool GTPMission = false; 	// GTP Mission, clams
	private bool MTIMission = false;	// MIT Mission, salmon
	// Spawn variables
	private float leftBound;
	private float rightBound;
	public static bool automaticCoins = true;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// default operation is automatic coin spawns
		automaticCoins = true;
		// Figure out which mission we are:   LTF & LITH | GTP | MTI
		if (GameController.missionInitials == "MTI") {
			maze = GameObject.Find ("Maze Generator").GetComponent<MazeGenerator>();
			MTIMission = true;
		} else if (GameController.missionInitials == "GTP") {
			GTPMission = true;
		} else {
			scoutMission = true;
		}
		// IF tutorial, disable auto-coins
		if (GameController.isTutorial) automaticCoinsSetting(false);
		// Grab the clock
		coinSpawnTime = GameController.gameClock;
	}
	// ***************************************************************
	void Update () {
		// Select a random interval for spawning (we get a goin from 3 - 7 seconds)
		randomInterval = Random.Range(-2f,2f);
		// Spawn the coins just inside of the outer bounds (weird for 4:3)
		leftBound = GameObject.Find("LeftSpawnEdge").gameObject.transform.position.x + 1.5f;
		rightBound = GameObject.Find("RightSpawnEdge").gameObject.transform.position.x - 1.5f;
		// Control automatic coins for each mission
		if (automaticCoins) {
			if (MTIMission){
				if (GameObject.Find ("Bonus Generator").transform.childCount <= 10) {
					spawnMTICoins(getValidPosition (new Vector2(0, maze.width - 1), new Vector2(0, maze.height - 1)));
				}
			}
			if (GTPMission) {
				if (GameController.gameClock - coinSpawnTime > spawnInterval+randomInterval) {
					Vector3 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
					// get random coordinates from the camera view
					Vector3 spawnLocation = new Vector3 (Random.Range (leftBound, rightBound), Random.Range (-screenDimensions.y + 2.5f, screenDimensions.y - 1.3f));
					spawnGTPCoins(spawnLocation);
				}
			} 
			if (scoutMission) {
				if (GameController.gameClock - coinSpawnTime > spawnInterval+randomInterval) {
					Vector3 screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
					// get random coordinates from the camera view
					Vector3 spawnLocation = new Vector3 (Random.Range (leftBound,rightBound), Random.Range (-screenDimensions.y + 2f, screenDimensions.y - 1f));
					spawnScoutCoins(spawnLocation);
				}
			} 
		}
	}
// ###############################################################
// Public function
// ###############################################################
	public void automaticCoinsSetting (bool setting) {
		automaticCoins = setting;
	}
// ###############################################################
// Coin spawning functions
// ###############################################################
	public void spawnScoutCoins (Vector3 position) {
		coinSpawnTime = GameController.gameClock;
		GameObject coins = Instantiate(Resources.Load<GameObject>("GamePrefabs/Coins"));
		coins.transform.position = position;
	}
	// ***************************************************************
	public void spawnGTPCoins (Vector3 position) {
		coinSpawnTime = GameController.gameClock;
		GameObject coins = Instantiate(Resources.Load<GameObject>("GamePrefabs/GTPCoins"));
		coins.transform.position = position;
	}
	// ***************************************************************
	public void spawnMTICoins (Vector3 position) {
		GameObject coins = Instantiate(Resources.Load<GameObject>("GamePrefabs/MTICoins"));
		coins.transform.parent = GameObject.Find ("Bonus Generator").transform;
		coins.transform.position = position;
	}
	private Vector3 getValidPosition (Vector2 xBounds, Vector2 zBounds) {
		bool valid = false;
		Vector3 position = new Vector3 ();
		while (!valid) {
			position = new Vector3 (
				(int) Random.Range (xBounds.x, xBounds.y) * maze.scale, 0f, 
				(int) Random.Range (zBounds.x, zBounds.y) * maze.scale
			);
			foreach (Transform child in maze.gameObject.transform) {
				Vector3 checkPosition = child.position;
				checkPosition.x += maze.scale / 2;
				checkPosition.z += maze.scale / 2;
				if (position == checkPosition) {
					valid = false;
				}
			}
			valid = true;
		}
		return position;
	}
}