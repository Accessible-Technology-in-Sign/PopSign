using UnityEngine;
using System.Collections;
using LitJson;

public enum BallColor
{
    blue = 1,
    green,
    red,
    violet,
    yellow,
    random,
    star
}

public class ColorBallScript : MonoBehaviour
{
    //POPSign tie the video name with ball
    //public Video video;

    public Sprite[] sprites;
    public BallColor mainColor;
    // Use this for initialization
    void Start()
    {
    }


    public void SetColor(BallColor color)
    {
        mainColor = color;
        foreach (Sprite item in sprites)
        {
            if (item.name == "ball_" + color)
            {
                GetComponent<SpriteRenderer>().sprite = item;
                gameObject.tag = "" + color;
            }
        }
    }

    public void SetColor(int color)
    {
        mainColor = (BallColor)color;
        GetComponent<SpriteRenderer>().sprite = sprites[color];
    }

    public void ChangeRandomColor()
    {
        mainscript.Instance.GetColorsInGame();
        SetColor((BallColor)mainscript.colorsDict[Random.Range(0, mainscript.colorsDict.Count)]);
        GetComponent<Animation>().Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y <= -16 && transform.parent == null) { Destroy(gameObject); }
    }
}
