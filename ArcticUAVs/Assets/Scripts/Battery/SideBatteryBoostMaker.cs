using System.Collections;
using UnityEngine;
// ###############################################################
// deprecated as of 7/18/2016 [REMOVE?]
// ###############################################################
public class SideBatteryBoostMaker : MonoBehaviour {
	private Vector3 screenDimensions;
	void Start () {
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		InvokeRepeating ("spawnBatteryBoost", 5, 8);
	}
	// Called once per frame
	void Update () {
		screenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
	}
	private void setupBoostRenderer (SpriteRenderer renderer) {
		renderer.sprite = Resources.Load<Sprite> ("Sprites/batteryBoostIcon");
		renderer.sortingLayerName = "Foreground";
		renderer.sortingOrder = 5;
	}
	void spawnBatteryBoost () {
		GameObject boost = new GameObject ("boost");
		setupBoostRenderer (boost.AddComponent<SpriteRenderer> ());
		boost.transform.parent = GameObject.Find ("Battery Boosts").transform;
		boost.transform.localScale = new Vector3 (0.5f, 0.5f, 1f);
		boost.transform.position = new Vector3 (
			Random.Range (
				GameObject.Find ("LeftSpawnEdge").transform.position.x + 0.45f, 
				GameObject.Find ("RightSpawnEdge").transform.position.x - 0.45f
			), 
			Random.Range (-screenDimensions.y + 2f, screenDimensions.y - 1f)
		);
		boost.AddComponent<PolygonCollider2D> ();
		boost.GetComponent<PolygonCollider2D> ().isTrigger = true;
		boost.AddComponent<BatteryBoostController> ();
		// boost.GetComponent<BatteryBoostController> ().view = "Side";
		AudioClip boostSound = (AudioClip)Resources.Load("Sounds/battery-boost", typeof(AudioClip));
		AudioSource sound = boost.AddComponent<AudioSource> ();
		sound.clip = boostSound;
	}
}
