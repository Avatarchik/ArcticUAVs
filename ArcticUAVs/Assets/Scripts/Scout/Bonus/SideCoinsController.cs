using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ###############################################################
// Side Coins Controller watches the coins for a collision with the UAV
// ###############################################################
public class SideCoinsController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private AudioSource sound;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		sound = GetComponent<AudioSource>();
		if (GameController.missionInitials != "LTF" || GameController.missionDiff != "Tutorial") Invoke ("destroyCoins", 6);
	}
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.gameObject.name == "uav") {
			if (GameController.sfxStatus) sound.Play();
			// Destroy coin, but wait unitl the sound finishes
			Invoke ("destroyCoins", sound.clip.length);
			// Hide the Coin until it gets destroyed
			GetComponent<SpriteRenderer>().enabled = false;
			// add bonus gained
			PointsController.bonusGained++;
		}
	}
// ###############################################################
//  Destroy function because invoke is a thing
// ###############################################################
	void destroyCoins () {
		Destroy (gameObject);
	}
}
