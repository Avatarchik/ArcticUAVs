using UnityEngine;
// ###############################################################
// Circle controller watches the circle for the UAV to enter it && rotates the circle
// ###############################################################
public class CircleController : MonoBehaviour {
// ###############################################################
// Unity Functions
// ###############################################################
	void Update () {gameObject.transform.Rotate(new Vector3(0f,0f,0.75f));}
	void OnTriggerEnter2D (Collider2D other) {
		GameController.objectiveGained++;
		GameController.circleOnScreen = false;
		Destroy (gameObject);
	}
}