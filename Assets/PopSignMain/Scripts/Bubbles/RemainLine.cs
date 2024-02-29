using UnityEngine;
using System.Collections;

public class RemainLine : MonoBehaviour {
	LineRenderer line;

	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer>();
		if(Camera.main.orthographicSize == 5) transform.position = Vector2.zero + Vector2.up*5.8f;
		else if(Camera.main.orthographicSize == 6) transform.position = Vector2.zero + Vector2.up*6.7f;
		line.SetPosition(0, new Vector2(-8f , transform.position.y));
		line.SetPosition(1, new Vector2(8f , transform.position.y));
	}

	public void UpdateLine(float step){
		line.SetPosition(0, new Vector2(-8f + step/2f, transform.position.y));
		line.SetPosition(1, new Vector2(8f - step/2f, transform.position.y));

	}

	public void ResetLine(){
		line.SetPosition(0, new Vector2(-8f, transform.position.y));
		line.SetPosition(1, new Vector2(8f, transform.position.y));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
