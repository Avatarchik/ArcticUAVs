using System.Collections;
using UnityEngine;
// ###############################################################
// deprecated as of 7/18/2016 [REMOVE?]
// ###############################################################
/*
	This is the OrientationSetter class, used in Main.cs to set the orientation. [MAIN NOW DOES THIS...]
*/
public class OrientationSetter : MonoBehaviour {

/*
	Content and Helper Functions 
*/
	public static void setOrientation (string orientation) {
		print("orientation");
		if (orientation == "portrait") {
			Screen.autorotateToPortrait = true;
			Screen.autorotateToPortraitUpsideDown = true;
		} else if (orientation == "landscape") {
			Screen.autorotateToLandscapeLeft = true;
			Screen.autorotateToLandscapeRight = true;
		}
		Screen.orientation = ScreenOrientation.AutoRotation;
	}
}
