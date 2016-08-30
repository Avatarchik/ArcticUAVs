using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// ################################################################
// SplashController is responsible for creating splashes
// when objects come into contact with the "Middle Wave" in
// the scene.
// ###############################################################
public class SplashController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private AudioSource sound;

// ###############################################################
// Unity Functions 
// ###############################################################
	// Called when the collider is entered by another
	void OnTriggerEnter2D(Collider2D other) {
		GameController.message = ScoutController.drowned;
		GameController.lost = true;
		GameController.inPlay = false;
		GameObject leftSplash = makeSplash ("splash_left", -1, other);
		GameObject rightSplash = makeSplash ("splash_right", 1, other);
		StartCoroutine (destroySplash (leftSplash, rightSplash, other.gameObject));
	}

// ###############################################################
// Splash Functions 
// ###############################################################
	private IEnumerator destroySplash (GameObject left, GameObject right, GameObject splasher) {
		yield return new WaitForSeconds (2);
		Destroy (left);
		Destroy (right);
		splasher.GetComponent<Rigidbody2D> ().isKinematic = true;
	}

	private GameObject makeSplash (string spriteName, int multiplier, Collider2D other) {
		GameObject splashObj = new GameObject ();
		SpriteRenderer renderer = splashObj.AddComponent<SpriteRenderer> ();
		renderer.sprite = Resources.Load ("Sprites/" + spriteName, typeof(Sprite)) as Sprite;
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 4;
		splashObj.transform.position = new Vector3 (
			other.transform.position.x + (multiplier * 0.3f), other.transform.position.y - 0.5f
		);
		splashObj.AddComponent<Rigidbody2D> ();
		splashObj.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (multiplier * 40, 280));
		splashObj.GetComponent<Rigidbody2D> ().AddTorque (-multiplier * 100);
		if (spriteName == "splash_right") {
			splashObj.AddComponent<SplashSound> ();
			AudioClip pointSound = (AudioClip)Resources.Load("Sounds/splash", typeof(AudioClip));
			AudioSource sound = splashObj.AddComponent<AudioSource> ();
			sound.clip = pointSound;
		}
		return splashObj;
	}
}
