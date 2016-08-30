using UnityEngine;
using System.Collections;
// ###############################################################
// ManController handles when the UAV hits the man in GTP
// ###############################################################
public class ManController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private AudioSource sound;
	private bool collided = false;
// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		AudioClip pointSound = Resources.Load<AudioClip>("Sounds/man-yell-eeeh");
		sound = gameObject.AddComponent<AudioSource> ();
		sound.clip = pointSound;
	}
	void OnCollisionEnter2D (Collision2D collision) {
		if (!collided) {
			collided = true;
			handleFirstCollision ();
		}
		handlePhysicalReaction (collision);
	}
// ###############################################################
// Animation functions
// ###############################################################
	private void handleFirstCollision () {
		// YELLLLllllLLLLLllLLLLlllLLLLLLL
		if (GameController.sfxStatus) sound.Play();
		// Add a rigidbody to the man so he falls "naturally"
		if (GetComponent<Rigidbody2D> () == null) {
			gameObject.AddComponent<Rigidbody2D> ();
		}
		// Switch the man to hands up man
		GetComponent<SpriteRenderer> ().sortingLayerName = "Foreground";
		GetComponent<SpriteRenderer> ().sortingOrder = 4;
		GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/man_handsup");
		// Add a collider so that the man makes a splash when he hits the waves
		gameObject.AddComponent<BoxCollider2D> ();
	}
	private void handlePhysicalReaction (Collision2D collision) {
		int horizontalForce = collision.collider.transform.position.x > transform.position.x ? -80 : 80;
		int rotation = collision.collider.transform.position.x > transform.position.x ? 30 : -30;
		Vector2 catcherImpulse = new Vector2 (horizontalForce, 80);
		Vector2 uavImpulse = new Vector2 (-horizontalForce, 80);

		GetComponent<Rigidbody2D> ().AddForce (catcherImpulse);
		if (collision.collider.gameObject.name == "uav") {
			GameObject.Find ("uav").GetComponent<Rigidbody2D> ().AddForce (uavImpulse);
		}
		transform.Rotate (0, 0, rotation);
	}

}