using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/*
	This is the StatisticsController class.  It simply displays statistics in the Statistics scene from the information 
	gathered in demo mode.
*/
public class StatisticsController : MonoBehaviour {

/*
	Class Functions 
*/
	// Initialization (called once when gameObject is created).
	void Start () {
		// Display total plays
		GameObject.Find ("Total Plays").GetComponent<Text> ().text = "Total plays: " + PlayerPrefs.GetInt ("Plays");
		// Fill in question 1 stats
		GameObject.Find ("Lame Percentage").GetComponent<Text> ().text = 
			"Lame: " + ((PlayerPrefs.GetFloat ("Lames") / PlayerPrefs.GetInt ("q1Answers")) * 100) + "%";
		GameObject.Find ("Okay Percentage").GetComponent<Text> ().text = 
			"Okay: " + ((PlayerPrefs.GetFloat ("Okays") / PlayerPrefs.GetInt ("q1Answers"))  * 100) + "%";
		GameObject.Find ("Awesome Percentage").GetComponent<Text> ().text = 
			"Awesome: " + ((PlayerPrefs.GetFloat ("Awesomes") / PlayerPrefs.GetInt ("q1Answers")) * 100) + "%";
		// Fill in question 2 stats
		GameObject.Find ("Not At All Percentage").GetComponent<Text> ().text = 
			"Not At All: " + ((PlayerPrefs.GetFloat ("Not At Alls") / PlayerPrefs.GetInt ("q2Answers")) * 100) + "%";
		GameObject.Find ("Sort Of Percentage").GetComponent<Text> ().text = 
			"Sort Of: " + ((PlayerPrefs.GetFloat ("Sort Ofs") / PlayerPrefs.GetInt ("q2Answers")) * 100) + "%";
		GameObject.Find ("Absolutely Percentage").GetComponent<Text> ().text = 
			"Absolutely: " + ((PlayerPrefs.GetFloat ("Absolutelys") / PlayerPrefs.GetInt ("q2Answers")) * 100) + "%";
		// Fill in suggestions
		GameObject.Find ("Content").GetComponent<Text> ().text = PlayerPrefs.GetString ("Suggestions");
	}
}
