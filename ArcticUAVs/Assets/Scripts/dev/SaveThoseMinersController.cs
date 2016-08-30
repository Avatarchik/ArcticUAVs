using UnityEngine;
using System.Collections;

public class SaveThoseMinersController : MonoBehaviour {

	private bool elevatorMoving = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Vector3 target = GameObject.Find ("Elevator Floor").transform.position;
		if (GameObject.Find ("Elevator Floor").transform.position.y > -20f) {
			target.y = -20f;
		} else {
			target.y = -2.89f;
		}
		if (!elevatorMoving) {
			elevatorMoving = true;
			StartCoroutine (MoveElevator (target));
		}
	}

	IEnumerator MoveElevator (Vector3 target) {
		if (GameObject.Find ("Elevator Floor").transform.position != target) {
			GameObject.Find ("Elevator Floor").transform.position = Vector3.MoveTowards (GameObject.Find ("Elevator Floor").transform.position,
				target,
				1f * Time.deltaTime);
			yield return new WaitForEndOfFrame ();
			StartCoroutine (MoveElevator (target));
		} else {
			elevatorMoving = false;
		}
	}
}
