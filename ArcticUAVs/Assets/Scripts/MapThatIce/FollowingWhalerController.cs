using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// FollowingWhalerController is responsible for the movement and 
// animations of the whalers who are not in front of the group
// in the MapThatIce mission.
// ###############################################################
public class FollowingWhalerController : MonoBehaviour {

// ###############################################################
// Class Variables 
// ###############################################################
	public string toFollow;
	public bool validMarker = true;
	private int currentIdx = 0;
	private Vector3 currentTarget;
	private bool okayToExtend = true;
	private List<string> sprites = new List<string> { "eskimo", "eskimo_left", "eskimo", "eskimo_right" };
	private float timeOnAnimation = 0;
	private float timePerAnimation = 0.3f;
	private AudioSource sound;
	private bool playing = false;

// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		sound = gameObject.AddComponent<AudioSource> ();
		AudioClip soundClip = (AudioClip)Resources.Load("Sounds/snow-walk", typeof(AudioClip));
		sound.clip = soundClip;
		sound.volume = 0.15f;
		sound.loop = true;
	}	
	// Called once per frame
	void Update () {
		// move toward the marker while the game is inplay
		// or if the game is won, still move toward the marker
		if (GameController.inPlay || GameController.won) {
			if (okayToFollow ()) { 
				setUpToFollow (); 
			} else if (transform.position != currentTarget && okayToExtend) {
				setUpToExtend (); 
			}
			moveFollower ();
		} else {
			sound.Stop();
			playing = false;
		}
	}

// ###############################################################
// Following Whaler Functions 
// ###############################################################
	// double check that it is ok to follow the target
	bool okayToFollow () {
		Vector3 whalerPosition = GameObject.Find (toFollow).transform.position;
		LayerMask mask = -1;
		return !(Physics.Linecast (whalerPosition, transform.position, mask.value, QueryTriggerInteraction.Ignore));
	}
	// find the position of the whaler in front of them
	private void setUpToFollow () {
		validMarker = true;
		currentTarget = GameObject.Find (toFollow).transform.position;
		okayToExtend = true;
	}
	// Extend the target to see get the follower around corners
	private void setUpToExtend () {
		validMarker = false;
		Vector3 heading = currentTarget - transform.position;
		Vector3 direction = heading / heading.magnitude;
		currentTarget = currentTarget + (direction * 5);
		okayToExtend = false;
	}
	// handle the movement mechanics
	private void moveFollower () {
		changeSprite ();
		changeRotation ();
		changePosition ();
	}
	// change the sprite to animate walking
	private void changeSprite () {
		timeOnAnimation += Time.deltaTime;
		if (timeOnAnimation > timePerAnimation) {
			timeOnAnimation = 0;
			currentIdx++;
			if (currentIdx == 4) { 
				currentIdx = 0; 
			}
		}
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/" + sprites[currentIdx]);
	}
	// change the follower's rotation towards the target
	private void changeRotation () {
		if (currentTarget - transform.position != Vector3.zero) {
			var newRotation = Quaternion.LookRotation (currentTarget - transform.position).eulerAngles;
			newRotation.x = Mathf.Clamp (-90, -90, -90);
			newRotation.z = 0;
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (newRotation), Time.deltaTime * 2);
		}
	}
	// move the follower towards the target
	private void changePosition () {
		if (notArrived ()) { 
			transform.position = Vector3.MoveTowards (transform.position, currentTarget, 5f * Time.deltaTime); 
		} else {
			GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/eskimo");
			currentIdx = 0;
			timeOnAnimation = 0;
		}
	}
	// check that we have not arrived at the target
	private bool notArrived () {
		if (currentTarget == GameObject.Find (toFollow).transform.position) {
			return Vector3.Distance (currentTarget, transform.position) > 4;
		}
		return transform.position != currentTarget;
	}
}
