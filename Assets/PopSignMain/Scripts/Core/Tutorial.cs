using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public GameObject nextButton;
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject leftie;
    public GameObject rightie;
    int currentPage;

  	void Start ()
    {
        currentPage = 1;
  	}

    public void Next()
    {
      switch(currentPage)
      {
        case 1:
          page1.SetActive(false);
          page2.SetActive(true);
          nextButton.SetActive(false);
          currentPage += 1;
          break;
        case 2:
          page2.SetActive(false);
          page3.SetActive(true);
          nextButton.SetActive(true);
          currentPage += 1;
          if(PlayerPrefs.GetString("Handedness") == "Left")
          {
            rightie.SetActive(false);
            leftie.SetActive(true);
          }
          else
          {
            leftie.SetActive(false);
            rightie.SetActive(true);
          }
          break;
        case 3:
          SceneManager.LoadScene("map");
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
          page2.SetActive(false);
          page1.SetActive(true);
          nextButton.SetActive(true);
          currentPage -= 1;
          break;
        case 3:
          page3.SetActive(false);
          page2.SetActive(true);
          nextButton.SetActive(false);
          currentPage -= 1;
          break;
        default:
          break;
      }
    }

    public void LeftButtonPress()
    {
      PlayerPrefs.SetString("Handedness", "Left");
      Next();
    }

    public void RightButtonPress()
    {
      PlayerPrefs.SetString("Handedness", "Right");
      Next();
    }
}
