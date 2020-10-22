using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameTutorial : MonoBehaviour
{
    public GameObject overlay1;
    public GameObject overlay2;
    public GameObject overlay3;
    int currentPage;

  	void Start ()
    {
        if(PlayerPrefs.GetInt("TutorialPlayed") == 0)
        {
          currentPage = 1;
          overlay2.SetActive(false);
          overlay3.SetActive(false);
        }
        else
        {
          gameObject.SetActive(false);
        }
  	}

    public void Next()
    {
      switch(currentPage)
      {
        case 1:
          overlay1.SetActive(false);
          overlay2.SetActive(true);
          currentPage += 1;
          break;
        case 2:
          overlay2.SetActive(false);
          overlay3.SetActive(true);
          currentPage += 1;
          break;
        case 3:
          gameObject.SetActive(false);
          break;
        default:
          break;
      }
    }

    public void Back()
    {
      switch(currentPage)
      {
        case 1:
          SceneManager.LoadScene("map");
          break;
        case 2:
          overlay2.SetActive(false);
          overlay1.SetActive(true);
          currentPage -= 1;
          break;
        case 3:
          overlay3.SetActive(false);
          overlay2.SetActive(true);
          currentPage -= 1;
          break;
        default:
          break;
      }
    }
}
