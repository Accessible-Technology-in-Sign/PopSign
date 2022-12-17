using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool pointerDown = false;
    public int pictureNumber = 0;
	public float timer = 0;

	public void OnPointerDown(PointerEventData eventData)
	{
		pointerDown = true;
		Debug.Log("Start time of capture");
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		pointerDown = false;
		Debug.Log("End time of capture");
		Debug.Log("pictureNumber: " + pictureNumber);
		Debug.Log("timer: " + timer);
		Debug.Log("FPS: " + pictureNumber / timer);
        pictureNumber = 0;
		timer = 0;
		
	}

	
	private void Update()
	{
		if (pointerDown)
		{
			timer += Time.deltaTime;
		}
	}
	
}
