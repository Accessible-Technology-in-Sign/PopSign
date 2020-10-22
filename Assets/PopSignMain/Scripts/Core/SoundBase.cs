using UnityEngine;
using System.Collections;
[RequireComponent( typeof( AudioSource ) )]
public class SoundBase : MonoBehaviour {
	public static SoundBase Instance;
	public AudioClip click;
	public AudioClip[] combo;
	public AudioClip[] swish;
	public AudioClip bug;
	public AudioClip bugDissapier;
	public AudioClip pops;
 	public AudioClip boiling;
 	public AudioClip hit;
 	public AudioClip kreakWheel;
 	public AudioClip spark;
 	public AudioClip winSound;
 	public AudioClip gameOver;
 	public AudioClip scoringStar;
 	public AudioClip scoring;
 	public AudioClip alert;
 	public AudioClip aplauds;
 	public AudioClip OutOfMoves;
 	public AudioClip Boom;
 	public AudioClip black_hole;

    ///SoundBase.Instance.audio.PlayOneShot( SoundBase.Instance.kreakWheel );

   // Use this for initialization
	void Awake () {
		if(Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(Instance);
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
