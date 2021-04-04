using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CustomizeLevelButtonHandler : MonoBehaviour
{
    public void handleButtonPressed ()
    {
        SceneManager.LoadScene("wordlist");
    }
}
