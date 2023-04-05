using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[HideInInspector]
	public bool pointerDown = false;

	[HideInInspector]
	public bool sessionDone = false;

	[HideInInspector]
	public int pictureNumber = 0;
	
	[HideInInspector]
	public float timer = 0;
	
	[HideInInspector]
	public int sessionNumber = 0;
	
	[HideInInspector]
	public int frameNumber = 0;


	public void OnPointerDown(PointerEventData eventData)
	{
		TfLiteManager.Instance.EmptyData();
		pointerDown = true;
		sessionNumber++;
		Debug.Log("Start time of capture");
		frameNumber = 0;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pointerDown = false;
		sessionDone = true;
		Debug.Log("End time of capture");
		Debug.Log("pictureNumber: " + pictureNumber);
		Debug.Log("timer: " + timer);
		Debug.Log("FPS: " + pictureNumber / timer);
		pictureNumber = 0;
		timer = 0;
		//StartCoroutine(ReadFile());

		TfLiteManager.Instance.RunModel();
	}

	private IEnumerator ReadFile()
    {
		yield return new WaitForEndOfFrame();
		string path = Application.dataPath + "/Images/" + sessionNumber + " landmarks.txt"; //dir to be changed accordingly
		StreamWriter sWriter = new StreamWriter(path, true);
		sWriter.Write("}");
		sWriter.Close();
	}
	
	private void Update()
	{
		if (pointerDown)
		{
			timer += Time.deltaTime;
		}
	}
	
}
