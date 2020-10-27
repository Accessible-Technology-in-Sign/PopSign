using UnityEngine;
using System.Collections;

public class butterfly : MonoBehaviour {
	Vector3 tempPosition;
	Vector3 targetPrepare;
	bool started;
	bool isPaused;
	float startTime;
	bool arcadeMode;
	public int revertButterFly;
	// Use this for initialization
	void Start () {
		isPaused = Camera.main.GetComponent<mainscript>().isPaused;
		arcadeMode = Camera.main.GetComponent<mainscript>().arcadeMode;

	}
	
	// Update is called once per frame
	void Update () {
		if(transform.position.x < -4 || transform.position.x > 4) Destroy(gameObject);
	}

    public void OnTriggerEnter2D(Collider2D owner)
    {
        // check if we collided with a top block and adjust our speed and rotation accordingly
        if (owner.gameObject.name.IndexOf("ball")==0 && owner.gameObject.GetComponent<ball>().setTarget)
        {
			Destroy(gameObject);
		}
	}

}
