using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour {


private GameObject Canvas;
private GameObject pauseBackground;
private GameObject quitButton;
private GameObject retryButton;
private GameObject resumeButton;
private GameObject watchButton;
private GameObject pauseTitle;

// private SpriteRenderer renderer;

	// Use this for initialization
	void Start () {

		
		// Canvas = GameObject.Find("Canvas");

		// pauseBackground = new GameObject();
		// pauseBackground.name = "pauseMenu_bg";
		// pauseBackground.transform.parent = Canvas.transform;
		// pauseBackground.AddComponent<Image>();
		// Image bgImage = pauseBackground.GetComponent<Image>();
		// bgImage.sprite = Resources.Load("Sprites/PanelBackground", typeof(Sprite)) as Sprite;
		// bgImage.color = new Vector4 (1f,1f,1f,167f/255f);
		// 	pauseBackground.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		// 	pauseBackground.GetComponent<RectTransform>().anchorMin = new Vector2 (0.3f,0.2f);
		// 	pauseBackground.GetComponent<RectTransform>().anchorMax = new Vector2 (0.7f,0.8f);
		// 	pauseBackground.GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		// 	pauseBackground.GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);


		// quitButton = new GameObject();
		// quitButton.name = "quitButton";
		// quitButton.transform.parent = pauseBackground.transform;
		// quitButton.AddComponent<Image>();
		// bgImage = quitButton.GetComponent<Image>();
		// bgImage.sprite = Resources.Load("Sprites/quit_icon", typeof(Sprite)) as Sprite;
		// bgImage.color = new Vector4 (1f,1f,1f,167f/255f);
		// 	quitButton.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		// 	quitButton.GetComponent<RectTransform>().anchorMin = new Vector2 (0.05f,0.05f);
		// 	quitButton.GetComponent<RectTransform>().anchorMax = new Vector2 (0.32f,.4f);
		// 	quitButton.GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		// 	quitButton.GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);


		// retryButton = new GameObject();
		// retryButton.name = "retryButton";
		// retryButton.transform.parent = pauseBackground.transform;
		// retryButton.AddComponent<Image>();
		// bgImage = retryButton.GetComponent<Image>();
		// bgImage.sprite = Resources.Load("Sprites/retry_icon", typeof(Sprite)) as Sprite;
		// bgImage.color = new Vector4 (1f,1f,1f,167f/255f);
		// 	retryButton.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		// 	retryButton.GetComponent<RectTransform>().anchorMin = new Vector2 (0.34f,0.05f);
		// 	retryButton.GetComponent<RectTransform>().anchorMax = new Vector2 (0.65f,.4f);
		// 	retryButton.GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		// 	retryButton.GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);

		// resumeButton = new GameObject();
		// resumeButton.name = "resumeButton";
		// resumeButton.transform.parent = pauseBackground.transform;
		// resumeButton.AddComponent<Image>();
		// bgImage = resumeButton.GetComponent<Image>();
		// bgImage.sprite = Resources.Load("Sprites/resume_icon", typeof(Sprite)) as Sprite;
		// bgImage.color = new Vector4 (1f,1f,1f,167f/255f);
		// 	resumeButton.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		// 	resumeButton.GetComponent<RectTransform>().anchorMin = new Vector2 (0.67f,0.05f);
		// 	resumeButton.GetComponent<RectTransform>().anchorMax = new Vector2 (0.95f,.4f);
		// 	resumeButton.GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		// 	resumeButton.GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);


		// watchButton = new GameObject();
		// watchButton.name = "watchButton";
		// watchButton.transform.parent = pauseBackground.transform;
		// watchButton.AddComponent<Image>();
		// bgImage = watchButton.GetComponent<Image>();
		// bgImage.sprite = Resources.Load<Sprite>("Sprites/watch_icon");
		// bgImage.color = new Vector4 (1f,1f,1f,167f/255f);
		// 	watchButton.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		// 	watchButton.GetComponent<RectTransform>().anchorMin = new Vector2 (0.05f,0.5f);
		// 	watchButton.GetComponent<RectTransform>().anchorMax = new Vector2 (0.32f,0.8f);
		// 	watchButton.GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		// 	watchButton.GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);


		// // pauseTitle = new GameObject();
		// pauseTitle = GameObject.Find ("Circles Needed");
		// pauseTitle.name = "pauseTitle";
		// pauseTitle.transform.parent = pauseBackground.transform;
		// // pauseTitle.AddComponent<Text>();


		// pauseTitle.GetComponent<Text>().text = "pause";

		// pauseTitle.GetComponent<Text>().font = GameObject.Find ("Circles Needed").GetComponent<Text>().font;
		// 	pauseTitle.GetComponent<RectTransform>().localScale = new Vector2 (1f,1f);
		// 	pauseTitle.GetComponent<RectTransform>().anchorMin = new Vector2 (0.05f,0.85f);
		// 	pauseTitle.GetComponent<RectTransform>().anchorMax = new Vector2 (.95f,0.95f);
		// 	pauseTitle.GetComponent<RectTransform>().offsetMax = new Vector2 (0f,0f);
		// 	pauseTitle.GetComponent<RectTransform>().offsetMin = new Vector2 (0f,0f);

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
