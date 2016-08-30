using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// WhalerControlleris responsible for the movement
// and animation of the leading eskimo in the MTI scenes.
// It looks for a "Placed Marker" in the scene and walks toward
// that marker.
// ###############################################################
public class WhalerController : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	private int currentIdx = 0;
	private float timePerAnimation = 0.3f;
	private float timeOnAnimation = 0;
	private List<string> sprites = new List<string> { "eskimo_left", "eskimo", "eskimo_right", "eskimo" };
	private AudioSource sound;
	private bool playing = false;

// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		sound = gameObject.AddComponent<AudioSource> ();
		AudioClip soundClip = (AudioClip)Resources.Load("Sounds/snow-walk", typeof(AudioClip));
		sound.clip = soundClip;
		sound.loop = true;
	}

	// Called once per frame
	void Update () {
		if (GameObject.Find ("Placed Marker Area") != null && (GameController.inPlay || GameController.won)) {
			if (!playing) {
				if (GameController.sfxStatus) sound.Play();
				playing = true;
			}
			moveWhaler ();
		} else {
			sound.Stop();
			playing = false;
		}
	}

// ###############################################################
// WhalerController Functions 
// ###############################################################
	private void changePosition (Vector3 target) {
		// if we have not reached the target marker
		if (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, 5f * Time.deltaTime);
		// if we have reached the target marker
		} else {
			// destroy the marker, and stop the whaler
			Destroy (GameObject.Find ("Placed Marker Area"));
			Destroy (GameObject.Find ("Placed Marker"));
			GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/eskimo");
			currentIdx = 0;
			timeOnAnimation = 0;
		}
	}

	private void changeRotation (Vector3 target) {
		// rotate the whaler to look towards the placed marker
		if (target - transform.position != Vector3.zero) {
			var newRotation = Quaternion.LookRotation (target - transform.position).eulerAngles;
			newRotation.x = Mathf.Clamp (-90, -90, -90);
			newRotation.z = 0;
			transform.rotation = Quaternion.Slerp (
				transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 2
			);
		}
	}

	private void moveWhaler () {
		// as the time moves on, change the image used for the walking whaler
		timeOnAnimation += Time.deltaTime;
		if (timeOnAnimation > timePerAnimation) {
			timeOnAnimation = 0;
			currentIdx++;
			if (currentIdx == 4) {
				currentIdx = 0;
			}
		}
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + sprites[currentIdx]);
		Vector3 target = GameObject.Find ("Placed Marker Area").transform.position;
		changeRotation (target);
		changePosition (target);
	}
}
