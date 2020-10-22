using System.Collections;
using UnityEngine;

public class ObbLoader : MonoBehaviour {
  
  #if UNITY_ANDROID && !UNITY_EDITOR
    void OnGUI()
    {
      if (!GooglePlayDownloader.RunningOnAndroid())
      {
        return;
      }
      else if (PlayerPrefs.GetInt ("obbLoaded", 0) == 0)
      {
        string expPath = GooglePlayDownloader.GetExpansionFilePath();
        if (expPath == null)
        {
            GUI.Label(new Rect(10, 10, Screen.width-10, 20), "External storage is not available!");
        }
        else
        {
          string mainPath = GooglePlayDownloader.GetMainOBBPath(expPath);
          if (mainPath == null)
            GooglePlayDownloader.FetchOBB();
          PlayerPrefs.SetInt ("obbLoaded", 1);
          PlayerPrefs.Save ();
        }
      }
    }
  #endif
}
