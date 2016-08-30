using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// ###############################################################
// Opening handles the opening UI, and releases the game to begin playing
// ###############################################################
public class Opening : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	GameObject startPanelPrefab;
	GameObject batteryPanel;
	private float timePoint = 0f;
	private Text number;
// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		// Create start panel
		startPanelPrefab = GameController.buildGameObject("UIPrefabs/StartPanel", new Vector2 (0f,0f), new Vector2 (0f,0f) );
		// Connect to UI
		number = GameObject.Find("Number").GetComponent<Text>();
		// If the game is NOT a tutorial, instantiate the battery charge icon
		if (!GameController.isTutorial) {
			batteryPanel = GameController.buildGameObject("UIPrefabs/BatteryPanel", new Vector2 (-25f,-35f), new Vector2(25f,35f));
			batteryPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2 (-25f,-35f);
			batteryPanel.transform.SetAsFirstSibling();
		}
		// set the challenge text (declared by scene controller)
		GameObject.Find ("Challenge").GetComponent<Text> ().text = GameController.challenge;
	}
	void Update () {
		// gameClock isn't running, so we have our own clock
		timePoint += Time.deltaTime;
		// Activate the panel
		startPanelPrefab.SetActive(true);
		// If the pause button is pressed, hide us, and don't increase the time
		if (GameObject.Find("PauseMenu(Clone)")) {
			startPanelPrefab.SetActive(false);
			timePoint -= Time.deltaTime;
		}
		// Set to 3... 2... 1... release the player to game play
 		if (timePoint > 0f) 	number.text = "3";
 		if (timePoint > 1f) 	number.text = "2";
 		if (timePoint > 2f) 	number.text = "1";
 		if (timePoint > 3f) 	go();
	}
// ###############################################################
// Opening functions
// ###############################################################
	private void go () {
			// Release gameplay, start the game, destroy the UI
			GameController.inPlay = true;
			GameController.started = true;
			Destroy (GameObject.Find ("StartPanel(Clone)"));
		// Reset the clock and destroy the opening script
		GameController.gameClock = 0;
		Destroy (gameObject);
	}
} // fin