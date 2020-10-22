﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PreTutorial : MonoBehaviour {
    public Sprite[] pictures;

	// Use this for initialization
	void Start () {
        GetComponent<Image>().sprite = pictures[(int)LevelData.mode];
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.swish[0] );

	}

	// Update is called once per frame
	public void  Stop() {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.swish[1] );

        GamePlay.Instance.GameStatus = GameState.Tutorial;
        gameObject.SetActive( false );
	}
}
