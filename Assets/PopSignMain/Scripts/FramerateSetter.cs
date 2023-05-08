using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateSetter : MonoBehaviour
{
    public int targetFPS = 30;

    // Start is called before the first frame update
    void Start()
{
        Application.targetFrameRate = targetFPS;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
