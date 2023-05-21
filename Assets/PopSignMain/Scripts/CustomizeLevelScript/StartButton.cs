using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (CustomizeLevelManager.Instance.tryingToCustomize) {
            this.gameObject.SetActive(true);
        } else {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Click start buttom
    public void startCustomizedLevel()
    {
        CustomizeLevelManager.startCustomizedLevel();





    }
}
