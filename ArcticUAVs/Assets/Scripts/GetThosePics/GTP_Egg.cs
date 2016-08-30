using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class GTP_Egg : MonoBehaviour {
	private GameObject manUp;
	private GameObject manDown;
	private AudioSource manYell;
	private bool setactive;
	private bool handsUP = false;
	void Start () { manDown = GameObject.Find("manHandsDown").gameObject;
					manUp = GameObject.Find("Canvas").transform.Find("manHandsUp").gameObject;
					manYell = manUp.GetComponent<AudioSource>();
					setactive = false;}
	void Update () {if (!handsUP && Time.time % 30 > 29 && !setactive) {
						manDown.SetActive(false);
						manUp.SetActive(true);
						manYell.Play();
						handsUP = true;
						setactive = true;}
					if (handsUP && Time.time % 30 < 1.5 && setactive) {
						manDown.SetActive(true);
						manUp.SetActive(false);
						handsUP = false;
						setactive = false;}}}