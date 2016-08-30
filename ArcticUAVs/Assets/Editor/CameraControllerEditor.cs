using UnityEngine;
using UnityEditor;
using System.Collections;

/*
	This is the CameraControllerEditor class.  It handles the display and behavior of the CameraController editor in the 
	Inspector.
*/
[CustomEditor(typeof(CameraController))]
public class CameraControllerEditor : Editor {

/*
	Class Variables 
*/
	private CameraController cameraController = null;		

/*
	Class Functions 
*/
	void OnEnable() {
		cameraController = (CameraController) target;
	}
	public override void OnInspectorGUI() {
		formatToFollowField ();
		formatFreeXToggle ();
		handleFreeXToggle ();
		formatFreeYToggle ();
		handleFreeYToggle ();
		formatFreeZToggle ();
		handleFreeZToggle (); 
	}

/*
	Content and Helper Functions 
*/
	private void formatFreeXToggle () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Free X", GUILayout.Width (70));
		cameraController.freeX = EditorGUILayout.Toggle (cameraController.freeX);
		GUILayout.EndHorizontal ();
	}

	private void formatFreeYToggle () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Free Y", GUILayout.Width (70));
		cameraController.freeY = EditorGUILayout.Toggle (cameraController.freeY);
		GUILayout.EndHorizontal ();
	}

	private void formatFreeZToggle () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Free Z", GUILayout.Width (70));
		cameraController.freeZ = EditorGUILayout.Toggle (cameraController.freeZ);
		GUILayout.EndHorizontal ();
	}

	private void formatMaxX () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Max X", GUILayout.Width (70));
		GUILayout.Label ("None:", GUILayout.Width (35));
		cameraController.noXMax = EditorGUILayout.Toggle (cameraController.noXMax);
		if (!cameraController.noXMax) {
			cameraController.xMax = EditorGUILayout.FloatField (cameraController.xMax);
		}
		GUILayout.EndHorizontal ();
	}

	private void formatMaxY () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Max Y", GUILayout.Width (70));
		GUILayout.Label ("None:", GUILayout.Width (35));
		cameraController.noYMax = EditorGUILayout.Toggle (cameraController.noYMax);
		if (!cameraController.noYMax) {
			cameraController.yMax = EditorGUILayout.FloatField (cameraController.yMax);
		}
		GUILayout.EndHorizontal ();
	}

	private void formatMaxZ () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Max Z", GUILayout.Width (70));
		GUILayout.Label ("None:", GUILayout.Width (35));
		cameraController.noZMax = EditorGUILayout.Toggle (cameraController.noZMax);
		if (!cameraController.noZMax) {
			cameraController.zMax = EditorGUILayout.FloatField (cameraController.zMax);
		}
		GUILayout.EndHorizontal ();
	}

	private void formatMinX () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Min X", GUILayout.Width (70));
		GUILayout.Label ("None:", GUILayout.Width (35));
		cameraController.noXMin = EditorGUILayout.Toggle (cameraController.noXMin);
		if (!cameraController.noXMin) {
			cameraController.xMin = EditorGUILayout.FloatField (cameraController.xMin);
		}
		GUILayout.EndHorizontal ();
	}

	private void formatMinY () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Min Y", GUILayout.Width (70));
		GUILayout.Label ("None:", GUILayout.Width (35));
		cameraController.noYMin = EditorGUILayout.Toggle (cameraController.noYMin);
		if (!cameraController.noYMin) {
			cameraController.yMin = EditorGUILayout.FloatField (cameraController.yMin);
		}
		GUILayout.EndHorizontal ();
	}

	private void formatMinZ () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Min Z", GUILayout.Width (70));
		GUILayout.Label ("None:", GUILayout.Width (35));
		cameraController.noZMin = EditorGUILayout.Toggle (cameraController.noZMin);
		if (!cameraController.noZMin) {
			cameraController.zMin = EditorGUILayout.FloatField (cameraController.zMin);
		}
		GUILayout.EndHorizontal ();
	}

	private void formatToFollowField () {
		GUILayout.BeginHorizontal ();
		GUILayout.Label ("To Follow", GUILayout.Width (70));
		cameraController.toFollow = (GameObject) EditorGUILayout.ObjectField (
			cameraController.toFollow, typeof (GameObject), true
		); 
		GUILayout.EndHorizontal ();
	}

	private void handleFreeXToggle () {
		if (cameraController.freeX) {
			formatMinX ();
			formatMaxX ();
		}
	}

	private void handleFreeYToggle () {
		if (cameraController.freeY) {
			formatMinY ();
			formatMaxY ();
		}
	}

	private void handleFreeZToggle () {
		if (cameraController.freeZ) {
			formatMinZ ();
			formatMaxZ ();
		}
	}
}
