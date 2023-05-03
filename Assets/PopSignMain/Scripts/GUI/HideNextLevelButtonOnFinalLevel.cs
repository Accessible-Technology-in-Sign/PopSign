using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideNextLevelButtonOnFinalLevel : MonoBehaviour
{
    [SerializeField] private GameObject nextLevelButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("NumLevels") == PlayerPrefs.GetInt("OpenLevel"))
        {
            nextLevelButton.SetActive(false);
        }
    }
}
