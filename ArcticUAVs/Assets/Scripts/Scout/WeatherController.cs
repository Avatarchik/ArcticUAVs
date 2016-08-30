using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ###############################################################
// WeatherController is responsible for all weather related
// mechanics for the scenes it is a part of.
// The chanceOfStrike, percipitation, and windIntesity are all
// determined by 1: the Difficulty, and 2: the Random function
// ###############################################################
public class WeatherController : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	private bool struck = false;
	private int chanceOfStrike = 0;
	private Dictionary<string, int> chancesOfStrike = new Dictionary<string, int> () { 
		{ "Tutorial", 0 }, { "Easy", 0 }, { "Normal", 100 }, { "Hard", 50 }, { "Insane", 10 } 
	};
	private SpriteRenderer percipRenderer;
	private int percipitation;
	private Dictionary<string, int> percipitationValues = new Dictionary<string, int> () { 
		{ "Tutorial", 0 }, { "Easy", 0 }, { "Normal", 1 }, { "Hard", 2 }, { "Insane", 3 } 
	};
	private int windIntensity;
	private float windForce;
	private Dictionary<string, int> windIntensities = new Dictionary<string, int> () { 
		{ "Tutorial", 0 }, { "Easy", 10 }, { "Normal", 25 }, { "Hard", 40 }, { "Insane", 100 } 
	};
	private SpriteRenderer swirlRenderer;
	private int cloudCover = 70;

	private GameObject rightLimit;
	private GameObject leftLimit;
	private Vector3 screenDimensions;

	private AudioSource rainSound;
	private AudioSource lightningSound;

// ###############################################################
// Unity Functions 
// ###############################################################

	// Initialization (called once when gameObject is created).
	void Start () {
		string difficulty = GameController.missionDiff;

		// Calculate wind force
		windForce = Random.Range (-windIntensities [difficulty], windIntensities [difficulty]) / 64f;
		// determine how much rain
		percipitation = Random.Range (0, percipitationValues [difficulty]);
		// color screen grey based on how much rain there is
		GameObject.Find ("Rain Panel").GetComponent<Image> ().color = new Color(0f, 0f, 0f, percipitation * .15f);
		// load chance of strikes
		chanceOfStrike = chancesOfStrike [difficulty];
		// set screen dimensions
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		// set screen edges
		leftLimit = GameObject.Find ("LeftSpawnEdge");
		rightLimit = GameObject.Find ("RightSpawnEdge");
		// Display wind amount and direction
		displayWindStats ();
		// make clouds
		makeClouds ();
		// Make Wind Swirls
		windIntensity = Mathf.CeilToInt(Mathf.Abs (windForce) * 10);
		if (windIntensity != 0) {
			StartCoroutine (makeSwirl ());
		}
		// setup Rain Sound
		AudioClip pointSound = (AudioClip)Resources.Load("Sounds/rain", typeof(AudioClip));
		rainSound = gameObject.AddComponent<AudioSource> ();
		rainSound.clip = pointSound;
		rainSound.volume = .2f;
		rainSound.loop = true;
		if (percipitation > 0) {
			if (GameController.sfxStatus) rainSound.Play();}
	}

	// Update: Called once per frame
	void Update () {
			// make the rain fall in the scene
		makeRain ();
			// Apply wind force to objects in the scene
		handleWindForces ();
			// Check if the UAV gets struck by a random bolt of lightning
		handleLightning ();
	}

// ###############################################################
// Wind and Cloud Functions
// ###############################################################
	private void makeClouds () {
		for (int clouds = 0; clouds < cloudCover; clouds++) {
			GameObject cloud = new GameObject (name: "cloud");
			cloud.transform.parent = GameObject.Find ("Clouds").transform;
				// Sprite Rendering
			SpriteRenderer cloudRenderer = cloud.AddComponent<SpriteRenderer> ();
			cloudRenderer.sortingLayerName = "Foreground";
			cloudRenderer.sortingOrder = 7;
				// Determin Rain clouds or normal clouds
			if (percipitation == 0) {
				cloudRenderer.sprite = Resources.Load<Sprite>("Sprites/Clouds/cloud" + Random.Range (1, 6));
			} else {
				cloudRenderer.sprite = Resources.Load<Sprite>("Sprites/Rain Clouds/rainCloud" + Random.Range (1, 6));
			}
				// set cloud position
			cloud.transform.position = new Vector2 (
				Random.Range (leftLimit.transform.position.x, rightLimit.transform.position.x + 1), 
				Random.Range (screenDimensions.y - 0.25f, screenDimensions.y)
			);
				// set cloud scale
			cloud.transform.localScale = new Vector3 (0.3f, 0.3f, cloud.transform.localScale.z);
		}
	}

	private void handleWindForces () {
		// add wind force to all the rain drops in the scene
		foreach (Transform drop in GameObject.Find ("Rain").transform) {
			drop.GetComponent<Rigidbody2D> ().AddForce (new Vector2 ((windForce * 7), 0));
		}
		// add force to the UAV
		GameObject.Find ("uav").GetComponent<Rigidbody2D> ().AddForce (new Vector2 ((windForce), 0));
	}

	private void displayWindStats () {
		GameObject.Find ("Wind Speed").GetComponent<Text> ().text = ((int)(Mathf.Abs(windForce) * 30)).ToString();
		if (windForce != 0) {
			Image windDirection = GameObject.Find ("Wind Direction").GetComponent<Image> ();
			windDirection.preserveAspect = true;
			if (windForce < 0) {
				windDirection.sprite = Resources.Load<Sprite>("Sprites/leftArrow");
			} else {
				windDirection.sprite = Resources.Load<Sprite>("Sprites/rightArrow");
			}
		}
	}

// ###############################################################
// Rain Functions 
// ###############################################################
	private void makeRain () {
		// Create as many specified by the rain intensity
		for (int particles = 0; particles < percipitation; particles++) {
			GameObject particle = new GameObject (name: "particle");
			percipRenderer = particle.AddComponent<SpriteRenderer> ();
			// Sprite Rendering
			percipRenderer.sprite = Resources.Load<Sprite> ("Sprites/Raindrop");
			// The size of the drop is scaled to the screen size
			particle.transform.localScale = new Vector2 (
				((rightLimit.transform.position.x - leftLimit.transform.position.x) / 2) / 1000, 
				((rightLimit.transform.position.x - leftLimit.transform.position.x) / 2) / 200
			); 
			particle.transform.parent = GameObject.Find ("Rain").transform;
			percipRenderer.sortingLayerName = "Foreground";
			percipRenderer.sortingOrder = 6;
			// Set position and rotation of the raindrop
			particle.AddComponent<Rigidbody2D> ();
			if (windForce > 0) {
				particle.transform.position = new Vector2 (
					Random.Range (leftLimit.transform.position.x - 10f, rightLimit.transform.position.x), 
					screenDimensions.y + 1
				);
			} else {
				particle.transform.position = new Vector2 (
					Random.Range (leftLimit.transform.position.x, rightLimit.transform.position.x + 10f),
					screenDimensions.y + 1
				);
			}
			particle.transform.Rotate (0, 0, windForce * 45);
			// Destroy the drop when it's done falling
			StartCoroutine (destroyParticle (particle)); 
		}
	}

	private IEnumerator destroyParticle (GameObject particle) {
		yield return new WaitForSeconds (1.55f);
		Destroy (particle);
	}

// ###############################################################
// Lightning Functions 
// ###############################################################
	private void handleStrike () {
		struck = true;
		// Setup Audio
		AudioClip pointSound = Resources.Load<AudioClip>("Sounds/thunder");
		lightningSound = gameObject.AddComponent<AudioSource> ();
		lightningSound.clip = pointSound;
		if (GameController.sfxStatus) lightningSound.Play();
		// Strike UAV
		GameObject.Find ("uav").GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		// Render Bolt
		GameObject lightning = new GameObject (name: "bolt");
		// Sprite Rendering
		SpriteRenderer renderer = lightning.AddComponent<SpriteRenderer> ();
		renderer.sprite = Resources.Load<Sprite> ("Sprites/bolt");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 0;
		// Setting Position
		lightning.transform.localScale = new Vector3 (0.05f, 0.05f, lightning.transform.localScale.z);
		lightning.transform.position = new Vector3 (
			GameObject.Find ("uav").transform.position.x + 0.3f, GameObject.Find ("uav").transform.position.y + 0.5f
		);
		// Destroy Lighning after 0.5 seconds
		Invoke ("destroyLightning", 0.5f);
		GameController.message = ScoutController.lightningStrike;
		GameController.lost = true;
		GameController.inPlay = false;
	}

	private void handleLightning () {
		bool inHighYRegion = GameObject.Find ("uav").transform.position.y > screenDimensions.y - 1.5f;
		if (inHighYRegion && !struck && percipitation != 0) {
			if (Random.Range (1, chanceOfStrike) == 1) {
				handleStrike ();
			}
		}
	}

	private void destroyLightning () {
		Destroy (GameObject.Find ("bolt"));
	}

// ###############################################################
// Wind Swirl Functions 
// ###############################################################
	private IEnumerator makeSwirl () {
		GameObject swirl = new GameObject (name: "swirl");
		swirl.transform.parent = GameObject.Find ("Wind Swirls").transform;
		// Sprite Rendering
		swirlRenderer = swirl.AddComponent<SpriteRenderer> ();
		swirlRenderer.sortingLayerName = "Foreground";
		swirlRenderer.sortingOrder = 5;
		swirlRenderer.sprite = Resources.Load<Sprite>("Sprites/WindSwirl");
		setupSwirlMovement (swirl);
		// The more intense the wind, the shorter the wait
		yield return new WaitForSeconds (1f / windIntensity); 
		StartCoroutine (makeSwirl ());
	}

	private void setupSwirlMovement (GameObject swirl) {
		if (windForce > 0) {
			swirl.transform.position = new Vector2 (
				leftLimit.transform.position.x, Random.Range (-screenDimensions.y + 0.5f, screenDimensions.y - 0.5f)
			);
			StartCoroutine (moveSwirl (swirl, "Right"));
		} else {
			swirl.transform.position = new Vector2 (
				rightLimit.transform.position.x, Random.Range (-screenDimensions.y + 0.5f, screenDimensions.y - 0.5f)
			);
			StartCoroutine (moveSwirl (swirl, "Left"));
		}
	}

	private IEnumerator moveSwirl (GameObject swirl, string side) {
		Vector3 target = new Vector3 (
			(side == "Left" ? leftLimit.transform.position.x : rightLimit.transform.position.x), swirl.transform.position.y
		);
		swirl.transform.position = Vector3.MoveTowards (swirl.transform.position, target, Mathf.Abs (windForce) * 3);
		if ((windForce > 0 && swirl.transform.position.x >= (target.x - .01f)) ||
		    (windForce < 0 && swirl.transform.position.x <= (target.x + .01f))) {
			Destroy (swirl);
		} else {
			yield return new WaitForEndOfFrame ();
			StartCoroutine (moveSwirl (swirl, side));
		}
	}
}
