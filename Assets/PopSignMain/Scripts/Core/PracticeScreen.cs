using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PracticeScreen : MonoBehaviour {
	public Sprite enabledImage;
	public Sprite disabledImage;
	//POPSign Shared Video Manager
	private VideoManager sharedVideoManager;
	private GameObject nextButton;
	private GameObject backButton;
	private GameObject playButton;
	private GameObject playText;
	private bool[] viewed = new bool[5];
	private Color[] colorArray = new Color[] {
		new Color(0.73F , 0.51F, 0.92F, 1.0F), // blue
		new Color(0.55F , 0.77F, 0.447F, 1.0F), // green
		new Color(0.91F , 0.451F, 0.41F, 1.0F), // red
		new Color(0.95F , 0.667F, 0.725F, 1.0F), // violet
		new Color(1.0F , 0.80F, 0.02F, 1.0F)}; // yellow

	void Awake ()
	{

	}

	// Use this for initialization
	void Start ()
	{
		nextButton = GameObject.Find("Next");
		backButton = GameObject.Find("Back");
		playButton = GameObject.Find("Play");
		playText = GameObject.Find("PlayText");

		backButton.SetActive( false );
		playButton.GetComponent<Button>().interactable = false;

		sharedVideoManager = VideoManager.getVideoManager();
		sharedVideoManager.curtVideoIndex = 0;
		changePracticeScreenVideo ();
		viewed[0] = true;
	}

	// Update is called once per frame
	void Update ()
	{
		bool viewedAll = true;
		for (int i = 0; i < 5; i++)
		{
			if (viewed[i] == false)
			{
				viewedAll = false;
				break;
			}
		}
		if(viewedAll)
		{
			playButton.GetComponent<Button>().interactable = true;
			playButton.GetComponent<Image>().sprite = enabledImage;
			// update the color of the text to match the new button image
			playText.GetComponent<Text>().color = new Color(251, 253, 252, 240);
		}
	}

	public void Next()
	{
		updateCircles (true);
		if (sharedVideoManager.curtVideoIndex < 4)
		{
			sharedVideoManager.curtVideoIndex++;
		}
		if (sharedVideoManager.curtVideoIndex == 4)
		{
			nextButton.SetActive( false );
		}
		else
		{
			if(!backButton.activeSelf)
				backButton.SetActive(true);
		}
		changePracticeScreenVideo ();
		viewed[sharedVideoManager.curtVideoIndex] = true;
	}

	public void Back()
	{
		updateCircles (false);
		if (sharedVideoManager.curtVideoIndex > 0)
		{
			sharedVideoManager.curtVideoIndex--;
		}
		if (sharedVideoManager.curtVideoIndex == 0)
		{
			backButton.SetActive(false);
		}
		else
		{
			if(!nextButton.activeSelf)
				nextButton.SetActive(true);
		}
		changePracticeScreenVideo ();
		viewed[sharedVideoManager.curtVideoIndex] = true;
	}

	public void updateCircles(bool isNext)
	{
		int curtCircleId;
		if (isNext) {
			if (sharedVideoManager.curtVideoIndex < 4) {
				curtCircleId = sharedVideoManager.curtVideoIndex + 1;
			} else {
				curtCircleId = 0;
			}
		} else {
			if (sharedVideoManager.curtVideoIndex > 0) {
				curtCircleId = sharedVideoManager.curtVideoIndex - 1;
			} else {
				curtCircleId = 4;
			}
		}

		RawImage curtCircle = (RawImage) GameObject.Find("Circle" + curtCircleId).GetComponent<RawImage>();
		RawImage prevCircle = (RawImage) GameObject.Find ("Circle" + sharedVideoManager.curtVideoIndex).GetComponent<RawImage>();

		Texture unfilledTexture = prevCircle.texture;
		Texture filledTexture = curtCircle.texture;
		curtCircle.texture = unfilledTexture;
		prevCircle.texture = filledTexture;
	}

	public void changePracticeScreenVideo()
	{
		sharedVideoManager = VideoManager.getVideoManager();
		sharedVideoManager.resetCurtVideo ();
		sharedVideoManager.shouldChangeVideo = true;

		Text currentWord = (Text) GameObject.Find("CurrentWord").GetComponent<Text>();
		currentWord.text = sharedVideoManager.curtVideo.fileName;
	}
}
