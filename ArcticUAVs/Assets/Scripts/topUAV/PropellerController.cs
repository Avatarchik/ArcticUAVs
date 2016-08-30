using System.Collections;
using UnityEngine;

// ###############################################################
// PropellerController is responsible for the rotation and blur
// of the MTI UAV.
// ###############################################################
public class PropellerController : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	private float blurAlphaStep;
	private float delay = 0f;
	private float maxDelay = 2f;
	private float propAlphaStep;
	public bool isSpinning = false;
	public float spinSpeed = 0f;

// ###############################################################
// Unity Functions 
// ###############################################################
	// Initialization (called once when gameObject is created).
	void Start () {
		// determin the steps to take for the alpha of the prop and blur
		propAlphaStep = 0.67f / (maxDelay / Time.fixedDeltaTime);
		blurAlphaStep = 0.33f / (maxDelay / Time.fixedDeltaTime);
	}
	// Called once per physics time step
	void FixedUpdate () {
		// if the UAV is actually touching the ground
		if (GameController.onGround) {
			// if we have not slowed the props down yet
			if (delay > 0) {
				// start slowing down
				delay = delay - Time.fixedDeltaTime;
				// change the blur to reflect the change in speed
				changeAlphaValues (
					new Vector4 ( 1f, 1f, 1f, transform.GetChild (0).GetComponent<SpriteRenderer> ().color.a - blurAlphaStep),
					new Vector4 ( 1f, 1f, 1f, GetComponent<SpriteRenderer> ().color.a + propAlphaStep)
				);
			// if the props are stopped
			} else {
				// set their alpha values
				delay = 0;
				changeAlphaValues (new Vector4 (1f, 1f, 1f, 0f), new Vector4 (1f, 1f, 1f, 1f));
			}
		// if the uav is not touching the ground anymore
		} else {
			// if the props have not sped up yet
			if (delay < maxDelay) {
				// start speeding up the props
				delay = delay + Time.fixedDeltaTime;
				// change the blur to reflect the change in speed
				changeAlphaValues (
					new Vector4 (
						1f, 1f, 1f, transform.GetChild (0).GetComponent<SpriteRenderer> ().color.a + blurAlphaStep
					),
					new Vector4 (1f, 1f, 1f, GetComponent<SpriteRenderer> ().color.a - propAlphaStep)
				);
			// if the props have fully sped up
			} else {
				// set their alpha values
				delay = maxDelay;
				changeAlphaValues (new Vector4 (1f, 1f, 1f, 0.33f), new Vector4 (1f, 1f, 1f, .33f));
			}
		}
		// rotate the propellers
		rotateProps ();
	}

// ###############################################################
// PropellerController Functions 
// ###############################################################
	private void changeAlphaValues (Vector4 blurColor, Vector4 propColor) {
		transform.GetChild (0).GetComponent<SpriteRenderer> ().color = blurColor;
		GetComponent<SpriteRenderer> ().color = propColor;
	}

	private void rotateProps () {
		// if the props should be spinning
		if (delay != 0) {
			// start spinning the props
			isSpinning = true;
			spinSpeed = delay * 0.5f;
			transform.Rotate (Vector3.forward, 34 * delay);
		} else {
			isSpinning = false;
		}
	}
}
