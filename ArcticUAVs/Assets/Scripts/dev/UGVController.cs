using UnityEngine;
using System.Collections;

public class UGVController : MonoBehaviour {

	public WheelJoint2D rightWheel;
	public WheelJoint2D leftWheel;
	private JointMotor2D motor1;
	private JointMotor2D motor2;
	private bool accelerate = false;
	private bool decelerate = false;
	private bool tiltLeft = false;
	private bool tiltRight = false;
	
	// Update is called once per frame
	void Update () {
		if (accelerate || decelerate) {
			motor1 = rightWheel.motor;
			motor2 = leftWheel.motor;
			rightWheel.useMotor = true;
			leftWheel.useMotor = true;

			if (accelerate) {
				if (motor1.motorSpeed > -10000) {
					motor1.motorSpeed -= 10;
				}
				if (motor2.motorSpeed > -10000) {
					motor2.motorSpeed -= 10;
				}
			}
			if (decelerate) {
				motor1.motorSpeed += 10;
				motor2.motorSpeed += 10;
			}

			rightWheel.motor = motor1;
			leftWheel.motor = motor2;
		} else {
//			motor1 = rightWheel.motor;
//			motor1.motorSpeed = 0;
//			rightWheel.motor = motor1;
//			motor2 = leftWheel.motor;
//			motor2.motorSpeed = 0;
//			leftWheel.motor = motor2;
			rightWheel.useMotor = false;
			leftWheel.useMotor = false;
		}

		if (tiltLeft) {
			GameObject.Find ("UGV").GetComponent<Rigidbody2D> ().angularVelocity += 10;
		}

		if (tiltRight) {
			GameObject.Find ("UGV").GetComponent<Rigidbody2D> ().angularVelocity -= 10;
		}
	}

	public void Accelerate () {
		accelerate = true;
	}

	public void StopAccelerating () {
		accelerate = false;
	}

	public void Decelerate () {
		decelerate = true;
	}

	public void StopDecelerating () {
		decelerate = false;
	}

	public void TiltLeft () {
		tiltLeft = true;
	}

	public void StopTiltingLeft () {
		tiltLeft = false;
	}

	public void TiltRight () {
		tiltRight = true;
	}

	public void StopTiltingRight () {
		tiltRight = false;
	}
}
