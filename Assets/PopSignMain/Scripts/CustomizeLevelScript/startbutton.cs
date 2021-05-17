using System;
using System.Collections;
using System.Collections.Generic;
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
        CustomizeLevelManager clm = CustomizeLevelManager.Instance;
        if (clm == null)
        {
            return;
        }

        int numOfWords = clm.selectedWord.Count;
        if (numOfWords < 3 || numOfWords > 5)
        {
            return;
        }

        LinkedList<TextAsset> listOfLevelsToPick = clm.levels[numOfWords];
        if (listOfLevelsToPick == null || listOfLevelsToPick.Count < 1)
        {
            return;
        }

        System.Random randomPicker = new System.Random();
        int randomUpperBound = listOfLevelsToPick.Count;
        int randomIndex = randomPicker.Next(randomUpperBound);
        TextAsset pickedLevel = null;
        LinkedList<TextAsset>.Enumerator enumerator = listOfLevelsToPick.GetEnumerator();
        for (int i = 1; i < randomIndex; i++)
        {
            enumerator.MoveNext();
        }

        pickedLevel = enumerator.Current;
        enumerator.Dispose();
        if (pickedLevel == null)
        {
            return;
        }

        //以下内容有可能出问题，可能会改动
        LevelData.loadLevelByTextAsset(pickedLevel);
        VideoManager.loadCustomizedData();
        //以上内容有可能出问题，可能会改动


    }
}
