using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateSetter : MonoBehaviour
{
    public bool target30FPS = false;

    // Start is called before the first frame update
    void Start()
    {
        if (target30FPS)
        {
            Application.targetFrameRate = 30;
        }
        else
        {
            Application.targetFrameRate = 15;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
