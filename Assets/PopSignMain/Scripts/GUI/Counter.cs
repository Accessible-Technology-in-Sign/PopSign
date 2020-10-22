using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InitScriptName;

public class Counter : MonoBehaviour {
  //  UILabel label;
  Text label;
	// Use this for initialization
	void Start ()
  {
      label = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update ()
  {
        if (name == "Moves")
        {
            label.text = "" + LevelData.LimitAmount;
            if( LevelData.LimitAmount <= 5 && GamePlay.Instance.GameStatus == GameState.Playing )
            {
                Color warningColor = new Color32(0xBB,0x4F, 0x66, 0xFF);
                label.color = warningColor;
                GameObject.Find("CanvasMoves").transform.Find("MovesText").GetComponent<Text>().color = warningColor;
                if( !GetComponent<Animation>().isPlaying )
                {
                    GetComponent<Animation>().Play();
                    SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.alert );
                }
            }
        }
        if( name == "Level" )
        {
            label.text = "Level " + PlayerPrefs.GetInt("OpenLevel");
        }
  }
}
