using System.Collections;
using System.Collections.Generic;
using System.IO;
using TensorFlowLite;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	[HideInInspector]
	public bool pointerDown = false;

	[HideInInspector]
	public float[,,,] data;
	float[,] outputs = new float[1, 5];

	[HideInInspector]
	public bool sessionDone = false;
	public int pictureNumber = 0;
	public float timer = 0;
	public int sessionNumber = 0;
	public int frameNumber = 0;


	public static string[] labels = {"dad","elephant","red","where","yellow"};

	void Awake()
	{

	}

	public void OnPointerDown(PointerEventData eventData)
	{
		data = new float[1, 300, 63, 1];
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
		StartCoroutine(ReadFile());


		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		var interpreter = new Interpreter(FileUtil.LoadFile("model_final.tflite"), options);
		var info = interpreter.GetInputTensorInfo(0);

		Debug.Log("Input " + info);

		// Allocate input buffer
		//interpreter.ResizeInputTensor(0, new int[] { 1, 300, 63, 1 });
		interpreter.AllocateTensors();

		interpreter.SetInputTensorData(0, data);

		// Blackbox!!
		interpreter.Invoke();

		Debug.Log("Output index " + interpreter.GetOutputTensorIndex(20));

		// Get data
		interpreter.GetOutputTensorData(0, outputs);

		Debug.Log("results!!!!!!!!!!!!!!!!!! " + outputs[0, 1]);

		//label1: 
		float max = 0f;
		string answer = "";
		for (int i = 0; i < 5; i++){
			if (outputs[0, i] > max)
            {
				max = outputs[0, i];
				answer = labels[i];

			}
        }

		Debug.Log("results!!!!!!!!!!!!!!!!!! " + answer);
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
