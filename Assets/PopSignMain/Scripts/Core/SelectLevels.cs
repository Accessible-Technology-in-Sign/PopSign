using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SelectLevels : MonoBehaviour
{
    int latestFile;
    public GameObject levelPrefab;
    public Vector3 startPosition;
    public Vector2 offset;
    public int countInRow = 4;
    public int countInColumn = 4;
    public Button backButton;
    public Button nextButton;
    public Text pageText;
    int firstShownLevelInGrid;
    int currPage = 1;
    int totalPage;
    int itemsInGrid = 16;
    // Use this for initialization
    void Start()
    {
        pageText = GameObject.Find("Page Text").GetComponent<Text>();
        GenerateGrid();
    }


    void GenerateGrid(int genfrom = 0)
    {
        int l = 0;
        int posCOunter = 0;
        ClearLevels();
        firstShownLevelInGrid = genfrom;
        latestFile = GetLastLevel();
        totalPage = (latestFile / itemsInGrid);
        if (latestFile % itemsInGrid != 0)
        {
          totalPage += 1;
        }
        SetPage(currPage);
        for (l = genfrom; l < latestFile; l++)
        {
            GameObject level = Instantiate(levelPrefab) as GameObject;
            level.GetComponent<Level>().number = l+1;
            level.transform.SetParent(transform);
            level.transform.localPosition = startPosition + Vector3.right * (posCOunter % countInRow) * offset.x + Vector3.down * (posCOunter / countInColumn) * offset.y;
            level.transform.localScale = Vector3.one;
            if (posCOunter + 1 >= countInRow * countInColumn) break;
            posCOunter++;
        }
        if (genfrom == 0) backButton.gameObject.SetActive(false);
        else if (genfrom > 0) backButton.gameObject.SetActive(true);
        if (l + 1 >= latestFile) nextButton.gameObject.SetActive(false);
        else nextButton.gameObject.SetActive(true);

    }

    void ClearLevels()
    {
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void Next()
    {
        GenerateGrid(firstShownLevelInGrid + countInRow * countInColumn);
        SetPage(currPage + 1);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.click );
    }

    public void Back()
    {
        GenerateGrid(firstShownLevelInGrid - countInRow * countInColumn);
        SetPage(currPage - 1);
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.click );
    }

    public void SetPage(int page)
    {
        currPage = page;
        pageText.text = string.Format("PAGE {0}/{1}", currPage, totalPage);
    }

    public int GetLastLevel()
    {
        TextAsset mapText = null;
        for (int i = 1; i < 50000; i++)
        {
            mapText = Resources.Load("Levels/" + i) as TextAsset;
            if (mapText == null)
            {
                PlayerPrefs.SetInt("NumLevels", i - 1);
                PlayerPrefs.Save();
                return i - 1;
            }
        }
        return 0;
    }
}
