using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// ###############################################################
// Otter Generator Creates the Otter with region GameObject
// each GameObject has a
//
// Picture Region 	: PicRegionController
// Scared Zone 		: ScaredController
// Otter 			: OtterController
// 
// PicRegionController handles the UAV hovering in the trigger zone for the requisite time
// ScaredController handles the UAV hovering too low
// OtterController handles animating the otter surfacing and diving
// 
// OtterGenerator destroys the alpha for the "flash" animation and plays the shutter sound
// There are some cool dictionaries to allow the whole system to work, see more below
// ###############################################################
public class OtterGenerator : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// Public dictionaries
		// Mapping a scaredZone GameObject -> it's OtterController
			// Allows specific scaredzone to scare it's otter
			// Allows specific scaredzone to check if the otter has had it's picture taken
		// Mapping a picRegion GameObject -> it's OtterController
			// Allows specific piczone to tell it's otter the picture was taken
		// Mapping an Otter GameObject -> it's OtterWithRegion Master GameObject
			// Allows an otter to nuke it's Master GameObject
	public static Dictionary<GameObject, OtterController> scaredZoneToOtterController = new Dictionary<GameObject, OtterController> () {};
	public static Dictionary<GameObject, OtterController> picRegionToOtterController = new Dictionary<GameObject, OtterController> () {};
	public static Dictionary<GameObject, GameObject> otterToRegionObject = new Dictionary<GameObject, GameObject> () {};
		// GameObject naming strings (static so we don't have to instance the generator)
	private static string otterKey = "OtterRegion_";
	private static int spawnOtterCount = 0;
		// Public amount of otters
	public static int otterCount = 0;
		// Random spawn position (x axis)
	private Vector3 randomPosition;
		// Flash panel, OtterGenerator is responsible for decreasing the alpha
	private Image canvasImage;
	private bool initOtter;
	private AudioSource cameraShutter;
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		initOtter = false;
		// connect to the canvas image
		canvasImage = GameObject.Find("Canvas").GetComponent<Image>();
		// init statics to base values
		spawnOtterCount = 0;
		otterCount = 0;
		// set up shutter sound
		AudioClip soundClip = Resources.Load<AudioClip>("Sounds/camera-shutter");
		cameraShutter = gameObject.AddComponent<AudioSource> ();
		cameraShutter.clip = soundClip;
	}
	// ***************************************************************
	void Update () {
		if (!initOtter && !GameController.isTutorial) initOtters ();
		// create new random position
		randomPosition = new Vector3(Random.Range(5,GTP_Mission.copies*20),-3.8f);
		// if the current otterCount is less than the number of copies, create another otter
		if (otterCount < GTP_Mission.copies + 2 && !GameController.isTutorial && GameController.started) {
			createOtter(randomPosition);
		}
		// Continually nuke the canvasImage alpha value
		if (GameController.inPlay && canvasImage.color.a == 1) 	cameraShutter.Play();
		if (GameController.inPlay && canvasImage.color.a > 0) canvasImage.color = new Vector4 (1f,1f,1f,canvasImage.color.a-(Time.deltaTime/0.4f));
	}
// ###############################################################
// otter creation Functions
// ###############################################################
	void initOtters () {
		if (GameController.started) {
			randomPosition = new Vector3(Random.Range(5,GTP_Mission.copies*20),-3.8f);
			createOtter(randomPosition);
			randomPosition = new Vector3(Random.Range(5,GTP_Mission.copies*20),-3.8f);
			createOtter(randomPosition);
			initOtter = true;
		}
	}
	// ***************************************************************
	public static void createOtter (Vector3 position ) {
		// create new otterWithRegion from prefab @ randomPosition
		GameObject newOtter = Instantiate(Resources.Load<GameObject>("GamePrefabs/OtterWithRegion"), position, Quaternion.identity) as GameObject;
		// name the otter in birth order
		newOtter.transform.name = otterKey + spawnOtterCount.ToString();
		spawnOtterCount++; // increase the birth order
		otterCount++; // increase the otters on screen
		// map otter -> Master gameObject
		// map scaredzone -> otterController
		//map picregion -> otterController
		otterToRegionObject[newOtter.transform.FindChild("OtterAndRegion/Otter").gameObject] = newOtter;
		scaredZoneToOtterController[newOtter.transform.FindChild("OtterAndRegion/ScaredZone").gameObject] = newOtter.transform.FindChild("OtterAndRegion/Otter").GetComponent<OtterController>();
		picRegionToOtterController[newOtter.transform.FindChild("OtterAndRegion/Region").gameObject] = newOtter.transform.FindChild("OtterAndRegion/Otter").GetComponent<OtterController>();
	}
}