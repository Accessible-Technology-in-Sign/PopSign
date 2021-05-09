using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeLevelManager : MonoBehaviour
{
    static public CustomizeLevelManager Instance;
    public bool tryingToCustomize = false;
    public HashSet<string> selectedWord = new HashSet<string>();
    public Dictionary<int, LinkedList<int>> levels = new Dictionary<int, LinkedList<int>>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            // filter level files and population levels variable
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
}
