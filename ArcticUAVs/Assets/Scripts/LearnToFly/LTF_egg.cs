using UnityEngine;
using System.Collections;
public class LTF_egg : MonoBehaviour {
	private Transform uav;
	void Start () {uav = GameObject.Find("uav").transform;}
	void Update () { if(Time.timeSinceLevelLoad > 30) {
						uav.Rotate(0f,0f,1f);}}}