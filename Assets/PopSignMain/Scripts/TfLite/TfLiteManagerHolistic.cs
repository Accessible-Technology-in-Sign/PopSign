using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using System.IO;
using UnityEngine.Networking;

public class TfLiteManagerHolistic : MonoBehaviour, ITfLiteManager
{

	[SerializeField, FilePopup("*.tflite")] string modelName;

	[HideInInspector]
	public float?[,,] input;

	[HideInInspector]
	public float[] outputs = new float[250];

	public int maxFrames;

	private bool isCapturingMediaPipeData = false;

	[HideInInspector]
	public int sessionNumber = 0;

	[HideInInspector]
	public int recordingFrameNumber = 0;

	public static string[] LABELS = { "chocolate", "for", "frenchfries", "sleep", "stuck" };

	private SignatureRunner runner;
	private float timer = 0f;

	public Queue<float?[,]> allData = new Queue<float?[,]>();

	[HideInInspector]
	public bool isWaitingForResponse = false;
	[HideInInspector]
	public bool isResponseReady = false;



	// Start is called before the first frame update
	void Awake()
	{
		if (TfLiteManager.Instance == null)
		{
			TfLiteManager.Instance = this;
		}

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		runner = new SignatureRunner("serving_default", FileUtil.LoadFile(modelName), options);
	}

	public void AddDataToList(object singleFrameData)
	{
		var floatdata = (float?[,])singleFrameData;
		allData.Enqueue(floatdata);
		if (allData.Count > maxFrames)
		{
			allData.Dequeue();
		}

		recordingFrameNumber++;
	}

	public void StartRecording()
	{
		//Clear Data
		input = new float?[maxFrames, 543, 3];
		allData = new Queue<float?[,]>();
		isCapturingMediaPipeData = true;
		sessionNumber++;
		recordingFrameNumber = 0;
	}

	public string StopRecording()
	{
		isCapturingMediaPipeData = false;
		timer = 0;

		CloseFileIfExists();
		return RunModel();
	}

	public bool IsRecording()
	{
		return isCapturingMediaPipeData;
	}

	private string RunModel()
	{
		outputs = new float[250];

		//For now we aren't padding data
		//if (allData.Count < maxFrames)
		//{
		//	var middleData = allData.[allData.Count / 2];
		//	int middleDataIndex = allData.Count / 2;
		//	int framesToAdd = maxFrames - allData.Count;
		//	for (int i = 0; i < framesToAdd; i++)
		//	{
		//		allData.Insert(middleDataIndex, middleData);
		//	}
		//}

		int dataRecordedSize = allData.Count;
		input = new float?[dataRecordedSize, 543, 3];

		for (int frameNumber = 0; frameNumber < dataRecordedSize; frameNumber++)
		{
			var currentFrameData = allData.Dequeue();

			for (int mediapipevalue = 0; mediapipevalue < 543; mediapipevalue++)
			{
				//This can be optimized later
				input[frameNumber, mediapipevalue, 0] = currentFrameData[mediapipevalue, 0];
				input[frameNumber, mediapipevalue, 1] = currentFrameData[mediapipevalue, 1];
				input[frameNumber, mediapipevalue, 2] = currentFrameData[mediapipevalue, 2];
			}
		}

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		runner = new SignatureRunner("serving_default", FileUtil.LoadFile(modelName), options);

		var info = runner.GetInputTensorInfo(0);

		// Allocate input buffer
		//interpreter.AllocateTensors();

		runner.SetInputTensorData(0, input);

		// Blackbox!!
		//interpreter.Invoke();

		// Debug.Log("Output index " + interpreter.GetOutputTensorIndex(20));

		// Get data
		//interpreter.GetOutputTensorData(0, outputs);

		//label1: 
		float max = 0f;
		string answer = "";
		for (int i = 0; i < outputs.Length; i++)
		{
			if (outputs[i] > max)
			{
				max = outputs[i];
				answer = "" + i;

			}
		}

		Debug.Log("Max Probability " + max);
		Debug.Log("results!!!!!!!!!!!!!!!!!! " + answer);

		return answer;
	}

	private void Update()
	{
		if (isCapturingMediaPipeData)
		{
			timer += Time.deltaTime;
		}
	}

	public void SaveToFile(string landmarks)
	{
		string path = Application.persistentDataPath + "/" + sessionNumber + "_landmarks.txt"; //dir to be changed accordingly

		if (recordingFrameNumber == 0)
		{
			File.WriteAllText(path, string.Empty);
		}
		StreamWriter sWriter = new StreamWriter(path, true);
		if (recordingFrameNumber == 0)
		{
			sWriter.Write("{\"" + recordingFrameNumber + "\": [" + landmarks + "]");
		}
		else
		{
			sWriter.Write(",\"" + recordingFrameNumber + "\": [" + landmarks + "]");
		}
		sWriter.Close();
	}

	private void CloseFileIfExists()
	{
		string path = Application.persistentDataPath + "/" + sessionNumber + "_landmarks.txt"; //dir to be changed accordingly
		
		//if file doesn't exist, than no need to write the final line
		if (!File.Exists(path))
			return;
		
		Debug.Log("File Saved to " + path);
		StreamWriter sWriter = new StreamWriter(path, true);
		sWriter.Write("}");
		sWriter.Close();
	}
}
