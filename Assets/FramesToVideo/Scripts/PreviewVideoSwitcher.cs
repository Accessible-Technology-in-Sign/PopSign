using UnityEngine; // 41 Post - Created by DimasTheDriver on Apr/20/2012 . Part of the 'Unity: Animated texture from image sequence' post series. Available at: http://www.41post.com/?p=4742
using System.Collections; //Script featured at Part 2 of the post series.
using UnityEngine.UI;
using UnityEngine.Video;

public class PreviewVideoSwitcher : MonoBehaviour
{
	//A texture object that will output the animation
	private Texture texture;

	//Gets the raw image
	private RawImage img;

	private VideoPlayer videoPlayer;

	//An integer to advance frames
	private int frameCounter = 0;

	//A string that holds the name of the folder which contains the image sequence
	public string folderName;
	//The name of the image sequence
	public string imageSequenceName;
	//The number of frames the animation has
	public int numberOfFrames;

	// Help text image to display
	public GameObject helpTextImageObject;

	// Help text background
	public GameObject helpTextObject;

	//The base name of the files of the sequence
	private string baseName;

	//Shared Video Manger
	private VideoManager sharedVideoManager;

	void Awake()
	{
		this.sharedVideoManager = VideoManager.getVideoManager ();
		//Get a reference to the Material of the game object this script is attached to
		this.img = (RawImage)this.GetComponent<RawImage>();
		this.videoPlayer = this.GetComponent<VideoPlayer>();
		//With the folder name and the sequence name, get the full path of the images (without the numbers)
		this.baseName = this.folderName + "/" + this.imageSequenceName;
	}

	void Start ()
	{
		//set the initial frame as the first texture. Load it from the first image on the folder
//		Debug.Log(sharedVideoManager.curtVideo.imageName);
		changePracticeScreenVideo ();
		if (baseName != "") {
			texture = (Texture)Resources.Load(baseName + "", typeof(Texture));

			// Popsign set initial word for help text
			// POPSign add image to video caption as help text
			if (helpTextImageObject) {
				SpriteRenderer helpTextImage = helpTextImageObject.GetComponent<SpriteRenderer> ();
				helpTextImage.sortingLayerName = "UI layer";
				helpTextImage.sortingOrder = 3;
				string textImageName = this.sharedVideoManager.curtVideo.imageName;
				helpTextImage.sprite = (Sprite)Resources.Load (textImageName, typeof(Sprite));
				// Consider the image size
				helpTextImage.transform.localScale = new Vector3 (0.5f, 0.5f, 0.0f);
				helpTextImage.transform.localPosition = new Vector3 (0f, 0f, 0f);

				//set background color the same as the ball
				SpriteRenderer helpTextBG = helpTextObject.GetComponent<SpriteRenderer> ();
				BallColor color = this.sharedVideoManager.curtVideo.color;
				string bgName = "VideoCaption/rect_" + color;
				helpTextBG.sprite = (Sprite)Resources.Load (bgName, typeof(Sprite));

				helpTextObject.SetActive (false);

			}
		}
	}

	void Update ()
	{
		this.sharedVideoManager = VideoManager.getVideoManager();
		if (sharedVideoManager.shouldChangeVideo) {
			//POPSign update the Video once the ball is shooted
			Video curtVideo = sharedVideoManager.getCurtVideo ();

			//POPSign reset all the variable to current video
			this.folderName = curtVideo.folderName;
			this.imageSequenceName = curtVideo.fileName;
			this.numberOfFrames = curtVideo.frameNumber;
			this.frameCounter = 0;

			this.baseName = this.folderName + "/" + this.imageSequenceName;
			if (this.baseName != "") {
				texture = (Texture)Resources.Load (baseName + "", typeof(Texture));

				// PopSign Update help text
				if (helpTextImageObject) {
					SpriteRenderer helpTextImage = helpTextImageObject.GetComponent<SpriteRenderer> ();

					if (helpTextImage == null) {
						helpTextImage = helpTextImageObject.AddComponent<SpriteRenderer> ();
						helpTextImage.sortingLayerName = "UI layer";
						helpTextImage.sortingOrder = 3;
					}

					string textImageName = this.sharedVideoManager.curtVideo.imageName;
					helpTextImage.sprite = (Sprite)Resources.Load (textImageName, typeof(Sprite));
					helpTextImage.transform.localScale = new Vector3 (0.5f, 0.5f, 0.0f);
					helpTextImage.transform.localPosition = new Vector3 (0f, 0f, 0f);

					SpriteRenderer helpTextBG = helpTextObject.GetComponent<SpriteRenderer> ();
					BallColor color = this.sharedVideoManager.curtVideo.color;
					string bgName = "VideoCaption/rect_" + color;
					helpTextBG.sprite = (Sprite)Resources.Load (bgName, typeof(Sprite));

					/*
					// Hide the hint when change the word
					helpTextObject.SetActive (false);
					*/
				}
			}

			sharedVideoManager.shouldChangeVideo = false;

			StartCoroutine(PlayUsingVideoPlayer());
		} 
		/*else {
			//Start the 'PlayLoop' method as a coroutine with a 0.04 delay
			StartCoroutine("PlayLoop", 0.04f);
			//Set the material's texture to the current value of the frameCounter variable
			if (this.texture != null) {
				img.texture = this.texture;
			}
		}*/
	}

	//The following methods return a IEnumerator so they can be yielded:
	//A method to play the animation in a loop
    IEnumerator PlayLoop(float delay)
    {
        //wait for the time defined at the delay parameter
        yield return new WaitForSeconds(delay);

		//advance one frame
		if (numberOfFrames != 0) {
			frameCounter = (++frameCounter)%numberOfFrames;
		}

		//load the current frame
		if (baseName != "") {
			this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString(), typeof(Texture));
		}

        //Stop this coroutine
        StopCoroutine("PlayLoop");
    }

	IEnumerator PlayUsingVideoPlayer()
	{
		yield return null;

#if UNITY_EDITOR
		videoPlayer.url = Application.dataPath + "/StreamingAssets/" + folderName +".mp4";
#elif UNITY_ANDROID
		videoPlayer.url = "jar:file://" + Application.dataPath + "!/assets/"+ folderName +".mp4";
#elif UNITY_IOS
		videoPlayer.url = Application.dataPath + "/Raw/" + folderName + ".mp4";
#endif
		Debug.Log(videoPlayer.url);
		videoPlayer.Prepare();
		videoPlayer.Play();
		Debug.Log(videoPlayer.isPlaying);
	}

	//A method to play the animation just once
    IEnumerator Play(float delay)
    {
        //wait for the time defined at the delay parameter
        yield return new WaitForSeconds(delay);

		//if it isn't the last frame
		if(frameCounter < numberOfFrames-1)
		{
			//Advance one frame
			++frameCounter;

			//load the current frame
			if (baseName != "") {
				this.texture = (Texture)Resources.Load(baseName + frameCounter.ToString(""), typeof(Texture));
			}
		}

        //Stop this coroutine
        StopCoroutine("Play");
    }

	public void changePracticeScreenVideo() {
		sharedVideoManager.resetCurtVideo ();
		sharedVideoManager.shouldChangeVideo = true;

	}

}
