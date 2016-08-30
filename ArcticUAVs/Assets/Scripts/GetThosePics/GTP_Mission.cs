using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// ###############################################################
// SC: GTP_Mission
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// GTP has opening animations and programatically extends the map
// ###############################################################
public class GTP_Mission : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	public static int copies = 0;
	public static bool localWon = false;
	private bool picturesCaptured = false;
	private GameObject landingCircle;
// ###############################################################
// Unity Functions
// ###############################################################
	void Awake () {
		// init diffs
		Difficulty.loadGTPDiff();
	}
	// ***************************************************************
	void Start () {
		// GameController variables init
		GameController.missionInitials = "GTP";
		GameController.challenge = "Get Pictures of Otters";
		// local vars init (OtterGenerator uses copies; PicPanel uses localWon)
		copies = 0;
		localWon = false;
		// Update UI
		GameObject.Find ("Needed Pics").GetComponent<Text> ().text = Difficulty.objectiveNeeded.ToString ();
		// Start animations
		StartCoroutine (pauseBeforePan ()); // Pan to UAV from city
	}
	// ***************************************************************
	void Update () {
		// Update UI
		GameObject.Find ("Captured Pics").GetComponent<Text> ().text = GameController.objectiveGained.ToString ();
		// Update graphics
		handleMapExtension ();
		if (GameObject.Find("landingCircle(Clone)")) landingCircle.transform.Rotate(new Vector3(0f,0f,0.75f));
		// check to see if all of the pictures have been taken; if true, draw the landing circle and display "return to dock" panel
		if (!picturesCaptured) {
			if (GameController.objectiveGained >= Difficulty.objectiveNeeded) {
				picturesCaptured = true;
				GameObject.Find("Canvas").transform.Find("Message Panel").gameObject.SetActive(true);
				landingCircle = Instantiate(Resources.Load<GameObject>("GamePrefabs/landingCircle"), new Vector3(-1.08f,-2.22f),Quaternion.identity) as GameObject;
		}	}
		// if you have all the pics, you're landed, haven't already won, and aren't moving, you win
		if (picturesCaptured && GameController.started && !GameController.lost && GameController.landed && !localWon && GameObject.Find ("uav").GetComponent<Rigidbody2D> ().velocity.Equals (Vector2.zero)) { 
			GameController.buildGameObject("UIPrefabs/ViewPicturesPanel",new Vector2(0f,0f), new Vector2(0f,0f)); 
		}
	}
// ###############################################################
// Start Animation Functions
// ###############################################################
	private IEnumerator pauseBeforePan () {
		yield return new WaitForSeconds (1f);
		StartCoroutine (panFromHomer ());
	}
	private IEnumerator panFromHomer () {
		Vector3 target = Vector3.zero;
		target.z = -10;
		if (GameObject.Find ("Main Camera").transform.localPosition != target) {
			GameObject.Find ("Main Camera").transform.localPosition = Vector3.MoveTowards (GameObject.Find ("Main Camera").transform.localPosition, target, 2f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
			StartCoroutine (panFromHomer ());
		}
	}
// ###############################################################
// Extend Map Functions: If you're close to the boundry, copy the background and widen the waves
// ###############################################################
	private void handleMapExtension () {
		if (GameObject.Find ("Camera Controller").transform.position.x >= (20 * (copies-1))) {
			copies++;
			makeNewBackground ();
			widenWaves ();
		}
	}
	private void makeNewBackground () {
		GameObject newBackground = Instantiate (Resources.Load<GameObject>("GamePrefabs/GTPBackground"));
		newBackground.transform.parent = GameObject.Find("Homer").transform;
		newBackground.transform.localPosition = new Vector3 (20 * copies, 2.9f);
		newBackground.SetActive(true);
		for (int i = 0; i < newBackground.transform.childCount-2; i++) { // For each object in background (excluding the base objects), hide or show randomly
			newBackground.transform.GetChild (i).gameObject.SetActive (Random.value > 0.5f);
		}
	}
	private void widenWaves () {
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 4; j++) {
				GameObject original = GameObject.Find ("Waves").transform.GetChild (i).transform.GetChild (j).gameObject;
				GameObject newObject = Instantiate (original);
				newObject.transform.parent = original.transform.parent;
				newObject.transform.position = new Vector3 (original.transform.position.x + (20 * copies),
					original.transform.position.y);
				newObject.transform.localScale = original.transform.localScale;
			}
		}
	}
}