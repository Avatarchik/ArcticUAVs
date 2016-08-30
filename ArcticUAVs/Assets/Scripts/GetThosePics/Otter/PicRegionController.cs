using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// ###############################################################
// Pic Region Controller watches a trigger zone above the otter, if the UAV
// hovers in the region for 1.5 seconds, a *PIC* occurs, the script
// *FLASHES* a canvas image, destroys the picregion UI elements, and then destroys itself
// We also tell the otter we took a pic so that scaredzone can destroy itself
// ###############################################################
public class PicRegionController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Picture taking variables
	private float triggerEnterTime = 0f;
	private bool takingPic = false;
	private bool picCaptured = false;
	private float deltaAlpha;
	private bool picRegionDestroyed = false;
	// flash animation
	private Image canvasImage;
	// Region UI Elements
	private SpriteRenderer cameraIconRenderer;
	private SpriteRenderer picRegion;
	private GameObject leftEdge;
	private GameObject rightEdge;
	private GameObject cameraIcon;
	// This regions otter
	private OtterController thisOtter;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// set how fast the camera darkens
		deltaAlpha = Time.deltaTime / 2f; // Set amount the camera icon alpha will increase for each frame
		// Connect to UI elements to destroy
		leftEdge = transform.Find("Left Edge").gameObject;
		rightEdge = transform.Find("Right Edge").gameObject;
		cameraIcon = transform.Find("Camera Icon").gameObject;
		cameraIconRenderer = cameraIcon.GetComponent<SpriteRenderer>();
		picRegion = gameObject.GetComponent<SpriteRenderer>();
		// Connect to canvas for "Flash"
		canvasImage = GameObject.Find("Canvas").GetComponent<Image>();
		// If Tutorial or Easy make pic region green
		if (GameController.missionDiff == "Tutorial" || GameController.missionDiff == "Easy") {
			picRegion.color = new Vector4 (1f,1f,1f,75f/255f);
		}
		// connect to our otter
		thisOtter = OtterGenerator.picRegionToOtterController[gameObject];
	}
	// ***************************************************************
	void Update () {
		// if we have been in the trigger zone longer than 1.5 seconds, and were taking a pic, *PIC*
		if (GameController.gameClock - triggerEnterTime > 1.5f && takingPic) {
			takingPic = false;
			picCaptured = true;
		}
		// increase alpha of camera, so we have visual feedback for picture taking
		if (takingPic) {
			cameraIconRenderer.color = new Vector4 (1f, 1f, 1f, cameraIconRenderer.color.a+deltaAlpha);
		}
		// if we have taken a picture
		if (picCaptured) {
			if (!picRegionDestroyed) {
				// Tell our otter *PIC*
				thisOtter.picCaptured ();
				// *FLASH*
				canvasImage.color = new Vector4 (1f,1f,1f,1f);
				// We took a picture
				GameController.objectiveGained++;
				// Destroy this regions trigger collider, the left and right dots, the camera, and ourselves
				Destroy(GetComponent<BoxCollider2D>());
				Destroy(leftEdge);
				Destroy(rightEdge);
				Destroy(cameraIconRenderer);
				Destroy(gameObject);
				picRegionDestroyed = true;
			}
		}
	}
	// ***************************************************************
	void OnTriggerEnter2D (Collider2D collider) {
		if (!takingPic) { 
			// Start the clock
			triggerEnterTime = GameController.gameClock;
			takingPic = true;
		}
	}
	// ***************************************************************
	void OnTriggerExit2D (Collider2D collider) {
		//stop taking the pic, tranparent the camera icon
		takingPic = false;
		cameraIconRenderer.color = new Vector4 (1f, 1f, 1f, 0);
	}
}
