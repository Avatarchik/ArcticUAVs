using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	This is the MapBirdGenerator class, responsible for spawning a certain number of birds in the MapThatIce scene depending
	on difficulty level.
*/
public class MapBirdGenerator : MonoBehaviour {

/*
	Class Variables 
*/
	private int numberOfBirds;
	private Dictionary<string, int> numbersOfBirds = new Dictionary<string, int> {
		{ "Basic", 0 }, { "Easy", 3 }, { "Normal", 6 }, { "Hard", 9 }, { "Insane", 12 }
	};

/*
	Class Functions 
*/
	// Initialization (called once when gameObject is created).
	void Start () {
		numberOfBirds = numbersOfBirds [PlayerPrefs.GetString ("Difficulty")];
		spawnBirds ();
	}

/*
	Content and Helper Functions 
*/
	private void setupBird (int centerX, int centerY, int idx) {
		GameObject bird = new GameObject (name : "bird " + idx);
		bird.transform.parent = GameObject.Find ("Birds Controller").transform;
		bird.transform.position = new Vector3 (centerX, 20, centerY);
		bird.AddComponent<MapBirdController> ();
		setupBirdRenderer (bird.AddComponent<SpriteRenderer> ());
		bird.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
		BoxCollider birdCollider = bird.AddComponent<BoxCollider> ();
		birdCollider.isTrigger = true;
		Rigidbody birdRigidBody = bird.AddComponent<Rigidbody> ();
		birdRigidBody.isKinematic = true;
	}

	private void setupBirdRenderer (SpriteRenderer renderer) {
		renderer.sortingOrder = 5;
		renderer.sprite = Resources.Load<Sprite> ("Sprites/bird_topview");
	}

	private void spawnBirds() {
		MazeGenerator maze = GameObject.Find ("Maze Generator").GetComponent<MazeGenerator>();
		for (int i = 0; i < numberOfBirds; i++) {
			setupBird (maze.width * maze.scale / 2, maze.height * maze.scale / 2, i);
		}
	}
}
