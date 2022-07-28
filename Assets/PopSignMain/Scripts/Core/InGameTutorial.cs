using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameTutorial : MonoBehaviour
{
    public GameObject practice;
    public GameObject tap;
    public GameObject drag;
    public GameObject bounce;
    public GameObject answer;
    public GameObject newSign;
    int currentPage;

  	void Start ()
    {
        if(PlayerPrefs.GetInt("TutorialPlayed") == 0)
        {
          currentPage = 1;
            practice.SetActive(true);
            tap.SetActive(false);
            drag.SetActive(false);
            bounce.SetActive(false);
            answer.SetActive(false);
            newSign.SetActive(false);
        }
        else
        {
          gameObject.SetActive(false);
        }
  	}

    public void StartTutorial()
    {
        practice.SetActive(false);
        tap.SetActive(true);
        currentPage += 1;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            switch (currentPage)
            {
                case 2:
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        tap.SetActive(false);
                        drag.SetActive(true);
                        currentPage += 1;
                    }
                    break;
                case 3:
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        drag.SetActive(false);
                        bounce.SetActive(true);
                        currentPage += 1;
                    }
                    break;
                case 4:
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        bounce.SetActive(false);
                        answer.SetActive(true);
                        currentPage += 1;
                    }
                    break;
                case 5:
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        answer.SetActive(false);
                        newSign.SetActive(true);
                        currentPage += 1;
                    }
                    break;
                case 6:
                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }
    }

    public void Skip()
    {
        gameObject.SetActive(false);
    }
}
