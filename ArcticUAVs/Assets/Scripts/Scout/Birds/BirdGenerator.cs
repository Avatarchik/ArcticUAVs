using UnityEngine;
using System.Collections;
// ###############################################################
// Bird generator spawns a bird on a random side based on an interval from Difficulty
//
// Alternatively, you can automaticBirdSetting(False) and makeBird (side) (technically it accepts "left" or "*")
// ###############################################################
public class BirdGenerator : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// init vars
	private GameObject newBird;
	float rightSideStart = 10f;
	float leftSideStart = -10f;
	// spawn timing
	float birdSpawnTimePoint;
	// bird settings
	public static bool automaticBirds = true;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// default operation
		automaticBirds = true;
		// connect to spawn zone
		rightSideStart = GameObject.Find("RightSpawnEdge").transform.position.x;
		leftSideStart = GameObject.Find("LeftSpawnEdge").transform.position.x;
		// Grab the current time
		birdSpawnTimePoint = GameController.gameClock;
	}
	void Update () {
		// Connect to spawn zone (FOR GTP)
		rightSideStart = GameObject.Find("RightSpawnEdge").transform.position.x;
		leftSideStart = GameObject.Find("LeftSpawnEdge").transform.position.x;
		// Control birds
		if (GameController.gameClock - birdSpawnTimePoint > Difficulty.birdSpawnTime && automaticBirds) {
			birdSpawnTimePoint = GameController.gameClock;
			if (Random.value > 0.5f) {
				makeBird("left");
			} else {
				makeBird("right");
			}
		}
	}
// ###############################################################
// Bird creation Functions
// ###############################################################
	public void makeBird (string side) {
		if (side == "left") {
			newBird = Instantiate(Resources.Load<GameObject>("GamePrefabs/leftBird"));
			newBird.transform.parent = gameObject.transform;
			newBird.transform.position = new Vector3 (leftSideStart,Random.Range(-0.55f,4.2f));
		} else {
			newBird = Instantiate(Resources.Load<GameObject>("GamePrefabs/RightBird"));
			newBird.transform.parent = gameObject.transform;
			newBird.transform.position = new Vector3 (rightSideStart,Random.Range(-0.55f,4.5f));
		}
	}
// ###############################################################
// Public Functions
// ###############################################################
	public void automaticBirdSetting (bool setting) {
		automaticBirds = setting;
	}
}