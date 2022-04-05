using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class ChangeTitle : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.UI.Text text;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        CustomizeLevelManager clm = CustomizeLevelManager.Instance;
        string content = "Words Learned";

        if (clm.tryingToCustomize == true)
        {
            content = "Customize Level";
        }
        text.text = content;
        VideoManager.resetVideoManager();
    }
}
