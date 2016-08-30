using System.Collections;
using UnityEngine;

/*
	This is the MapBirdController class, responsible for the movement and behavior of the birds in the MapThatIce scene.
*/
public class MapBirdController : MonoBehaviour {

/*
	Class Variables 
*/
	private int maxX;
	private int maxY;
	private MazeGenerator maze;
	private bool minX = false;
	private Vector3 target;

/*
	Class Functions 
*/
	// Initialization (called once when gameObject is created).
	void Start () {
		initializeValues ();
        changeTarget();
	}
	// Called once per frame
	void Update () {
		moveBird ();
	}
	// Called on collision
	void OnTriggerEnter (Collider other) {
		if (other.gameObject.name == "uav") {
			GameController.message = "BIRD STRIKE!";
			GameController.lost = true;
		}
	}

/*
	Content and Helper Functions 
*/
	private void changeTarget () {
		target = new Vector3 (minX ? 0 : 120, 20, Random.Range (0, minX ? maxY : maxX));
		minX = !minX;
	}

	private void handleBirdPosition () {
		if (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, 5 * Time.deltaTime);
		} else {
			changeTarget ();
		}
	}

	private void handleBirdRotation () {
		if (target - transform.position != Vector3.zero) {
			var newRotation = Quaternion.LookRotation (target - transform.position).eulerAngles;
			newRotation.x = Mathf.Clamp (90, 90, 90);
			newRotation.z = 0;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 2);
		}
	}

	private void initializeValues () {
		maze = GameObject.Find ("Maze Generator").GetComponent<MazeGenerator>();
		maxX = maze.width * maze.scale;
		maxY = maze.height * maze.scale;
		minX = Random.value > 0.5f;
	}

	private void moveBird () {
		handleBirdRotation ();
		handleBirdPosition ();
	}
}
