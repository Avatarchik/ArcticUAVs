using UnityEngine;
using System.Collections;
// ###############################################################
// OtterController controls the otter surface and dive animations
// The scared mechanic for the scaredzone are also in this function
// ###############################################################
public class OtterController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Public otter status vars
	public bool scared = false;
	public bool picTaken = false;
	// Private otter status vars (relating to animation)
	private bool surfaced = false;
	// Time vars
	private float randomDelay = 0f;
	private float spawnTime = 0f;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		picTaken = false;
		randomDelay = Random.Range(0,5);
		spawnTime = GameController.gameClock;
	}
	// ***************************************************************
	void Update () {
		// If we haven't surfaced, animate the otter to the surface
		if (!surfaced) surfaced = bringOtterToSurface(gameObject);
		// If the otter gets scared, and has already surfaced, animate a dive
		if (scared && surfaced) sendOtterToTheDeeps();
		// If the otter has been around for 8s + randDelay AND NOT tutorial, animate a dive
		if ( GameController.gameClock - spawnTime > 8 + randomDelay && !GameController.isTutorial) sendOtterToTheDeeps();	
	}
// ###############################################################
// Animation Functions
// ###############################################################
	// Animate the otter to the surface the otter starts | and needs to end _ (look at prefab)
	public bool bringOtterToSurface (GameObject otter) {
		if (otter.transform.eulerAngles.z > 3 && otter.transform.eulerAngles.z <358){
			otter.transform.position = Vector3.MoveTowards(otter.transform.position, new Vector3 (otter.transform.position.x-2,otter.transform.position.y+1.3f,0f), 0.035f);
			otter.transform.Rotate(new Vector3(0f,0f,2.5f));
			return false;
		} else {
			return true;
		}
	}
	// Animate the otter diving, also needs to register one less otter with OtterGenerator, so it can be replaced
	public void sendOtterToTheDeeps () {
		GameObject otter = gameObject; // Not needed, but isn't otter.blahblah easier to think about than gameObject.blahblah?
		float otterX = otter.transform.position.x;
		float otterY = otter.transform.position.y;
		// We animate by rotating it something like 180 degrees and then starting a really fast MoveTowards, once it's close to fully rotated
		if (otter.transform.eulerAngles.z < 85 || otter.transform.eulerAngles.z > 95) {
			otter.transform.Rotate(new Vector3(0f,0f,-5f));
			
			if (otter.transform.eulerAngles.z > 70 && otter.transform.eulerAngles.z < 110) {
				otter.transform.position = Vector3.MoveTowards(otter.transform.position, new Vector3 (otterX,otterY - 1f,0f), .20f);
			}

		} else {
			// Destroy the OtterWithRegion and tell OtterGenerator we're gone
			Destroy(OtterGenerator.otterToRegionObject[gameObject]);
			OtterGenerator.otterCount--;
		}
	}
// ###############################################################
// public function
// ###############################################################
	// Public function for PicRegionController to set picTaken true, which allows ScaredController to destroy it's redzone
	public void picCaptured () {
		picTaken = true;
	}
}