using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.iOS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ###############################################################
// Button controller handles UI buttons across the game
// ###############################################################
public class ButtonController: MonoBehaviour {
// ###############################################################
// BUTTON Functions
// ###############################################################
	public void nextScene (string nextSceneName) {
		Time.timeScale = 1;
		SceneManager.LoadScene (nextSceneName, LoadSceneMode.Single);
	}
	public void setDifficulty (string difficulty) {
		// setDifficultly is attached to a button and sets the difficulty
		// This generates according Points, Weather Conditions, Battery drain & capacity, and objective count
		GameController.missionDiff = difficulty;
	}
	// Handles a press of FrontierScientists.com on Credits
	public void linkTo(String url) {
		Application.OpenURL (url);
	}
	// sameScene() is used for resets of the game
	public void sameScene () {
		Time.timeScale = 1;
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
	// Handle a press of NEXT button in tutorial
	public void tutorialContinue () {
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
        GameController.inPlay = true;
        GameController.popupDisplaying = false;
    }
    // Launch button in MTI
    public void landOrLaunchUAV() {
    	print("Land or Launch");
    	GameObject.Find("MTILandingZone").GetComponent<LandingZoneController>().landOrLaunch();
    }
// ###############################################################
// Pause management: pauseGame() and unpauseGame() handle the pause menu
// ###############################################################
	public void pauseGame () {
		Time.timeScale = 0; // magic
		if (ScoutController.propeller != null) ScoutController.propeller.Pause();
			// Most important part, GameController.inPlay HAS to be false for the [insert here] to pause correctly
		GameController.inPlay = false;
			// The pause menu prefab (UIPrefabs/PauseMenu) is only 1 part of the pause menu effect
			// The other part is an image of ~100/255 alpha that is attached DIRECTLY to the canvas
		GameObject canvas = GameObject.Find("Canvas");
			// Only if the pause menu doesn't exist, init!
		if (GameObject.Find("PauseMenu(Clone)") == null) {
				// canvas image creation
			Image canvasImage = canvas.GetComponent<Image>();
			canvasImage.color = new Vector4 (1f,1f,1f,100f/255f);
				//Creating the pause prefab
			GameController.buildGameObject("UIPrefabs/PauseMenu", new Vector2 (0f,0f), new Vector2 (0f,0f));
		}
	}
	public void unpauseGame () {
		// Destroy the grey fading
		Image canvasImage = GameObject.Find("Canvas").GetComponent<Image>();
		canvasImage.color = new Vector4 (1f,1f,1f,0f);
		// Destroy the pause menu prefab
		Destroy(GameObject.Find("PauseMenu(Clone)"));
		// if the start panel doesn't exist, set inPlay
		// The start panel is currently SetActive(false), but the transform search can find it
		if (GameObject.Find("Canvas").transform.Find("StartPanel(Clone)") == null) {
			GameController.inPlay = true;
		}
		Time.timeScale = 1;
		if (ScoutController.propeller != null) ScoutController.propeller.UnPause();
	}
// ###############################################################
// Watch Button Functions
// ###############################################################
	public void handleWatchButtonPress (string id) {
		StartCoroutine (checkInternetConnection ((isConnected) => {
			if (isConnected) { 
				playLongVideo (id); 
			} else {
				GameController.buildGameObject("UIPrefabs/NoInternetPanel", new Vector2 (-5f,-5f), new Vector2 (5f,5f));
			}
		}));
	}
	// ***************************************************************
	// Plays the mission's video from the pause menu, based on mission initials
	public void handlePauseWatchButtonPress () {
		string id = "B0w_JWdmq58"; // default is FS promo
		if (GameController.missionInitials == "LTF") {
			id = "s5dFBRVhIHA";//flying tools
		} else if (GameController.missionInitials == "LITH") {
			id = "0mc4Y7QVjQ0";//fly scout fly
		} else if (GameController.missionInitials == "GTP") {
			id = "T0jvYnnnVtU";//Uav over otters
		} else if (GameController.missionInitials == "MTI") {
			id = "wgjRe5NWZMo";//mapping ice trails by uav
		}
		// once the video id is set, check for internet connection
		StartCoroutine (checkInternetConnection ((isConnected) => {
			if (isConnected) { 
				playLongVideo (id); // if it's connected, play our video
			} else {
				// else display the no internet panel
				GameController.buildGameObject("UIPrefabs/NoInternetPanel", new Vector2 (-5f,-5f), new Vector2 (5f,5f));
			}
		}));
	}
	// ***************************************************************
	private IEnumerator checkInternetConnection(Action<bool> action) {
		WWW www = new WWW("http://google.com");
		yield return www;
		action (www.error == null);
	}
	// ***************************************************************
	private void playLongVideo (string id) {
		#if UNITY_IPHONE
			Handheld.SetActivityIndicatorStyle(ActivityIndicatorStyle.Gray);
		#elif UNITY_ANDROID
			Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Large);
		#endif
		Handheld.StartActivityIndicator();
		Handheld.PlayFullScreenMovie(YoutubeVideo.Instance.RequestVideo(id, 360), Color.black,FullScreenMovieControlMode.Full);
	}
	// ***************************************************************
	// button on NoInternetPanel
	public void acceptInternetSituation () {
		Destroy(GameObject.Find("NoInternetPanel(Clone)"));
	}
}