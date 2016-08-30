using UnityEngine;
using System.Collections;

public class Terrain2DCapController : MonoBehaviour {

	public string sortingLayerName;
	public int sortingOrder;

	// Use this for initialization
	void Start () {
		GetComponent<MeshRenderer> ().sortingLayerName = sortingLayerName;
		GetComponent<MeshRenderer> ().sortingOrder = sortingOrder;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
