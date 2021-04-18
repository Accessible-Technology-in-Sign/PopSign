using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;


public class ScrollView : MonoBehaviour {
		VideoManager sharedVideoManager;
		ReviewVideo reviewManager;
		GameObject practice;
		GameObject wordListExplanation;
		GameObject selectedWord;
		GameObject backButton;
		GameObject closeButton;
		GameObject scrollView;
		GameObject search;
		GameObject videoGroup;
		public GameObject ButtonTemplate;
		private List<string> WordList;
		private List<string> SelectedWords;
		private string currentWord;

		void Start ()
		{
				sharedVideoManager = VideoManager.getVideoManager();
				WordList = new List<string>();
				SelectedWords = new List<string>();

				practice = GameObject.Find ("PracticeExplanations");
				practice.SetActive( false );
				selectedWord = GameObject.Find ("SelectedWord");
				selectedWord.SetActive( false );
				backButton = GameObject.Find ("BackButton");
				backButton.SetActive( false );
				videoGroup = GameObject.Find ("VideoGroup");
				videoGroup.SetActive( false );

				wordListExplanation = GameObject.Find ("WordListExplanation");
				closeButton = GameObject.Find ("Close");
				scrollView = GameObject.Find ("ScrollView");
				search = GameObject.Find ("Search");

				LoadWords();

				// this gets incremented so that the buttons appear in a vertical list. if this were a static number instead,
				// all of the buttons would draw on top of each other.
				int yPos = -40;

				foreach(string str in WordList)
				{
						if(str != "")
						{
							GameObject go = Instantiate(ButtonTemplate) as GameObject;
							go.SetActive(true);
							go.name = str;
							go.tag = "cloned";
							ScrollButton sb = go.GetComponent<ScrollButton>();
							sb.SetName(str);

                            if (CustomizeLevelManager.Instance.tryingToCustomize)
                            {
                                sb.ButtonText.transform.localPosition = new Vector3(77, 0, 0);
                                sb.CheckBox.SetActive(true);
                            }
                            go.transform.SetParent(ButtonTemplate.transform.parent);
							go.transform.localScale = new Vector3(1, 1, 1);
							go.transform.localPosition = new Vector3(250, yPos, 0);
							yPos -= 100;
						}
				}
		}

		void LoadWords ()
    {
				string wordsSeen = PlayerPrefs.GetString("WordsSeen");
				string[] words = wordsSeen.Split(',');
				WordList.AddRange(words);
				WordList.Sort();
    }

		public void ButtonClicked(string str)
		{
        scrollView.SetActive( false );
        closeButton.SetActive( false );
        wordListExplanation.SetActive( false );
				search.SetActive( false );

				practice.SetActive( true );
				selectedWord.SetActive( true );
				backButton.SetActive( true );
				videoGroup.SetActive( true );

				sharedVideoManager = VideoManager.getVideoManager();
				sharedVideoManager.ChangeReviewVideo(str);
				currentWord = str;
		}

		public void Search(InputField _input)
		{

			string query = _input.text;
			Debug.Log(query);

			ResetSearch();

			if(query == null || query == "")
			{
				return;
			}

			foreach(string str in WordList)
			{
					if(str != "" && !str.Contains(query))
					{
						GameObject go = GameObject.Find(str);
						if(go != null)
							go.SetActive(false);
					}
					else
					{
						GameObject go = GameObject.Find(str);
						if(go != null)
							go.SetActive(true);
					}
			}
		}

		public void ResetSearch()
		{
			foreach(Button sb in Resources.FindObjectsOfTypeAll<Button>())
			{
					if(sb != null && sb.gameObject.tag == "cloned")
						sb.gameObject.SetActive(true);
			}
		}

		public void BackToWordList()
		{
				practice.SetActive( false );
				selectedWord.SetActive( false );
				backButton.SetActive( false );
        videoGroup.SetActive( false );

        scrollView.SetActive( true );
        closeButton.SetActive( true );
        wordListExplanation.SetActive( true );
        search.SetActive( true );
		}

		public void NextWord()
		{
			int currentIndex = WordList.IndexOf(currentWord);
			if(currentIndex != WordList.Count - 1)
			{
				currentWord = WordList[currentIndex + 1];
				sharedVideoManager = VideoManager.getVideoManager();
				sharedVideoManager.ChangeReviewVideo(currentWord);
			}
		}

		public void PreviousWord()
		{
			int currentIndex = WordList.IndexOf(currentWord);
			if (currentIndex != 1)
			{
				currentWord = WordList[currentIndex - 1];
				sharedVideoManager = VideoManager.getVideoManager();
				sharedVideoManager.ChangeReviewVideo(currentWord);
			}
		}

		public void UpdateSelection(string word, bool selected)
		{
            if (CustomizeLevelManager.Instance == null)
            {
                return;
            }
            HashSet<string> set = CustomizeLevelManager.Instance.selectedWord;
			if (selected) {
				set.Add(word);
			} else {
				set.Remove(word);
			}
		}

		void CreateCustomLevel()
    {

    }
}
