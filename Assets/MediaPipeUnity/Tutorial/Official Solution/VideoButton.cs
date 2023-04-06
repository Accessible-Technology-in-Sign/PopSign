using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

	public void OnPointerDown(PointerEventData eventData)
	{
		TfLiteManager.Instance.StartRecording();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		TfLiteManager.Instance.StopRecording();
	}
	
}
