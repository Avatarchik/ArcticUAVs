using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// TopCoinsController is responsible for handling the coin
// collection in the MTI mission.
// ###############################################################
public class TopCoinsController : MonoBehaviour {

	private List<string> validConsumers = new List<string> { "Whaler", "Follower 1", "Follower 2", "Follower 3" };
	private AudioSource sound;

	// Initialization (called once when gameObject is created).
	void Start () {
		// setup sound
		sound = GetComponent<AudioSource>();
	}

	// Called when the collider is entered by another
	void OnTriggerEnter (Collider collider) {
		if (validConsumers.Contains(collider.gameObject.name)) {
			if (GameController.sfxStatus) sound.Play();
			// call "destroyCoins" after the sound is done playing
			Invoke ("destroyCoins", sound.clip.length);
			GetComponent<SpriteRenderer>().enabled = false;
			PointsController.bonusGained += 1;
		}
	}

	void destroyCoins () {
		Destroy (gameObject);
	}
}
