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

		//For Random Levels (BK)
		int randomizeLevels = PlayerPrefs.GetInt("RandomizeLevel", 0);


		//This seems to fetc

		//For Random Levels (BK)
		TextAsset textReader;
		if(randomizeLevels == 0)
        {
			//Original code
			textReader = Resources.Load("VideoConnection/" + "level" + currentLevel) as TextAsset;
		}
        else
        {
			string path = Application.persistentDataPath + "/level" + currentLevel + ".txt";
			string levelText;
			using(StreamReader reader = new StreamReader(path))
            {
				levelText = reader.ReadToEnd();
            }

			textReader = new TextAsset(levelText);
		}
		
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

	public BallColor getBallColorFromVideoName(string videoName)
    {
		for (int i = 0; i <videoList.Count; i++)
		{
			if (((Video)videoList[i]).fileName == videoName)
			{
				return (BallColor)(i + 1);
			}
		}
		return 0;
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

    public static void loadCustomizedData()
    {
        if (sharedVideoManager == null)
        {
            resetVideoManager();
        }

        sharedVideoManager.videoList.Clear();

        //start prepare fake json file
        string fakeJsonTxt = "{\n";
        CustomizeLevelManager clm = CustomizeLevelManager.Instance;
        if (clm == null)
        {
            return;
        }

        HashSet<string> selectedWords = clm.selectedWord;
        int numOfWords = selectedWords.Count;
        if (numOfWords < 3 || numOfWords > 5)
        {
            return;
        }

        string allWords = (Resources.Load("words") as TextAsset).text;
        for (int i = 0; i < numOfWords; i++)
        {
            string aWord = selectedWords.ToArray<string>()[i];
            string color = "";
            switch(i)
            {
                case 0:
                    color = "blue";
                    break;
                case 1:
                    color = "green";
                    break;
                case 2:
                    color = "red";
                    break;
                case 3:
                    color = "violet";
                    break;
                case 4:
                    color = "yellow";
                    break;
            }

            string fileName = "\"" + color + "fileName\":\"" + aWord + "\",\n";
            string frameNumber = "\"" + color + "frameNumber\":";
            string folderName = "\"" + color + "folderName\":\"MacarthurBates/";
            string imageName = "\"" + color + "ImageName\":\"WordIcons/" + aWord + "\"" + (i + 1 == numOfWords? "\n}": ",\n");

            int searchStartIndex = allWords.IndexOf(aWord) + aWord.Length;
            string keySubstring = allWords.Substring(searchStartIndex);
            int targetStartIndex = keySubstring.IndexOf("\"folderName\":\"") + "\"folderName\":\"".Length + " \"folderName\": \"".Length;
            int targetEndIndex = keySubstring.IndexOf("\",", targetStartIndex);
            folderName = folderName + keySubstring.Substring(targetStartIndex, targetEndIndex - targetStartIndex) + "/" + aWord + "\",\n";

            searchStartIndex = targetEndIndex;
            keySubstring = keySubstring.Substring(searchStartIndex);
            targetStartIndex = keySubstring.IndexOf("\"frameNumber\":") + "\"frameNumber\":\"".Length;
            targetEndIndex = keySubstring.IndexOf("},", targetStartIndex);
            frameNumber = frameNumber + keySubstring.Substring(targetStartIndex, targetEndIndex - targetStartIndex - 5) + ",\n";

            fakeJsonTxt = fakeJsonTxt + fileName + frameNumber + folderName + imageName;
        }
        //end prepare fake json file

        JsonData jd = JsonMapper.ToObject(fakeJsonTxt);

        foreach (BallColor color in Enum.GetValues(typeof(BallColor)))
        {
            if (color == BallColor.random)
            {
                break;
            }
            string fileName = jd[color + "fileName"] + "";
            sharedVideoManager.folderName = jd[color + "folderName"] + "";
            string frameNumber = jd[color + "frameNumber"] + "";
            string imageName = jd[color + "ImageName"] + "";
            if (fileName != "" && sharedVideoManager.folderName != "" && frameNumber != "" && imageName != "")
            {
                sharedVideoManager.videoList.Add(new Video(int.Parse(frameNumber), fileName, sharedVideoManager.folderName, imageName, color));
            }
        }
        sharedVideoManager.curtVideo = (Video)sharedVideoManager.videoList[0];
        sharedVideoManager.curtVideoIndex = 0;

    }

}
