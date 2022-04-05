using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class CustomizeLevelManager : MonoBehaviour
{
    static public CustomizeLevelManager Instance;
    public bool tryingToCustomize = false;
    public HashSet<string> selectedWord = new HashSet<string>();
    public Dictionary<int, LinkedList<TextAsset>> levels = new Dictionary<int, LinkedList<TextAsset>>();

    void readlevel() {
        UnityEngine.Object[] level = Resources.LoadAll("Levels");
        foreach (UnityEngine.Object temp in level) {
            TextAsset textAsset = (TextAsset)temp;
            int index = textAsset.text.IndexOf("COLOR LIMIT");
            string numstr = textAsset.text.Substring(index + 12, 1);
            int numlevel = Int32.Parse(numstr);
            if (levels.ContainsKey(numlevel)) {
                levels[numlevel].AddLast(textAsset);
            } else {
                LinkedList<TextAsset> newlist = new LinkedList<TextAsset>();
                newlist.AddLast(textAsset);
                levels.Add(numlevel, newlist);
            }
        }

    }

    public static void reset()
    {
        Instance.selectedWord = new HashSet<string>();
    }

    public static void switchOff()
    {
        if (Instance != null)
        {
            Instance.tryingToCustomize = false;
            Instance.selectedWord = new HashSet<string>();
        }
    }
        
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            // filter level files and population levels variable
            readlevel();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void startCustomizedLevel()
    {
        if (Instance == null || !Instance.tryingToCustomize)
        {
            return;
        }
        int numOfWords = Instance.selectedWord.Count;
        if (numOfWords < 3 || numOfWords > 5)
        {
            return;
        }

        Debug.Log(numOfWords);

        LinkedList<TextAsset> listOfLevelsToPick = Instance.levels[numOfWords];
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
        if (pickedLevel == null)
        {
            return;
        }
        InitScriptName.InitScript.Instance.currentTarget = LevelData.loadLevelByTextAsset(pickedLevel);
        SceneManager.LoadScene("game");
        VideoManager.loadCustomizedData();
    }
}
