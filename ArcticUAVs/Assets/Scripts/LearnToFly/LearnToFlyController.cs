using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	This is the LearnToFlyController class.  This class is in charge of setting up and stepping the player through the
	LearnToFly scene.  This includes formatting goal lines, showing and hiding instructional panels and offering 
	encouragement with a lumberjack toting positive statements.
*/
public class LearnToFlyController : MonoBehaviour {

/*
	Class Variables 
*/
	public bool completedInstructional = false;
	public int instructional;
	public bool instructionsComplete = false;
	public bool startedRight = false;
	private bool animationsStarted = false;
	private bool encouragementManMoving = false;
	private List<string> encouragements = new List<string> { 
		"Nice", "Well done", "You rock", "Alright", "Great job", "Super", "Awesome", "Wow" 
	};
	private GameObject instructionPopup1;
	private GameObject instructionPopup2;
	private GameObject instructionPopup3;
	private GameObject instructionPopup4;
	private GameObject instructionPopup5;
	private GameObject instructionPopup6;
	private Vector3 screenDimensions;
	private float timeKinematic = 0;

/*
	Class Functions 
*/
	// Initialization (called once when gameObject is created).
	void Start () {
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		hideNecessaryObjects ();
		instructional = 1;
	}
	// Called once per frame
	void Update () {
		handleFirstFrame ();
		handleUAVFreezing ();
		handleInstructionalStatus ();
	}

/*
	Content and Helper Functions 
*/
	private IEnumerator animateThumbTap (GameObject thumb) {
		if (thumb != null) {
			if (thumb.transform.localScale.x > 0.4f) {
				thumb.transform.localScale = new Vector3 (
					thumb.transform.localScale.x - 0.2f * Time.fixedDeltaTime, 
					thumb.transform.localScale.y - 0.2f * Time.fixedDeltaTime
				);
				yield return new WaitForEndOfFrame ();
				StartCoroutine (animateThumbTap (thumb));
			} else {
				StartCoroutine (animateThumbLift (thumb));
				StartCoroutine (animateTapRipple (thumb));
			}
		}
	}

	private IEnumerator animateThumbLift (GameObject thumb) {
		if (thumb != null) {
			if (thumb.transform.localScale.x < 0.5f) {
				thumb.transform.localScale = new Vector3 (
					thumb.transform.localScale.x + 0.2f * Time.fixedDeltaTime, 
					thumb.transform.localScale.y + 0.2f * Time.fixedDeltaTime
				);
				yield return new WaitForEndOfFrame ();
				StartCoroutine (animateThumbLift (thumb));
			} else {
				StartCoroutine (animateThumbTap (thumb));
			}
		}
	}

	private IEnumerator animateTapRipple (GameObject thumb) {
		GameObject ripple1 = setupRipple (thumb, new GameObject (thumb.name + " Ripple 1"), 0.055f, 0.094f);
		yield return new WaitForSeconds (0.1f);
		GameObject ripple2 = setupRipple (thumb, new GameObject (thumb.name + " Ripple 2"), 0.104f, 0.176f);
		yield return new WaitForSeconds (0.1f);
		StartCoroutine (fadeOutRipple (ripple1));
		yield return new WaitForSeconds (0.1f);
		StartCoroutine (fadeOutRipple (ripple2));
	}

	private Vector3 determineManPosition (int canvasWidth, int canvasHeight) {
		if (Random.value > 0.5f) { // Left side
			if (!encouragementManMoving) {
				encouragementManMoving = true;
				StartCoroutine (leanLeft ("Left"));
			}
			return new Vector3 (
				-0.098f * canvasWidth, Random.Range (0.156f * canvasHeight, canvasHeight - (0.391f * canvasHeight))
			);
		} else { // Right side
			if (!encouragementManMoving) {
				encouragementManMoving = true;
				StartCoroutine (leanRight ("Right"));
			}
			return new Vector3 (
				canvasWidth + (0.098f * canvasWidth), 
				Random.Range (0.156f * canvasHeight, canvasHeight - (0.391f * canvasHeight))
			);
		}
	}

	private void encourage () {
		GameObject.Find ("Encouragement").GetComponent<Text> ().text = 
			encouragements [Random.Range (0, encouragements.Count)] + "!";
		int canvasWidth = (int) GameObject.Find ("Top Right Corner").GetComponent<RectTransform> ().position.x;
		int canvasHeight = (int) GameObject.Find ("Top Right Corner").GetComponent<RectTransform> ().position.y;
		Vector3 position = determineManPosition (canvasWidth, canvasHeight);
		GameObject.Find ("Encouragement Man").transform.position = position;
	}

	private IEnumerator fadeOutRipple (GameObject ripple) {
		for (float i = 1; i >= 0; i -= 0.1f) {
			if (ripple != null) {
				ripple.GetComponent<SpriteRenderer> ().color = new Vector4 (1f, 1f, 1f, i);
				yield return new WaitForSeconds (0.005f);
			} else {
				yield break;
			}
		}
		Destroy (ripple);
	}

	private void handleContinue () {
		switch (instructional - 1) { // The instructional just completed
		case 1:
			Destroy (GameObject.Find ("Instruction Popup 1"));
			break;
		case 2:
			if (!startedRight) Destroy (GameObject.Find ("Instruction Popup 2"));
			else Destroy (GameObject.Find ("Instruction Popup 3"));
			break;
		case 3:
			if (!startedRight) Destroy (GameObject.Find ("Instruction Popup 3"));
			else Destroy (GameObject.Find ("Instruction Popup 2"));
			break;
		case 4:
			Destroy (GameObject.Find ("Instruction Popup 4"));
			break;
        case 6:
			Destroy (GameObject.Find ("Instruction Popup 5"));
            break;
		case 7:
			Destroy (GameObject.Find ("Instruction Popup 6"));
			break;
		default:
			break;
		}
	}

	private void handleFirstFrame () {
		if (GameController.inPlay && !animationsStarted) {
			animationsStarted = true;
			setupInstructional ();
		}
	}

	private void handleInstructionalStatus () {
		if (!instructionsComplete) {
			if (completedInstructional) {
				completedInstructional = false;
				encourage ();
				StartCoroutine (transitionToNextInstructional ());
			}
		} else {
			if (completedInstructional) {
				completedInstructional = false;
				encourage ();
				setupInstructional ();
			}
		}
	}

	private void handleUAVFreezing () {
		if (GameObject.Find ("uav").GetComponent<Rigidbody2D> ().isKinematic) {
			timeKinematic += Time.deltaTime;
		} else {
			timeKinematic = 0;
		}
		if (timeKinematic > 0.5 && Input.GetMouseButton (0)) {
			GameObject.Find ("uav").GetComponent<Rigidbody2D> ().isKinematic = false;
			handleContinue ();
		}
	}

	private void hideNecessaryObjects () {
		instructionPopup1 = GameObject.Find ("Instruction Popup 1");
		instructionPopup2 = GameObject.Find ("Instruction Popup 2");
		instructionPopup3 = GameObject.Find ("Instruction Popup 3");
		instructionPopup4 = GameObject.Find ("Instruction Popup 4");
		instructionPopup5 = GameObject.Find ("Instruction Popup 5");
		instructionPopup6 = GameObject.Find ("Instruction Popup 6");
		instructionPopup1.SetActive (false);
		instructionPopup2.SetActive (false);
		instructionPopup3.SetActive (false);
		instructionPopup4.SetActive (false);
		instructionPopup5.SetActive (false);
		instructionPopup6.SetActive (false);
	}

	private IEnumerator leanLeft (string side) {
		for (int i = 0; i < 65; i++) {
			GameObject.Find ("Encouragement Man").GetComponent<RectTransform> ().Rotate (0, 0, -1);
			yield return new WaitForEndOfFrame ();
		}
		if (side == "Left") {
			yield return new WaitForSeconds (1);
			StartCoroutine (leanRight (side));
		} else {
			encouragementManMoving = false;
		}
	}

	private IEnumerator leanRight (string side) {
		for (int i = 0; i < 65; i++) {
			GameObject.Find ("Encouragement Man").GetComponent<RectTransform> ().Rotate (0, 0, 1);
			yield return new WaitForEndOfFrame ();
		}
		if (side == "Right") {
			yield return new WaitForSeconds (1);
			StartCoroutine (leanLeft (side));
		} else {
			encouragementManMoving = false;
		}
	}

	private void makeFirstGoal () {
		for (float i = -screenDimensions.x; i <= screenDimensions.x + 2.1f; i += 2.1f) {
			GameObject segment = new GameObject ("Line Segment");
			segment.transform.parent = GameObject.Find ("Goal Line").transform;
			setupSegmentRenderer (segment.AddComponent<SpriteRenderer> ());
			segment.transform.Rotate (0, 0, 90);
			segment.transform.localScale = new Vector3 (0.3f, 0.3f, 1);
			segment.transform.position = new Vector3 (i, -0.141f * screenDimensions.y, 0);
			setupSegmentCollider (segment, 1);
			segment.AddComponent<GoalLineController> ();
		}
	}

	private void makeGoalLine (string side) {
		for (float i = -screenDimensions.y; i <= screenDimensions.y + (0.516f * screenDimensions.y); 
			i += (0.516f * screenDimensions.y)) {
			GameObject segment = new GameObject ("Line Segment");
			segment.transform.parent = GameObject.Find ("Goal Line").transform;
			setupSegmentRenderer (segment.AddComponent<SpriteRenderer> ());
			segment.transform.localScale = new Vector3 (0.3f, 0.3f, 1);
			segment.transform.position = new Vector3 (((side == "Left" ? -1 : 1) * screenDimensions.x) / 3, i, 0);
			setupSegmentCollider (segment, side == "Right" ? 1 : -1);
			segment.AddComponent<GoalLineController> ();
		}
	}

	private GameObject makeThumb (string side) {
		GameObject thumb = new GameObject (side + " Thumb");
		SpriteRenderer renderer = thumb.AddComponent<SpriteRenderer> ();
		renderer.sprite = Resources.Load<Sprite> ("Sprites/thumb");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 1;
		thumb.transform.Rotate (new Vector3 (0, 0, side == "Left" ? -45 : 45));
		thumb.transform.localScale = new Vector3 (0.5f, 0.5f, 1);
		Vector3 thumbPosition = Camera.main.ScreenToWorldPoint (
			new Vector3 (Screen.width / 6 * (side == "Left" ? 1 : 5), Screen.height / 4)
		);
		thumbPosition.z = 0;
		thumb.transform.position = thumbPosition;
		return thumb;
	}

    private GameObject makeCircle() {
        GameObject dottedCircle = new GameObject ("Dotted Circle");
        SpriteRenderer renderer = dottedCircle.AddComponent<SpriteRenderer> ();
        renderer.sprite = Resources.Load<Sprite> ("Sprites/DottedCircle");
        renderer.sortingLayerName = "Foreground";
        renderer.sortingOrder = 1;
        dottedCircle.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
		dottedCircle.transform.position = setCirclePosition ();
        dottedCircle.AddComponent<CircleCollider2D> ();
        dottedCircle.GetComponent<CircleCollider2D> ().isTrigger = true;
		dottedCircle.GetComponent<CircleCollider2D> ().radius = 0.035f * screenDimensions.x;
        dottedCircle.AddComponent<GoalLineController> ();
        return dottedCircle;
    }

	private Vector3 setCirclePosition () {
		Vector3 position;
		do {
			position = new Vector3 (
				Random.Range (-screenDimensions.x + (0.138f * screenDimensions.x), 
					screenDimensions.x - (0.138f * screenDimensions.x)
				),
				Random.Range (-screenDimensions.y + (0.234f * screenDimensions.y), 
					screenDimensions.y - (0.234f * screenDimensions.y)
				)
			);
		} while (Vector3.Distance (GameObject.Find ("uav").transform.position, position) < 0.276f * screenDimensions.x);
		return position;
	}

	private void setColliderOffset (GameObject segment, int multiplier) {
		if (Application.platform == RuntimePlatform.OSXEditor) {
			segment.GetComponent<PolygonCollider2D> ().offset = new Vector2 (multiplier * 2f, 0f);
		} else {
			segment.GetComponent<PolygonCollider2D> ().offset = new Vector2 (multiplier * 5f, 0f);
		}
	}

	private void setupFirstInstructional () {
		// GameObject.Find ("uav").GetComponent<Rigidbody2D> ().isKinematic = true; // Start the UAV as kinematic
		instructionPopup1.SetActive (true);
		GameObject leftThumb = makeThumb("Left");
		GameObject rightThumb = makeThumb("Right");
		StartCoroutine (animateThumbTap (leftThumb));
		StartCoroutine (animateThumbTap (rightThumb));
		new GameObject ("Goal Line");
		makeFirstGoal ();
	}

	private void setupInstructional () {
		switch (instructional) {
		case 1:
			setupFirstInstructional ();
			break;
		case 2:
			setupSecondInstructional ();
			break;
		case 3:
			setupThirdInstructional ();
			break;
		case 4:
			instructionPopup4.SetActive (true);
			makeCircle();
			break;
		case 5:
			makeCircle();
			break;
		case 6:
			GameObject.Find ("uav").GetComponent<ScoutController> ().clampToSides = false;
			instructionPopup5.SetActive (true);
			makeCircle();
			break;
		case 7:
			instructionPopup6.SetActive (true);
			makeCircle();
			break;
		default:
			instructionsComplete = true;
			makeCircle();
			break;
		}
		instructional++;
	}

	private GameObject setupRipple (GameObject thumb, GameObject ripple, float scaleX, float scaleY) {
		SpriteRenderer renderer = ripple.AddComponent<SpriteRenderer> ();
		renderer.sprite = Resources.Load<Sprite> (
			"Sprites/thumbRipple" + ((ripple.name == thumb.name + " Ripple 1") ? "1" : "2")
		);
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 1;
		ripple.transform.localScale = new Vector3 (0.4f, 0.4f, 1);
		if (thumb.name == "Left Thumb") {
			ripple.transform.position = new Vector3 (thumb.transform.position.x + (scaleX * screenDimensions.x), 
				thumb.transform.position.y + (scaleY * screenDimensions.y), 0);
			ripple.transform.Rotate (0, 0, -45);
		} else {
			ripple.transform.position = new Vector3 (thumb.transform.position.x - (scaleX * screenDimensions.x), 
				thumb.transform.position.y + (scaleY * screenDimensions.y), 0);
			ripple.transform.Rotate (0, 0, 45);
		}
		return ripple;
	}

	private void setupSecondInstructional () {
		new GameObject ("Goal Line");
		if (!startedRight) {
			instructionPopup2.SetActive (true);
			makeGoalLine ("Right");
		} else {
			instructionPopup3.SetActive (true);
			makeGoalLine ("Left");
		}
	}

	private void setupSegmentCollider (GameObject segment, int multiplier) {
		segment.AddComponent<PolygonCollider2D> ();
		segment.GetComponent<PolygonCollider2D> ().isTrigger = true;
		setColliderOffset (segment, multiplier);
	}

	private void setupSegmentRenderer (SpriteRenderer renderer) {
		renderer.sprite = Resources.Load<Sprite> ("Sprites/dottedLine");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 1;
	}

	private void setupThirdInstructional () {
		new GameObject ("Goal Line");
		if (!startedRight) {
			instructionPopup3.SetActive (true);
			StartCoroutine (animateThumbTap (makeThumb("Left")));
			makeGoalLine ("Left");
		} else {
			instructionPopup2.SetActive (true);
			StartCoroutine (animateThumbTap (makeThumb("Right")));
			makeGoalLine ("Right");
		}
	}

	private IEnumerator transitionToNextInstructional () {
		yield return new WaitForSeconds (0.2f);
		setupInstructional ();
	}
}
