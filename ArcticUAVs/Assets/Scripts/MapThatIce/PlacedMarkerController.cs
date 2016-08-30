using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// ###############################################################
// PlacedMarkerController responsible for the animation of placed
// markers in the MTI missions.
// ###############################################################
public class PlacedMarkerController : MonoBehaviour {

	void Start () {
		StartCoroutine (animateMarker (new Vector3 (transform.position.x, 0.5f, transform.position.z)));
	}

	void OnTriggerEnter (Collider collider) {
		Destroy (gameObject);
	}

	// animate the marker
	IEnumerator animateMarker (Vector3 target) {
		if (transform.position != target) {
			transform.position = Vector3.MoveTowards (transform.position, target, 0.5f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
			StartCoroutine (animateMarker (target));
		} else {
			if (target.y > 0) {
				StartCoroutine (animateMarker (new Vector3 (transform.position.x, 0, transform.position.z)));
			} else {
				StartCoroutine (animateMarker (new Vector3 (transform.position.x, 0.5f, transform.position.z)));
			}
		}
	}
}
