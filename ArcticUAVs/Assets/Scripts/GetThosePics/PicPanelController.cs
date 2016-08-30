using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
// ###############################################################
// PicPanelController handles displaying the otter pictures that the were taken in GTP
// ###############################################################
public class PicPanelController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################
	private List<int> otterIndeces = new List<int> ();
	private int currentIndex = 0;
// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		if (GameController.isTutorial) {
			GameObject.Find("Photo").GetComponent<Image> ().sprite = Resources.Load<Sprite>("Sprites/Otter Pics/" + Random.Range (1, 18));
			GameObject.Find ("Left Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0);
			GameObject.Find ("Right Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0);
		} else {
			GameController.message = "PICS CAPTURED!"; // Display winning message behind the view pictures panel
			GTP_Mission.localWon = true;
			GameController.inPlay = false;
			getRandomIndeces ();
			handleFirstPicturePanelDisplay ();
			}
	}
// ###############################################################
// PicPanel functions
// ###############################################################
	private void getRandomIndeces () {
		for (int i = 0; i < GameController.objectiveGained; i++) {
			int index = Random.Range (1, 19);
			// Make sure the new index is not already present (if possible)
			if (GameController.objectiveGained < 19) {
				while (otterIndeces.Contains (index)) {
					index = Random.Range (1, 19);
				}
			}
			otterIndeces.Add (index);
		}
		// [FUTURE WORK: re-Implement albino otters]
		// for (int i = 0; i < albinosCaptured; i++) {
		// 	otterIndeces.Add (19); // Add the albino index (or indeces)
		// }
	}
	// ***************************************************************
	private void handleFirstPicturePanelDisplay () {
		// Display the picture of the first index
		GameObject.Find ("Photo").GetComponent<Image> ().sprite = Resources.Load<Sprite>("Sprites/Otter Pics/" + otterIndeces[currentIndex]);
		// Dim the left next arrow to show there are no pictures to see in that direction
		GameObject.Find ("Left Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0);
		// If there is only 1 picture, also dim the right arrow
		if (otterIndeces.Count == 1) {
			GameObject.Find ("Right Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0);
		}
	}
// ###############################################################
// Button Functions
// ###############################################################
	public void nextPic () {
		if (currentIndex != GameController.objectiveGained - 1) {
			currentIndex += 1;
			GameObject.Find ("Left Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 1);
		}
		GameObject.Find ("Photo").GetComponent<Image> ().sprite = Resources.Load<Sprite>(
			"Sprites/Otter Pics/" + otterIndeces[currentIndex]);
		if (currentIndex == GameController.objectiveGained - 1) { 
			GameObject.Find ("Right Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0); 
		}
	}
	// ***************************************************************
	public void previousPic () {
		if (currentIndex != 0) {
			currentIndex -= 1;
			GameObject.Find ("Right Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 1);
		}
		GameObject.Find ("Photo").GetComponent<Image> ().sprite = Resources.Load (
			"Sprites/Otter Pics/" + otterIndeces[currentIndex], typeof(Sprite)
		) as Sprite;
		if (currentIndex == 0) { 
			GameObject.Find ("Left Next").GetComponent<Image> ().color = new Vector4 (1, 1, 1, 0); 
		}
	}
	// ***************************************************************
	public void hidePictures () {
		GameController.won = true;
		Destroy(GameObject.Find("ViewPicturesPanel(Clone)"));
	}
}