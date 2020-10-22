using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using LitJson;

public class VideoManager {
	private static VideoManager sharedVideoManager;
	private ArrayList videoList;
	public string curtVideoName;
	public string reviewWord;
	public string folderName;
	public Video curtVideo;
	public int curtVideoIndex;
	public int frameCount;
	public bool shouldChangeVideo;

	private VideoManager()
	{
		videoList = new ArrayList ();
		shouldChangeVideo = true;
		curtVideoIndex = 0;
		curtVideo = null;
		if(SceneManager.GetActiveScene().name != "review")
		{
				ReadJsonFromTXT ();
		}
	}

	public static void resetVideoManager()
	{
		if (sharedVideoManager == null)
		{
			sharedVideoManager = new VideoManager ();
		}
		sharedVideoManager.resetVideoList ();
	}

	public static VideoManager getVideoManager()
	{
		if (sharedVideoManager == null)
		{
			sharedVideoManager = new VideoManager ();
		}
		return sharedVideoManager;
	}

	//POPSign Read Json file(The connection between colors and videos)
	void ReadJsonFromTXT()
	{
    int currentLevel = PlayerPrefs.GetInt("OpenLevel");

		TextAsset textReader = Resources.Load("VideoConnection/" + "level" + currentLevel ) as TextAsset;
		JsonData jd = JsonMapper.ToObject(textReader.text);

		foreach(BallColor color in Enum.GetValues(typeof(BallColor)))
		{
			if (color == BallColor.random)
			{
				break;
			}
			string fileName = jd[color + "fileName"] + "";
			folderName = jd[color + "folderName"] + "";
			string frameNumber = jd[color + "frameNumber"] + "";
			string imageName = jd [color + "ImageName"] + "";
			if (fileName != "" && folderName != "" && frameNumber != "" && imageName != "")
			{
				videoList.Add(new Video (int.Parse (frameNumber), fileName, folderName, imageName, color));
			}
			string wordsSeen = PlayerPrefs.GetString("WordsSeen", "");
			string[] words = wordsSeen.Split(',');
			if(!words.Contains(fileName))
			{
				PlayerPrefs.SetString("WordsSeen", wordsSeen + fileName + ",");
			}
		}
		curtVideo = (Video) videoList [0];
		curtVideoIndex = 0;
	}

	public void setReviewWord(string word)
	{
			reviewWord = word;
			Text currentWord = (Text) GameObject.Find("SelectedWord").GetComponent<Text>();
			currentWord.text = word;

			using (StreamReader r = new StreamReader("Assets/PopSignMain/Resources/words.json"))
	    {
	        string json = r.ReadToEnd();
					JsonData jd = JsonMapper.ToObject(json);
					JsonData wordData = jd[reviewWord];
					folderName = (string) wordData["folderName"];
					frameCount = (int) wordData["frameNumber"];
					curtVideo = new Video (frameCount, reviewWord, folderName);
					curtVideoIndex = 0;
					videoList.Add(curtVideo);
	    }
	}

	public void addVideoToVideoList(Video video)
	{
		videoList.Add (video);
	}

	public Video getVideoByVideoName(string videoName)
	{
		foreach (Video video in videoList)
		{
			if (video.fileName == videoName)
			{
				return video;
			}
		}
		return null;
	}

	public Video getVideoByColor(BallColor color)
	{
		return (Video) videoList[(int)color - 1];
	}

	public string getFolderName()
	{
		return folderName;
	}

	public string getReviewWord()
	{
		return reviewWord;
	}

	public void resetVideoList()
	{
		videoList.Clear ();
		ReadJsonFromTXT ();
	}

	public Video getCurtVideo()
	{
		return curtVideo;
	}

	public int getFrameCount()
	{
		return frameCount;
	}

	public void resetCurtVideo()
	{
		curtVideo = (Video) videoList [curtVideoIndex];
	}

	void Start ()
	{

	}

	public void ChangeReviewVideo(string reviewWord)
	{
		setReviewWord(reviewWord);
		shouldChangeVideo = true;
	}

}
