using UnityEngine;
using System.Collections;
public class LITH_egg : MonoBehaviour {
	private bool birdSpawned;
	private bool otterSpawned;
	private BirdGenerator birdGenerator;
	private OtterController otterController;
	void Start () { birdGenerator = GameObject.Find("egg2").GetComponent<BirdGenerator>();
					birdGenerator.automaticBirdSetting(false);
					GameController.inPlay = true;
					otterSpawned = false;}
	void Update () {if (Time.time % 20 > 19 && !birdSpawned) {
						birdGenerator.makeBird("right");
						birdSpawned = true;}
					if (Time.time % 20 < 1 && birdSpawned) {
						birdSpawned = false;}
					if (Time.timeSinceLevelLoad > 30 && !otterSpawned) {
						OtterGenerator.createOtter(new Vector3(1.9f,-4f));
						Destroy(GameObject.Find("Region").gameObject);
						otterSpawned = true;}}}