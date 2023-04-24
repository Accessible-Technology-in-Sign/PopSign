using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 8;
        Application.targetFrameRate = 15;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
