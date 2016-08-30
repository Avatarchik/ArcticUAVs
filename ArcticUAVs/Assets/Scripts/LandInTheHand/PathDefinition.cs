using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ###############################################################
// PathDefinition is responsible for iterating over the given points for FollowPath.
// It works closely with FollowPath by holding the points of the path to follow.
// ###############################################################
public class PathDefinition : MonoBehaviour {

// ###############################################################
// Class Variables 
// ###############################################################
	public Transform[] points;

// ###############################################################
// Content and Helper Functions 
// ###############################################################
	public IEnumerator<Transform> getPathEnumerator () {
		if (points == null || points.Length < 1) {
			yield break;
		}
		var direction = 1;
		var index = 0;
		while (true) {
			yield return points [index];
			if (points.Length == 1) {
				continue;
			}
			if (index <= 0) {
				direction = 1;
			} else if (index >= points.Length - 1) {
				direction = -1;
			}
			index = index + direction;
		}
	}

	public void onDrawGizmosSelected () {
		print("hello");
		if (points == null || points.Length < 2) {
			return;
		}
		for (var i = 1; i < points.Length; i++) {
			Gizmos.DrawLine (points [i - 1].position, points [i].position);
		}
	}
}
