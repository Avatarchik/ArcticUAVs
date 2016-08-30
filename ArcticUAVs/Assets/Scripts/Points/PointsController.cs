using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// ###############################################################
// This is the PointsController class, responsible for 
// keeping/displaying the score, as well as determining if 
// a high score is achieved. If a high score is achieved, it 
// also handles saving that high score.
// ###############################################################
public class PointsController : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	public static int missionScore = 0;
	public static int objectiveScore = 0;
	public static int bonusGained = 0;
	public static int bonusScore = 0;

	private int _missionScore_ = 0;
	private int _objectiveScore_ = 0;
	private int _objectiveValue_ = 0;
	private int _bonusGained_ = 0;
	private int _bonusScore_ = 0;
	private int _bonusValue_ = 0;
	private int _nearMissValue_ = 0;
	
	private int displayedScore = 0;

// ###############################################################
// Functions 
// ###############################################################
	// Initialization (called once when gameObject is created).
	void Start () {
		InvokeRepeating ("animatePointsChange", 0, 0.01f);
	}

	// Called once per frame
	void Update () {
		objectiveScore = (GameController.objectiveGained * Difficulty.objectiveValue);
		bonusScore = (bonusGained * Difficulty.bonusValue) + (GameController.birdsMissed * Difficulty.nearMissValue);
		missionScore = bonusScore + objectiveScore;
		
		// used for editor monitoring
		_missionScore_ = missionScore;
		_objectiveScore_ = objectiveScore;
		_objectiveValue_ = Difficulty.objectiveValue;
		_bonusGained_ = bonusGained;
		_bonusScore_ = bonusScore;
		_bonusValue_ = Difficulty.bonusValue;
		_nearMissValue_ = Difficulty.nearMissValue;
	}
	// Animate any change in points for the displaying score
	private void animatePointsChange () {
		// if the mission score is greater then the score displaying on the screen
		if (missionScore > displayedScore) {
			// increase the display score
			if ((missionScore - displayedScore) > 50000) {
				displayedScore += 12345;
			} else if ((missionScore - displayedScore) > 5000) {
				displayedScore += 1234;
			} else if ((missionScore - displayedScore) > 500) {
				displayedScore += 123;
			} else if ((missionScore - displayedScore) > 50) {
				displayedScore += 12;
			} else {
				displayedScore += 1;
			}
		}
		// display changed score
		if (GameObject.Find("Points")){
			GameObject.Find ("Points").GetComponent<Text> ().text = displayedScore.ToString ();
		}
	}
}