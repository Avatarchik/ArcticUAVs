// [FUTURE WORK]
// Implement Modular Weather
// 
// 
// 
// 
// 
// 
// 
// 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
	This is the WeatherController class, responsable for all weather related activities in the LandInTheHand and 
	GetThosePics scenes.
*/
public class WeatherController : MonoBehaviour {

/*
	Class Variables 
*/
	private Vector3 screenDimensions;
	private int cloudCover = 70;
	public int rain;
	private bool struck = false;
	private int strikeChance;
	private int windSpeed;
	private float windForce;
	private bool leftWind;

	// Difficulty Variables
	private int[] windSpeeds = {0,4,17,24,30};
	private int[] strikeChances = {0,0,5,20,75};

	// Connected Components
	private GameObject rainPanel;
	private GameObject weatherPanel;
	private GameObject leftSpawnEdge;
	private GameObject rightSpawnEdge;

	// Sounds
	private AudioSource rainSound;
	private AudioSource lightningSound;



/*
	Class Functions 
*/
	// Initialization (called once when gameObject is created).
	void Start () {
		// Instantiate UI objects
		weatherPanel = GameController.buildGameObject("UIPrefabs/WindPanel", new Vector2(0f,0f), new Vector2(0f,0f));
		// rainPanel = GameController.buildGameObject("UIPrefabs/RainPanel", new Vector2(0f,0f), new Vector2(0f,0f));
		// rainPanel.GetComponent<Image> ().color = new Color(0f, 0f, 0f, Difficulty.weatherIntesity * 0.15f);
		// rainPanel.transform.setAsFirstSibling();

		leftSpawnEdge = GameObject.Find ("LeftSpawnEdge");
		rightSpawnEdge = GameObject.Find ("RightSpawnEdge");
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

		windSpeed = Random.Range (0, windSpeeds[Difficulty.weatherIntesity]);
		if (windSpeed > 0) {
			leftWind = Random.Range (0, 1) == 1;
		}

		rain = Random.Range (0, Difficulty.weatherIntesity);
		rain = 4;

		strikeChance = strikeChances [Difficulty.weatherIntesity];

		displayWindStats ();
		setupClouds ();
		// if (Difficulty.weatherIntesity != 0) {
		// 	StartCoroutine (makeSwirl ());
		// }

		AudioClip rainClip = (AudioClip)Resources.Load("Sounds/rain", typeof(AudioClip));
		rainSound = gameObject.AddComponent<AudioSource> ();
		rainSound.clip = rainClip;
		rainSound.volume = .2f;
		rainSound.loop = true;
		if (rain > 0) {
			if (GameController.sfxStatus) rainSound.Play();}
	}
	// Called once per frame
	void Update () {
		makeRain ();
		handleLightning ();
	}

	// Create as many specified by the rain intensity
	private void makeRain () {
		for (int i = 0; i < rain; i++) { 
			GameObject raindrop = Instantiate(Resources.Load<GameObject>("GamePrefabs/Raindrop"));
			float xpos = Random.Range(leftSpawnEdge.transform.position.x, rightSpawnEdge.transform.position.x);
			raindrop.transform.position = new Vector3(xpos, 5f, 0f);
		}
	}

	private void displayWindStats () {
		GameObject.Find ("Wind Speed").GetComponent<Text> ().text = ((int)(Mathf.Abs(windSpeed) * 100)).ToString();
		if (windSpeed != 0) {
			Image windDirection = GameObject.Find ("Wind Direction").AddComponent<Image> ();
			if (leftWind) {
				windDirection.sprite = Resources.Load<Sprite> ("Sprites/rightArrow");
			} else {
				windDirection.sprite = Resources.Load<Sprite> ("Sprites/leftArrow");
			}
		}
	}

	private void handleLightning () {
		bool inHighYRegion = GameObject.Find ("uav").transform.position.y > screenDimensions.y - 1.5f;
		if (inHighYRegion && !struck && rain != 0) {
			if (Random.Range (1, strikeChance) == 1) {
				handleStrike ();
			}
		}
	}

	private void handleStrike () {
		struck = true;
		
		AudioClip pointSound = Resources.Load<AudioClip>("Sounds/thunder");
		lightningSound = gameObject.AddComponent<AudioSource> ();
		lightningSound.clip = pointSound;
		if (GameController.sfxStatus) lightningSound.Play();

		GameObject.Find ("uav").GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		GameObject lightning = new GameObject (name: "bolt");
		setupBoltRenderer (lightning.AddComponent<SpriteRenderer> ());
		lightning.transform.localScale = new Vector3 (0.05f, 0.05f, lightning.transform.localScale.z);
		lightning.transform.position = new Vector3 (
			GameObject.Find ("uav").transform.position.x + 0.3f, GameObject.Find ("uav").transform.position.y + 0.5f
		);
		Invoke ("destroyLightning", 0.5f);
		GameController.message = ScoutController.lightningStrike;
		GameController.lost = true;
		GameController.inPlay = false;
	}

	private void destroyLightning () {
		Destroy (GameObject.Find ("bolt"));
	}

	private IEnumerator makeSwirl () {
		// Load GameObject from Prefabs
		GameObject swirl = Instantiate(Resources.Load<GameObject>("GamePrefabs/windSwirl"));
		swirl.transform.parent = GameObject.Find ("Wind Swirls").transform;
		// Set swirl position
		if (leftWind) {
			swirl.transform.position = new Vector2 (
				leftSpawnEdge.transform.position.x, Random.Range (-screenDimensions.y + 1f, screenDimensions.y - 1f)
			);
			StartCoroutine (moveSwirl (swirl, false));
		} else {
			swirl.transform.position = new Vector2 (
				rightSpawnEdge.transform.position.x, Random.Range (-screenDimensions.y + 1f, screenDimensions.y - 1f)
			);
			StartCoroutine (moveSwirl (swirl, true));
		}
		yield return new WaitForSeconds (1f / Difficulty.weatherIntesity); // The more intense the wind, the shorter the wait
		StartCoroutine (makeSwirl ());
	}

	private IEnumerator moveSwirl (GameObject swirl, bool leftSide) {
		Vector3 target;
		if (leftSide) {
			target = new Vector3 (leftSpawnEdge.transform.position.x, swirl.transform.position.y);
		} else {
			target = new Vector3 (rightSpawnEdge.transform.position.x, swirl.transform.position.y);
		}

		swirl.transform.position = Vector3.MoveTowards (swirl.transform.position, target, windSpeed * Time.deltaTime);
		if ((leftWind && swirl.transform.position.x >= (target.x - .01f)) || (!leftWind && swirl.transform.position.x <= (target.x + .01f))) {
			Destroy (swirl);
		} else {
			yield return new WaitForEndOfFrame ();
			StartCoroutine (moveSwirl (swirl, leftSide));
		}
	}

	private void setupBoltRenderer (SpriteRenderer renderer) {
		renderer.sprite = Resources.Load<Sprite>("Sprites/bolt");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 0;
	}

	private SpriteRenderer setupCloudRenderer (GameObject cloud) {
		SpriteRenderer renderer = cloud.AddComponent<SpriteRenderer> ();
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 7;
		return renderer;
	}

	private void setupClouds () {
		for (int clouds = 0; clouds < cloudCover; clouds++) {
			GameObject cloud = new GameObject (name: "cloud");
			cloud.transform.parent = GameObject.Find ("Clouds").transform;
			SpriteRenderer cloudRenderer = setupCloudRenderer (cloud);
			if (rain == 0) { // Rain cloud or not
				cloudRenderer.sprite = Resources.Load<Sprite> ( "Sprites/Clouds/cloud" + Random.Range (1, 6));
			} else {
				cloudRenderer.sprite = Resources.Load<Sprite> ("Sprites/Rain Clouds/rainCloud" + Random.Range (1, 6));
			}
			cloud.transform.position = new Vector2 (
				Random.Range (leftSpawnEdge.transform.position.x, rightSpawnEdge.transform.position.x + 1), 
				Random.Range (screenDimensions.y - 0.25f, screenDimensions.y)
			);
			cloud.transform.localScale = new Vector3 (0.3f, 0.3f, cloud.transform.localScale.z);
		}
	}
}
