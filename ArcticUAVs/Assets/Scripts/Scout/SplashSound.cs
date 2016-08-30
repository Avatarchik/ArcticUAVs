using UnityEngine;
using System.Collections;


// ###############################################################
// SplashSound is responsible making the sound of the splash.
// When an object comes in contact with the water, The Splash
// GameObject is created with this script on the "Right" Splash.
// Because the splashes are created in pairs, The sound is only 
// set to the right splash so that there is one splash sound 
// per splash generated.
// ###############################################################
public class SplashSound : MonoBehaviour {

	private AudioSource sound;

	// Use this for initialization
	void Start () {
		sound = GetComponent<AudioSource>();
		if (GameController.sfxStatus) {
			// vary pitch of splash sounds for some variation
			sound.pitch = Random.Range(0.75f,1.5f);
			sound.Play();
		}
	}
}
