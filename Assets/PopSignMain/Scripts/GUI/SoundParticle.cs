using UnityEngine;
using System.Collections;

public class SoundParticle : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Stop () {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.swish[0] );
	}
    public void Hit()
    {
        SoundBase.Instance.GetComponent<AudioSource>().PlayOneShot( SoundBase.Instance.hit );
    }

}
