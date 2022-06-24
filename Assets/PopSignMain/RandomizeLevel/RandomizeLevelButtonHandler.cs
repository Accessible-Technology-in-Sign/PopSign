using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RandomizeLevelButtonHandler : MonoBehaviour
{
    public void handleButtonPressed()
    {
        SceneManager.LoadScene("randomize");
    }
}
