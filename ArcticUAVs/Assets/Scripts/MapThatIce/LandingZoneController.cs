using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ###############################################################
// LandingZoneController is responsible for animatin, changing 
// the text, and landing or launching the UAV
// ###############################################################
public class LandingZoneController : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	private float delay = 0.2f;
	private float doubleTapTimer;
	private int goalMarker = 1;
	private bool oneTap = false;
	private float targetX;
	private float uavAltitude = 20;
	public bool showLandingPanel = true;
	private Vector3 landingPanelOn;
	private Vector3 landingPanelOff;

	// game objects
	private GameObject uav;
	private BoxCollider landingZone;
	private GameObject landingPanel;
	private Text landingMessage;
	private Image landingIcon;

// ###############################################################
// Unity Functions 
// ###############################################################
	// Use this for initialization
	void Start () {
		uav = GameObject.Find ("uav");
		landingZone = GetComponent<BoxCollider> ();
		landingPanel = GameObject.Find ("Land Launch Button");
		landingMessage = landingPanel.transform.Find ("Text").GetComponent<Text> ();
		landingIcon = landingPanel.transform.Find ("Icon").GetComponent<Image> ();
		landingPanelOff = landingPanel.transform.position;
		landingPanelOn = GameObject.Find ("LandOnPosition").transform.position;
		targetX = GameObject.Find ("uav").transform.position.x;
	}

	// Called once per frame
	void Update () {
		// Pause check
		bool PauseMenuCheck = GameObject.Find("PauseMenu(Clone)") != null;
		landingPanel.SetActive(!PauseMenuCheck);

		bool gameConditionCheck = !GameController.lost && !GameController.won && GameController.started;
		if (showLandingPanel && landingZone.bounds.Contains (uav.transform.position) && gameConditionCheck) {
			// Handle the showing of the takeoff/landing message panel
			if (landingPanel.transform.position != landingPanelOn) { // The message is not "showing"
				// So, show it
				moveLandingMessage (landingPanelOn);
			}
		} else {
			// Handle the hiding of the takeoff/landing message panel
			if (landingPanel.transform.position != landingPanelOff) { // The message is not "hidden"
				// So, hide it
				moveLandingMessage (landingPanelOff);
			}
		}
		// if
		if (GameController.onGround) {
			landingIcon.sprite = Resources.Load<Sprite>("Sprites/uav_launch");
			landingMessage.text = "Launch";
		} 
		if (!GameController.landed) {
			landingIcon.sprite = Resources.Load<Sprite>("Sprites/uav_land");
			landingMessage.text = "Land";
		}
	}

// ###############################################################
// LandingZone Functions 
// ###############################################################
	// animate the land or launch button
	private void moveLandingMessage (Vector3 target) {
		if (landingPanel.transform.position != target) {
			landingPanel.transform.position = Vector3.MoveTowards (landingPanel.transform.position, target, 650f * Time.deltaTime);
		}
	}
	// launch or land the UAV based on whether it is flying or landed
	public void landOrLaunch() {
		print("land or launch called");
		print(GameController.onGround);
		print(GameController.landed);
		// On the ground
		if (GameController.landed && GameController.onGround) { 
			GameController.onGround = false;
			StartCoroutine (launch ());
		// At flying altitude
		} else if (uav.transform.position.y == uavAltitude) { 
			GameController.landed = true;
			StartCoroutine (land ());
		}
	}
	// launch the UAV
	private IEnumerator launch () {
		print("Launch Activated");
		yield return new WaitForSeconds(2);
		Vector3 target = new Vector3 (targetX, uavAltitude, 0f);
		while (GameController.landed) {
			if (uav.transform.position != target) {
				uav.transform.position = Vector3.MoveTowards (uav.transform.position, target, 10f * Time.deltaTime);
				yield return new WaitForEndOfFrame ();
			} else {
				GameController.landed = false;
			}
		}
	}
	// Land the UAV
	private IEnumerator land () {
		print("Land Activated");
		Vector3 target = new Vector3 (targetX, 1.2f, -8);
		if (uav.transform.position != target) {
			uav.transform.position = Vector3.MoveTowards (uav.transform.position, target, 5f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
			StartCoroutine (land ());
		} else {
			GameController.onGround = true;
		}
	}
}
