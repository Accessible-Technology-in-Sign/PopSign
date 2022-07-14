using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameTutorial : MonoBehaviour
{
    public GameObject overlay1;
    public GameObject overlay2;
    int currentPage;

  	void Start ()
    {
        if(PlayerPrefs.GetInt("TutorialPlayed") == 0)
        {
          currentPage = 1;
            overlay1.SetActive(true);
          overlay2.SetActive(false);
        }
        else
        {
          gameObject.SetActive(false);
        }
  	}

    public void StartTutorial()
    {
        overlay1.SetActive(false);
        overlay2.SetActive(true);
        currentPage = 2;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (currentPage == 2)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }

    public void Skip()
    {
        gameObject.SetActive(false);
    }
}
