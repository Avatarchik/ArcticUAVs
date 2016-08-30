using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ###############################################################
// BatteryBostsController is attached to the boost game objects and detects trigger enters
// with the uav. If the uav enters the trigger, the battery bar controller is told to gain power
// ###############################################################
public class BatteryBoostController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private AudioSource sound;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// Destroy the coin after 4 seconds
		if (!GameController.isTutorial) Invoke("destroyBoost", 4);
		// Connect to the prefab's sound
		sound = GetComponent<AudioSource>();
	}
	void OnTriggerEnter2D (Collider2D collider) {
		// if the UAV collides with the battery
		if (collider.gameObject.name == "uav") boostAttained ();
	}
// ###############################################################
// Battery Boost Functions
// ###############################################################
	private void destroyBoost () {Destroy (gameObject);}

	private void boostAttained () {
		// play the battery sound
		if (GameController.sfxStatus) sound.Play();
		// destroy the battery after the clip plays
		Invoke ("destroyBoost", sound.clip.length);
		// destroy the sprite immediately
		Destroy(gameObject.GetComponent<SpriteRenderer>());
		// Tell the battery bar that the boost was consumed
		GameObject.Find ("Battery Bar").GetComponent<BatteryBarController> ().gainPower ();
	}
}
