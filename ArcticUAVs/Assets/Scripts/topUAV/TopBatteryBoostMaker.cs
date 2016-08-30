using System.Collections;
using UnityEngine;

// ###############################################################
// TopBatteryBoostMaker is responsible for making the battery 
// boosts for the MTI mission.
// ###############################################################
public class TopBatteryBoostMaker : MonoBehaviour {

	// Initialization (called once when gameObject is created).
	void Start () {
		InvokeRepeating ("spawnBoost", 0, 0.1f);
	}
	// Spawn up to 5 battery boosts in the MTI maze
	private void spawnBoost () {
		if (GameObject.Find ("Battery Boost Controller").transform.childCount < 5) {
			MazeGenerator maze = GameObject.Find ("Maze Generator").GetComponent<MazeGenerator>();
			GameObject boost = Instantiate(Resources.Load<GameObject>("GamePrefabs/MTIBattery"));
			boost.transform.parent = GameObject.Find ("Battery Boost Controller").transform;
			boost.transform.position = getValidPosition (
				maze, new Vector2(0, maze.width - 1), new Vector2(0, maze.height - 1)
			);
		}
	}
	// Get a valid position from the maze. So that boosts are not inside the ice blocks
	private Vector3 getValidPosition (MazeGenerator maze, Vector2 xBounds, Vector2 zBounds) {
		bool valid = false;
		Vector3 position = new Vector3 ();
		while (!valid) {
			position = new Vector3 (
				(int) Random.Range (xBounds.x, xBounds.y) * maze.scale, 0f, 
				(int) Random.Range (zBounds.x, zBounds.y) * maze.scale
			);
			valid = pointIsValid (maze, position);
		}
		return position;
	}
	// check that the given point is not inside an ice block in the maze.
	private bool pointIsValid (MazeGenerator maze, Vector3 position) {
		foreach (Transform child in maze.gameObject.transform) {
			Vector3 checkPosition = child.position;
			checkPosition.x += maze.scale / 2;
			checkPosition.z += maze.scale / 2;
			if (position == checkPosition) {
				return false;
			}
		}
		return true;
	}
}
