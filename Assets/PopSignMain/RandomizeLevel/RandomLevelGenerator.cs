using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

//Make clearer
public class RandomLevelGenerator : MonoBehaviour
{
    private int randomizeLevels;
    // Start is called before the first frame update
    void Start()
    {
        randomizeLevels = PlayerPrefs.GetInt("RandomizeLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
