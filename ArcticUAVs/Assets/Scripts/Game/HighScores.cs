using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// ###############################################################
// HighScores is responsible for the manegment of the high score system.
// It checks if a score is in the top five of the high scores list, 
// as well as resorting the list after a new high score has been added.
// It also saves the high scores to the playerprefs to be loaded
// the next time the game loads.
// ###############################################################
public class HighScores : MonoBehaviour {

// ###############################################################
// Variables
// ###############################################################
	// ###############################################################
	// HighScore is a container for a high score.
	// It contains a string for the player name and
	// an int for the player score.
	// ###############################################################
	public class HighScore {
		public string name;
		public int score;

		public HighScore() {
			name = "- - -";
			score = 0;
		}

		public HighScore(string name_, int score_) {
			name = name_;
			score = score_;
		}
	}

	public List<HighScore> highScores;

// ###############################################################
// Class Functions
// ###############################################################
	// Default constructor
	public HighScores() {
		highScores = new List<HighScore>(5);
	}
	// Add function, add's the score to the high score's list and resorts the list accordingly
	public void add(string name, int score) {
		highScores.Add(new HighScore(name, score));
		sort();
		highScores.Reverse();
		while (highScores.Count > 5) {
			highScores.RemoveAt(5);
		}
	}
	// Change the name associated with a given score rank
	public void changeName(string name, int idx) {
		highScores[idx - 1].name = name;
	}
	// Provide single level access to the high scores
	public HighScore this[int key]
	{
		get {
			return highScores[key];
		}
	}
	// Check if a score ranks into a high score and return where it ranks
	public int checkScore(int score) {
		int i;
		for (i = 0; i < highScores.Count; ++i) {
			if (score > highScores[i].score) break;
		}
		return i+1;
	}
	// Sort the HighScore objects based on the score
	private void sort() {
		highScores.Sort((a,b) => a.score.CompareTo(b.score));
	}
	// Return the number of high scores.
	// This allows for increased number of high scores later
	public int count() {
		return highScores.Count;
	}
	// pretty print the high scores
	public void print() {
		int i = 0;
		foreach (HighScore s in highScores) 
		{	
			++i;
			print(i.ToString() + ": " + s.name + " = " + s.score.ToString());
		}
	}
	// save the high scores to the PlayerPrefs
	public void save(string missionInitials) {
		for (int i = 1; i <= 5; ++i) {
			PlayerPrefs.SetInt (missionInitials + i.ToString () + "Score", highScores[i-1].score);
			PlayerPrefs.SetString (missionInitials + i.ToString () + "Name", highScores[i-1].name);
		}
	}
	// load the high scores from the PlayerPrefs
	public void load(string missionInitials) {
		for (int i = 1; i <= 5; ++i) {
			if (!PlayerPrefs.HasKey (missionInitials + i.ToString () + "Score")) {
				PlayerPrefs.SetInt (missionInitials + i.ToString () + "Score", 0);
				PlayerPrefs.SetString (missionInitials + i.ToString () + "Name", "- - -");
				add("- - -", 0);
			} else {					
				int score = PlayerPrefs.GetInt (missionInitials + i.ToString () + "Score");
				string name = PlayerPrefs.GetString (missionInitials + i.ToString () + "Name");
				add(name, score);
			}
		}
	}
}
