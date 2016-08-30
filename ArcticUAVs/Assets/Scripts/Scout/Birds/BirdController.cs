using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ###############################################################
// Bird Controller moves the bird across the screen from it's spawn side to the opposite side
// It also is responsible for near misses, crashes, and random bird sounds
// ###############################################################
public class BirdController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private bool collided = false;
	private bool newNearMiss = false;
	private string side = "";
	private AudioSource birdSound;
	private AudioSource birdScream;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// setting up crash sound
		AudioClip gullSound = Resources.Load<AudioClip>("Sounds/gull-scream");
		birdScream = gameObject.AddComponent<AudioSource> ();
		birdScream.clip = gullSound;
		// setting up random gull sound
		gullSound = Resources.Load<AudioClip>("Sounds/gull1");
		birdSound = gameObject.AddComponent<AudioSource> ();
		birdSound.clip = gullSound;
		birdSound.pitch = Random.Range(.8f,1.3f);
		// play the random bird sound at a random interval
		Invoke("playBirdSound",Random.Range (1f,7f));

		if (GetComponent<SpriteRenderer> ().sprite.name == "bird mid wing left") {
			side = "left";
		} else {
			side = "right";
		}
		StartCoroutine (moveBird ());
	}
	// ***************************************************************
	// Near Misses
	void OnTriggerEnter2D (Collider2D collider) {
		GameController.birdsMissed++;
	}
	// ***************************************************************
	// crashes
	void OnCollisionEnter2D (Collision2D collision) {
		if (!collided) {
			collided = true;
			GameController.lost = true;
			GameController.inPlay = false;
			GameController.birdsHit++;
			if (GameController.sfxStatus) birdScream.Play();
			handleBirdReaction (collision);
		}
	}
// ###############################################################
// Movement Functions
// ###############################################################
	private Vector3 determineTarget () {
		if (side == "right") { 
			return new Vector3 (GameObject.Find ("LeftSpawnEdge").transform.position.x - 3f, transform.position.y); 
		}
		return new Vector3 (GameObject.Find ("RightSpawnEdge").transform.position.x + 3f, transform.position.y);
	}
	// ***************************************************************
	private IEnumerator moveBird () {
		Vector3 target = determineTarget ();
		bool nearTarget = isNearTarget (target);
		
		if (nearTarget) { 
			Destroy (gameObject); 
		}
		else if (!collided) {
			if (GameController.inPlay || GameController.lost) {
				transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * 3f);
			}
			yield return new WaitForEndOfFrame ();
			StartCoroutine (moveBird ());
		}
	}
	// ***************************************************************
	private bool isNearTarget (Vector3 target) {
		if (side == "right") { 
			return transform.position.x <= (target.x + .01f); 
		}
		return transform.position.x >= (target.x - .01f);
	}
// ###############################################################
// Sound Function
// ###############################################################
	private void playBirdSound () {
		if (GameController.sfxStatus) birdSound.Play();
	}
// ###############################################################
// Animation Functions (For collisions)
// ###############################################################
	private void handleBirdReaction (Collision2D collision) {
		gameObject.AddComponent<Rigidbody2D> ();
		if (collision.collider.gameObject.name == "uav") {
			collision.collider.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (-getForceBasedOnSide (), 0));
			GetComponent<Rigidbody2D> ().AddForce (new Vector2 (getForceBasedOnSide (), 0));
			GetComponent<Rigidbody2D> ().AddTorque (20);
		}
		StartCoroutine (flailBird ());
	}
	// ***************************************************************
	private float getForceBasedOnSide () {
		if (side == "left") return -50;
		return 50;
	}
	// ***************************************************************
	private IEnumerator flailBird () {
		if (GetComponent<SpriteRenderer> ().sprite.name == "bird mid wing " + side || 
			GetComponent<SpriteRenderer> ().sprite.name == "bird high wing " + side) {
			GetComponent<SpriteRenderer> ().sprite = Resources.Load (
				"Sprites/bird low wing " + side, typeof(Sprite)
			) as Sprite;
		} else {
			GetComponent<SpriteRenderer> ().sprite = Resources.Load (
				"Sprites/bird high wing " + side, typeof(Sprite)
			) as Sprite;
		}
		yield return new WaitForSeconds (0.1f);
		StartCoroutine (flailBird ());
	}
}