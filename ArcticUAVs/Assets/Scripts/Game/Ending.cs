using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using PC = PointsController;
// ###############################################################
// Ending is created by GameController on start, Ending waits for other scripts to set GC.won || GC.lost
// Ending handles the end game UI
//
// Win no high score 	: Success panel no text box for new high score
// Win high score 		: Success panel with text box for new high score (1-3 intitals)
// loss					: Fail panel with message
// tutorial  [SPECIAL] 	: Tutorial panel
//
// All of the above are prefabs in Resources/UIPrefabs
// ###############################################################
public class Ending : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	// UI
	private GameObject endPanel;
	// Scoring information
	private HighScores missionHS;
	private int scoreRank;
// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		// Load the high scores
		missionHS = new HighScores();
		missionHS.load(GameController.missionInitials);
	}
	void Update () {
		if (GameObject.Find("EndPanel") == null) {
			// if the game won, build the success panel
			if (GameController.won) {
				Destroy(GameObject.Find("Pause Button"));
				// if the mission is a tutorial, build the tutorial sucess panel
				if (GameController.isTutorial) {
					buildTutorialSuccess();
					GameController.started = false;
					StartCoroutine(calculateScore());
				// else, build the normal sucess panel
				} else {
					print("Score:" + PC.missionScore.ToString());
					scoreRank = missionHS.checkScore(PC.missionScore);
					
					buildSuccessPanel();
					GameController.started = false;
					StartCoroutine(calculateScore());
					updateHighScores();
				}
			}
			// if the game is lost, build the fail menu
			if (GameController.lost) {
				Destroy(GameObject.Find("Pause Button"));
				buildFailPanel();
				GameController.started = false;
			}
		} else {
			// Don't allow null entries into a high scrore field
			if (GameObject.Find("InputField") != null) {
				string name = GameObject.Find("InputField/Player Name").GetComponent<Text>().text;
				if (name == "") disableContinue();
				else enableContinue();
				missionHS.changeName(name, scoreRank);
				missionHS.save(GameController.missionInitials);
			}
			// if the game is not lost and not won, 
			// destroy any endpanel that might be displaying
			if (!GameController.won && !GameController.lost) {
				Destroy(GameObject.Find("End Panel"));
			}
		}
	}

// ############################################################
// Building end panel functions
// ############################################################
	
	private void buildSuccessPanel () {
		// Create the end panel, name it, set the mission specific fields
		endPanel = GameController.buildGameObject("UIPrefabs/SuccessPanel", new Vector2(-300f,-195f), new Vector2(300f,195f));
		endPanel.name = "EndPanel";
		GameObject.Find("Objective Icon").GetComponent<Image>().sprite = GameObject.Find("ObjectiveIcon").GetComponent<Image>().sprite; 
		GameObject.Find("Bonus Icon").GetComponent<Image>().sprite = GameObject.Find("BonusIcon").GetComponent<Image>().sprite; 
		GameObject.Find("Message").GetComponent<Text>().text = GameController.message;
		// Populate the high scores
		Transform HighScoresPanel = endPanel.transform.Find("High Scores");
		for (int i = 0; i < missionHS.count(); ++i) {
			HighScoresPanel.Find("High Score " + (i+1).ToString() + "/Name").GetComponent<Text>().text = missionHS[i].name;
			HighScoresPanel.Find("High Score " + (i+1).ToString() + "/Score").GetComponent<Text>().text = missionHS[i].score.ToString();
		}
	}

	private void buildTutorialSuccess () {
		// Create the end panel, name it, set the mission specific fields
		endPanel = GameController.buildGameObject("UIPrefabs/TutorialSuccessPanel", new Vector2(-150f,-150f), new Vector2(150f,150f));
		endPanel.name = "EndPanel";
		GameObject.Find("Objective Icon").GetComponent<Image>().sprite = GameObject.Find("ObjectiveIcon").GetComponent<Image>().sprite;
		GameObject.Find("Bonus Icon").GetComponent<Image>().sprite = GameObject.Find("BonusIcon").GetComponent<Image>().sprite;
		GameObject.Find("Message").GetComponent<Text>().text = GameController.message;
	}

	private void buildFailPanel () {
		// Create the end panel, name it, set the mission specific fields
		endPanel = GameController.buildGameObject("UIPrefabs/FailPanel", new Vector2(-150f,-100f), new Vector2(150f,100f));
		endPanel.name = "EndPanel";
		GameObject.Find("Message").GetComponent<Text>().text = GameController.message;
	}

	// Disable the accept and retry buttons until a character has been entered
	private void disableContinue() {
		// disable accept button
		GameObject accept = GameObject.Find("Accept");
		Button button = accept.transform.Find("Button").GetComponent<Button>();
		Text text = accept.transform.Find("Text").GetComponent<Text>();
		Image icon = accept.transform.Find("Icon").GetComponent<Image>();
		button.interactable = false;
		text.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 0.5f);
		icon.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 0.5f);

		// disable retry button
		GameObject retry = GameObject.Find("Retry");
		button = retry.transform.Find("Button").GetComponent<Button>();
		text = retry.transform.Find("Text").GetComponent<Text>();
		icon = retry.transform.Find("Icon").GetComponent<Image>();
		button.interactable = false;
		text.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 0.5f);
		icon.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 0.5f);
	}
	// Once the user has inputed a character, enable the accept and retry button
	private void enableContinue() {
		// enable accept button
		GameObject accept = GameObject.Find("Accept");
		Button button = accept.transform.Find("Button").GetComponent<Button>();
		Text text = accept.transform.Find("Text").GetComponent<Text>();
		Image icon = accept.transform.Find("Icon").GetComponent<Image>();
		button.interactable = true;
		text.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 1f);
		icon.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 1f);

		// enable retry button
		GameObject retry = GameObject.Find("Retry");
		button = retry.transform.Find("Button").GetComponent<Button>();
		text = retry.transform.Find("Text").GetComponent<Text>();
		icon = retry.transform.Find("Icon").GetComponent<Image>();
		button.interactable = true;
		text.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 1f);
		icon.color = new Vector4(71f/255f, 71f/255f, 71f/255f, 1f);
	}

// ############################################################
// Scoring Functions
// ############################################################
		
	IEnumerator calculateScore() {
		yield return StartCoroutine(animateScoreDisplay("Objective Score", PC.objectiveScore));
		yield return StartCoroutine(animateScoreDisplay("Bonus Score", PC.bonusScore));
		yield return StartCoroutine(animateScoreDisplay("Total Score", PC.missionScore));
	}

	IEnumerator animateScoreDisplay (string ScoreText, int score) {
		print(ScoreText);
		int displayedScore = 0;
		while (score > displayedScore) 
		{
			if ((score - displayedScore) > 25000) {
				displayedScore += 12345;
			} else if ((score - displayedScore) > 2500) {
				displayedScore += 1234;
			} else if ((score - displayedScore) > 250) {
				displayedScore += 123;
			} else if ((score - displayedScore) > 25) {
				displayedScore += 12;
			} else {
				displayedScore += 1;
			}
			GameObject.Find (ScoreText).GetComponent<Text> ().text = displayedScore.ToString ();
			yield return new WaitForSeconds(0.01f);
		}
	}

	private void updateHighScores() {
		if (scoreRank < 6) {
			missionHS.add("TMP", PC.missionScore);
		}

		Transform HighScoresPanel = endPanel.transform.Find("High Scores");
		print(scoreRank);
		for (int i = 0; i < missionHS.count(); ++i) {
			HighScoresPanel.Find("High Score " + (i+1).ToString() + "/Name").GetComponent<Text>().text = missionHS[i].name;
			HighScoresPanel.Find("High Score " + (i+1).ToString() + "/Score").GetComponent<Text>().text = missionHS[i].score.ToString();
		}
		if (scoreRank < 6) HighScoresPanel.Find("High Score " + scoreRank.ToString() + "/InputField").gameObject.SetActive(true);
	}
}
