using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LitJson;
using System.Text;
using System.IO;

//Make clearer
public class EnableRandomLevels : MonoBehaviour
{
	public GameObject checkmark;
	private int randomizeLevels;

	//Will need to be changed when number of levels increases
	private int numLevels = 548;

	public void Start()
	{
		randomizeLevels = PlayerPrefs.GetInt("RandomizeLevel", 0);
		if(randomizeLevels == 0)
        {
			checkmark.SetActive(false);
        }
        else
        {
			checkmark.SetActive(true);
        }
	}


	public void ButtonClick()
	{
	}

	public void ToggleSelection()
	{
		if(randomizeLevels == 0)
        {
			randomizeLevels = 1;
			checkmark.SetActive(true);
			GenerateRandomLevel();
        }
		else
        {
			randomizeLevels = 0;
			checkmark.SetActive(false);
        }
		PlayerPrefs.SetInt("RandomizeLevel", randomizeLevels);
	}

	void GenerateRandomLevel()
	{
		TextAsset wordsTextAsset = Resources.Load("WordBanks/AllWords") as TextAsset;
		string[] wordList = wordsTextAsset.text.Split('\n');

		TextAsset textReader = Resources.Load("words") as TextAsset;
		JsonData jd = JsonMapper.ToObject(textReader.text);

		// Knuth shuffle algorithmn to randomize words in list
		for(int i = 0; i < wordList.Length; i++)
		{
			string temp = wordList[i];
			int randomNumber = Random.Range(0, wordList.Length);
			wordList[i] = wordList[randomNumber];
			wordList[randomNumber] = temp;
		}


		for(int level = 1; level <= numLevels; level++)
		{
			
			string[] selectedWords = new string[5];
			
			for (int i = 0; i < selectedWords.Length; i++)
			{
				int randomNumber = Random.Range(0, wordList.Length);
				string wordSelected = wordList[randomNumber].Trim();
				for (int j = 0; j < i; j++)
				{
					if (selectedWords[j].Equals(wordSelected))
					{
						randomNumber = Random.Range(0, wordList.Length);
						wordSelected = wordList[randomNumber].Trim();
						j = -1;
					}
				}
				selectedWords[i] = wordSelected;
			}
			
			
			/* 
			for(int i = 0; i < selectedWords.Length; i++)
			{
				int wordIndex = (((level - 1) / 5)*5) + i;
				if(wordIndex >= wordList.Length)
				{
					wordIndex = wordIndex - wordList.Length;
				}
				selectedWords[i] = wordList[wordIndex].Trim();
			}
			*/
			StringBuilder sb = new StringBuilder();
			JsonWriter writer = new JsonWriter(sb);

			writer.WriteObjectStart();

			// blue
			writer.WritePropertyName("bluefileName");
			writer.Write(selectedWords[0]);
			writer.WritePropertyName("blueframeNumber");
			writer.Write((int)jd[selectedWords[0]]["frameNumber"]);
			writer.WritePropertyName("bluefolderName");
			writer.Write("MacarthurBates/AllWords/" + jd[selectedWords[0]]["folderName"] + "/" + selectedWords[0]);
			writer.WritePropertyName("blueImageName");
			writer.Write("WordIcons/" + selectedWords[0]);

			// green
			writer.WritePropertyName("greenfileName");
			writer.Write(selectedWords[1]);
			writer.WritePropertyName("greenframeNumber");
			writer.Write((int)jd[selectedWords[1]]["frameNumber"]);
			writer.WritePropertyName("greenfolderName");
			writer.Write("MacarthurBates/AllWords/" + jd[selectedWords[1]]["folderName"] + "/" + selectedWords[1]);
			writer.WritePropertyName("greenImageName");
			writer.Write("WordIcons/" + selectedWords[1]);

			// red
			writer.WritePropertyName("redfileName");
			writer.Write(selectedWords[2]);
			writer.WritePropertyName("redframeNumber");
			writer.Write((int)jd[selectedWords[2]]["frameNumber"]);
			writer.WritePropertyName("redfolderName");
			writer.Write("MacarthurBates/AllWords/" + jd[selectedWords[2]]["folderName"] + "/" + selectedWords[2]);
			writer.WritePropertyName("redImageName");
			writer.Write("WordIcons/" + selectedWords[2]);

			// violet
			writer.WritePropertyName("violetfileName");
			writer.Write(selectedWords[3]);
			writer.WritePropertyName("violetframeNumber");
			writer.Write((int)jd[selectedWords[3]]["frameNumber"]);
			writer.WritePropertyName("violetfolderName");
			writer.Write("MacarthurBates/AllWords/" + jd[selectedWords[3]]["folderName"] + "/" + selectedWords[3]);
			writer.WritePropertyName("violetImageName");
			writer.Write("WordIcons/" + selectedWords[3]);

			// yellow
			writer.WritePropertyName("yellowfileName");
			writer.Write(selectedWords[4]);
			writer.WritePropertyName("yellowframeNumber");
			writer.Write((int)jd[selectedWords[4]]["frameNumber"]);
			writer.WritePropertyName("yellowfolderName");
			writer.Write("MacarthurBates/AllWords/" + jd[selectedWords[4]]["folderName"] + "/" + selectedWords[4]);
			writer.WritePropertyName("yellowImageName");
			writer.Write("WordIcons/" + selectedWords[4]);

			writer.WriteObjectEnd();

			string path = Application.persistentDataPath + "/level" + level + ".txt";
			StreamWriter sWriter = new StreamWriter(path, false);
			sWriter.Write(sb.ToString());
			sWriter.Close();
		}
	}
}
