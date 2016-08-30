using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// NanookGenerator is resposible for spawning polar bears in 
// legal places throughout the maze in the.
// ###############################################################
public class NanookGenerator : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	private MazeGenerator maze;
	private List<Vector3> nanookPositions = new List<Vector3> ();
	private int nanookSpacing;
	private Vector2 xBounds;
	private Vector2 zBounds;
	private bool polarBearsSpawned = false;

// ###############################################################
// Functions
// ###############################################################
	void Start () {
		// determine the polar bear spacing based on difficulty 
		if (GameController.missionDiff == "Insane") {
			nanookSpacing = 3;
		} else {
			nanookSpacing = 5;
		}
		// connect to the maze
		maze = GameObject.Find ("Maze Generator").GetComponent<MazeGenerator>();
		xBounds = new Vector2(0, maze.width - 1);
		zBounds = new Vector2(6/2, maze.height - 1);
		// start spawning polar bears
		polarBearsSpawned = false;
	}

	void Update() {
		if (!polarBearsSpawned) {
			polarBearsSpawned = true;
			spawnPolarBears ();
			// Nanook Generator doesn't need to be running during the MTI mission,
			// So destroy it after generating the Polar Bears
			Destroy(gameObject.GetComponent<NanookGenerator>());
		}
	}

	// Spawn polar bears
	private void spawnPolarBears() {
		for (int i = 0; i < Difficulty.polarBears; i++) {
			buildPolarBear (getValidPosition ());
		}
	}

	private void buildPolarBear (Vector3 position) {
		// initialize polar bear game object
		GameObject polarBear = Instantiate(Resources.Load<GameObject>("GamePrefabs/Nanook"));
		polarBear.transform.parent = GameObject.Find ("Nanooks Controller").transform;
		// elevate the polar bear above the terrain blocks
		// this eliminates the head going through the blocks
		position.y += 2.5f;
		polarBear.transform.position = position;
	}

	// get a random position inside the maze and make sure it is not inside an ice block
	private Vector3 getValidPosition () {
		bool valid = false;
		Vector3 position = new Vector3 ();
		// while there is not valid position, try and get a valid position
		while (!valid) {
			// get a random x,z position
			position = new Vector3 (
				(int) Random.Range (xBounds.x, xBounds.y) * 6, 
				0f, 
				(int) Random.Range (zBounds.x, zBounds.y) * 6
			);
			valid = true;
			// check if the random position is inside a maze block
			foreach (Transform child in maze.gameObject.transform) {
				Vector3 checkPosition = child.position;
				checkPosition.x += 6 / 2;
				checkPosition.z += 6 / 2;
				// if the random position is inside an ice block
				if (position == checkPosition) {
					valid = false;
				}
			}
			// check if another polar bear is too close to another polar bear
			foreach (Vector3 nanook in nanookPositions) {
				if (Vector3.Distance (position, nanook) <= nanookSpacing * 6) {
					valid = false;
				}
			}
		}
		// add the position to the list of the position to be checked against later
		nanookPositions.Add (position);
		return position;
	}
}
