using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ###############################################################
// Battery Bar Controller is responsible for the battery charge mechanic
// The life spans are saved in Difficulites per difficulty per mission
// the battery drains based on time. For example, the LTF_Tutorial lifespan is 8 seconds
// this script calculate a % based on that lifespan and time elapsed, and updates the HUD element
// if the % drops below, the HUD element turns red
// ###############################################################
public class BatteryBarController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// charge variables
	private float lifeSpan;
	private float charge;
	private float drainStart;
	private float chargePercentage;
	// UI elements
	private RectTransform batterBar;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// Connect to difficulties for battery life
		lifeSpan = Difficulty.batteryLife;
		// Save this time point as the start of draining
		drainStart = GameController.gameClock;
		// Connect to UI Element
		batterBar = GameObject.Find("Battery Bar").GetComponent<RectTransform>();
	}
	// Update the current charge, charge percentage, UI Element, game status(has the battery 100% drained)
	void Update () {
		charge = GameController.gameClock - drainStart;
		chargePercentage = (lifeSpan - charge)/lifeSpan;
		if (chargePercentage > 0.2f) {
			greenBar();
		} else {
			redBar();
		}
		batterBar.anchorMax = new Vector2(1f, chargePercentage);
		batterBar.offsetMax = new Vector2(0f, 0f);
		batterBar.offsetMin = new Vector2(0f, 0f);
		checkPower ();
	}
// ###############################################################
// Battery bar Functions
// ###############################################################
	private void greenBar () {GetComponent<Image> ().sprite = Resources.Load<Sprite>("Sprites/green_bar");}
	// ***************************************************************
	private void redBar () {GetComponent<Image> ().sprite = Resources.Load<Sprite>("Sprites/red_bar");}
	// ***************************************************************
	private void checkPower () {
		// When the battery bar gets to 0%, the game is over
		if (0f >= chargePercentage) {
			// Show failing message
			GameController.message = ScoutController.deadBattery;
			GameController.lost = true; // Disable control of the UAV (crash), ran out of batteries
			GameController.inPlay = false;
		}
	}
// ###############################################################
// Public Functions: SideBatteryBoosts call this to reset the battery when a boost is grabbed
// ###############################################################
	public void gainPower () {
		drainStart = GameController.gameClock;
	}
// ###############################################################
// planned functions
// ###############################################################
	// [FUTURE WORK: Implement battery drains based on wind effects]
	// This function is not called as of 7/18/2016 JTN is the 🐐
	public void changeBatteryLife(float newLifeSpan) {
		lifeSpan = newLifeSpan;
		drainStart = GameController.gameClock - (newLifeSpan * chargePercentage);
	}
}