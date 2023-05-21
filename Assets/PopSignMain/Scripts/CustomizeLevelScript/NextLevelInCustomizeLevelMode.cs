using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelInCustomizeLevelMode : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (CustomizeLevelManager.Instance == null || !CustomizeLevelManager.Instance.tryingToCustomize)
        {
            text.text = "Next Level";
        }
        else
        {
            text.text = "Recreate";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
