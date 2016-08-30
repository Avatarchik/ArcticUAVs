using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// ###############################################################
// JoystickUAVController is responsible for the control
// of the MTI UAV. It takes the direction from the 
// "MobileJoystickController" in the scene
// ###############################################################
public class JoystickUAVController : MonoBehaviour {

// ###############################################################
// Variables 
// ###############################################################
	public Joystick joystick;	
	public float speed = 5f;
	private Vector2 mazeMaxes;
	private Vector2 mazeMins;
	private int scale;
	private AudioSource sound;
	private bool playing = false;
	private PropellerController prop;

// ###############################################################
// Unity Functions 
// ###############################################################
	void Start () {
		// add sound to the UAV
		prop = GameObject.Find("propeller_1").GetComponent<PropellerController> ();
		sound = gameObject.AddComponent<AudioSource> ();
		AudioClip soundClip = (AudioClip)Resources.Load("Sounds/plane-propeller", typeof(AudioClip));
		sound.clip = soundClip;
		sound.volume = 0f;
		sound.loop = true;

		// Get the bounds of he maze for the UAV clamping
		MazeGenerator maze = GameObject.Find ("Maze Generator").GetComponent<MazeGenerator> ();
		scale = maze.scale;
		mazeMaxes = new Vector2 ((maze.height * scale) - (3 * scale), (maze.width * scale) + (3 * scale));
		mazeMins = new Vector2 (3 * scale, -5);
		if (GameObject.Find ("Max")) {
			GameObject.Find ("Max").transform.position = new Vector3 (mazeMaxes.x, 0f, mazeMaxes.y);
		}
		if (GameObject.Find ("Min")) {
			GameObject.Find ("Min").transform.position = new Vector3 (mazeMins.x, 0f, mazeMins.y);
		}
	}

	void Update () {
		// get the spin stats
		bool isSpinning = prop.isSpinning;
		float spinSpeed = prop.spinSpeed;
		if (!GameController.landed && GameController.inPlay) {
			float moveX = CrossPlatformInputManager.GetAxis ("Horizontal") * Time.deltaTime * speed;
			float moveY = CrossPlatformInputManager.GetAxis ("Vertical") * Time.deltaTime * speed;
			if (transform.position.x + moveX > mazeMaxes.x || transform.position.x + moveX <= mazeMins.x) moveX = 0;
			if (transform.position.z + moveY > mazeMaxes.y || transform.position.z + moveY <= mazeMins.y) moveY = 0;
			transform.Translate (moveX, moveY, 0f);
		}
		// scale the sound for how fast the props are spinning
		// if the prop is spinning and not playing
		if (isSpinning && !playing) {
			if (GameController.sfxStatus) sound.Play();
			playing = true;
		// if it is spinning and playing
		} else if (isSpinning) {
			// scale the volume based on spin speed
			sound.volume = spinSpeed * 0.15f;
		// the prop is not spinning
		} else {
			// pause the prop sound
			sound.Pause();
			playing = false;
		}
	}
}
