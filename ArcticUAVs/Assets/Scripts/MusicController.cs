
using UnityEngine;
using System.Collections;

// ###############################################################
// MusicController is responsable for playing the music in each
// of the missions if the music setting is on
// ###############################################################
public class MusicController : MonoBehaviour {
// ###############################################################
// Variables 
// ###############################################################

	private AudioClip track1;
	private AudioClip track2;
	private AudioClip track3;
	private AudioClip track4;
	private AudioSource sound;

// ###############################################################
// Unity Functions 
// ###############################################################
	// Use this for initialization
	void Start () {
		track1 = Resources.Load<AudioClip>("Music/track1");
		track2 = Resources.Load<AudioClip>("Music/track2");
		track3 = Resources.Load<AudioClip>("Music/track3");
		track4 = Resources.Load<AudioClip>("Music/track4");

		sound = gameObject.AddComponent<AudioSource> ();
		// Learn to Fly - Track 3
		// Land in the Hand - Track 4 
		// Get those Pics - Track 2 
		// Map That Ice - Track 1
		// Decide music track based on mission initials
		switch (GameController.missionInitials) 
		{
			case "LTF":
				sound.clip = track3;
				break;
			case "LITH":
				sound.clip = track4;
				break;
			case "GTP":
				sound.clip = track2;
				break;
			case "MTI":
				sound.clip = track1;
				break;
			default:
				sound.clip = track1;
				break;
		}
		sound.volume = .5f;
		sound.loop = true;
		if (GameController.musicStatus) {
			sound.Play();
		}
	}
}