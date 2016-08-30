using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// ###############################################################
// SC: Settings
//
// Scene Controllers handle anything in the scene that the generators and UAV can't
//
// Settings handles SFX and music
// ###############################################################
public class SettingsController : MonoBehaviour {
// ###############################################################
// Unity Functions
// ###############################################################
	void Start () {
		// connect to the toggles
		Toggle musicButton = GameObject.Find ("Music Toggle").GetComponent<Toggle> ();
		Toggle sfxButton = GameObject.Find ("SoundEffects Toggle").GetComponent<Toggle> ();
		// if our status is false, the button should be deselected
			if (!GameController.musicStatus) musicButton.isOn = false;
		// if our status is false, the button should be deselected
			if (!GameController.sfxStatus) sfxButton.isOn = false;
		// listeners for button changes
		musicButton.onValueChanged.AddListener (muteMusic);
		sfxButton.onValueChanged.AddListener (muteSoundEffects);}
// ###############################################################
// Setting Functions
// ###############################################################
	public void muteMusic (bool newValue ) {
		// [FUTURE WORK: Delete music object, or at least stop it from playing]
		GameController.musicStatus = newValue;
		PlayerPrefs.SetInt (("music pref") , GameController.musicStatus ? 1 : 0);}

	public void muteSoundEffects (bool newValue ) {
		GameController.sfxStatus = newValue;
		PlayerPrefs.SetInt (("sfx pref") , GameController.sfxStatus ? 1 : 0);}
}