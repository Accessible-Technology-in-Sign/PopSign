using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

public enum Target
{
    Top = 0,
    Star
}

namespace InitScriptName
{
    public class InitScript : MonoBehaviour
    {
        public static InitScript Instance;
        private bool _isShow;

        public static int lastPlace;
        public static bool beaten;
        public static int messCount;

        public static DateTime today;
        public static DateTime DateOfRestLife;
        public static string timeForReps;
        public Target currentTarget;

        public void Awake()
        {
            Instance = this;

            if(!PlayerPrefs.HasKey("Launched"))
            {
                PlayerPrefs.SetInt("Launched", 1);
                PlayerPrefs.SetInt("Music", 1);
                PlayerPrefs.SetInt("Sound", 1);
                PlayerPrefs.SetInt("MaxLevel", 1);
                PlayerPrefs.SetInt("OpenLevel", 0);
                PlayerPrefs.SetInt("TutorialPlayed", 0);
                PlayerPrefs.SetInt("ReviewModalShown", 0);
                PlayerPrefs.SetString("WordsSeen", "");
                PlayerPrefs.Save();
            }

            GameObject.Find("Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Music");
            SoundBase.Instance.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Sound");
        }

        void Start()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                Application.targetFrameRate = 60;
            }
            GameObject.Find("Music").GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Music");
            SoundBase.Instance.GetComponent<AudioSource>().volume = PlayerPrefs.GetInt("Sound");
        }

        #region selectlevel
        public int LoadLevelStarsCount(int level)
        {
            return level > 10 ? 0 : (level % 3 + 1);
        }

        public void SaveLevelStarsCount(int level, int starsCount)
        {
            Debug.Log(string.Format("Stars count {0} of level {1} saved.", starsCount, level));
        }

        public void ClearLevelProgress(int level)
        {

        }

        public void OnLevelClicked(int number)
        {
            currentTarget = LevelData.GetTarget(number);
            PlayerPrefs.SetInt("OpenLevel", number);
            PlayerPrefs.Save();
            VideoManager.resetVideoManager();
            SceneManager.LoadScene("practice");
        }

        void OnApplicationPause(bool pauseStatus)
        {

        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

        }


        #endregion

    }


}
