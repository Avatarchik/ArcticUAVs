using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// ###############################################################
// SC: LTF_Mission
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// LTF_Mission checks for victory conditions and makes circles
// ###############################################################
public class LTF_Mission : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private Vector3 screenDimensions;
	private Rigidbody2D uav;
// ###############################################################
// Unity Functions
// ###############################################################
	void Awake () {
		// init diffs
		Difficulty.loadLTFDiff();
	}
	void Start () {
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		// Connect to UI
		GameController.missionInitials = "LTF";
		GameController.challenge = "Hover in the circles";
		GameObject.Find("Circles Needed").GetComponent<Text>().text = Difficulty.objectiveNeeded.ToString();
		// Create first circle
		makeCircle(randomPosition());
	}
	void Update () {
		GameObject.Find ("Circles Hovered").GetComponent<Text> ().text = GameController.objectiveGained.ToString ();
		if (!GameController.circleOnScreen) {
			makeCircle(randomPosition ());
		} 
		checkForLevelCompletion();
	}
// ###############################################################
// LTF Mission Functions
// ###############################################################
	// [FUTURE WORK: base the circles on the left and right spawns!]
	private Vector3 randomPosition () {
		Vector3 position;
		do {
			position = new Vector3 (
				Random.Range (-screenDimensions.x + (0.138f * screenDimensions.x), 
					screenDimensions.x - (0.138f * screenDimensions.x)
				),
				Random.Range (-screenDimensions.y + (0.234f * screenDimensions.y), 
					screenDimensions.y - (0.234f * screenDimensions.y)
				)
			);
		} while (Vector3.Distance (GameObject.Find ("uav").transform.position, position) < 0.276f * screenDimensions.x);
		return position;
	}
	void makeCircle (Vector3 position) {
	    GameObject dottedCircle = new GameObject ("Dotted Circle");
        SpriteRenderer renderer = dottedCircle.AddComponent<SpriteRenderer> ();
        renderer.sprite = Resources.Load<Sprite> ("Sprites/DottedCircle");
        renderer.sortingLayerName = "Foreground";
        renderer.sortingOrder = 1;
        dottedCircle.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
		dottedCircle.transform.position = position;
        dottedCircle.AddComponent<CircleCollider2D> ();
        dottedCircle.GetComponent<CircleCollider2D> ().isTrigger = true;
		dottedCircle.GetComponent<CircleCollider2D> ().radius = .14f * screenDimensions.x;
        dottedCircle.AddComponent<CircleController> ();
        GameController.circleOnScreen = true;
	}
	void checkForLevelCompletion () {
		if (Difficulty.objectiveNeeded == GameController.objectiveGained) {
			GameController.won = true;
			GameController.inPlay = false;
			GameController.message = "You won!";
		}
	}
}
