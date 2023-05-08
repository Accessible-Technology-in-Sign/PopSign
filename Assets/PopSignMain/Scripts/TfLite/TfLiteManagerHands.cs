using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using System.IO;
using UnityEngine.Networking;

public class TfLiteManagerHands : MonoBehaviour, ITfLiteManager
{

	[SerializeField, FilePopup("*.tflite")] string modelName;

	[HideInInspector]
    public float[,,,] data;
    
	[HideInInspector]
	public float[,] outputs = new float[1, 5];

	[HideInInspector]
	public int maxFrames;

	private bool isCapturingMediaPipeData = false;

	[HideInInspector]
	public int sessionNumber = 0;

	[HideInInspector]
	public int recordingFrameNumber = 0;

	public static string[] LABELS = { "dad", "elephant", "red", "where", "yellow" };

	private Interpreter interpreter;
	private float timer = 0f;

	public List<float[]> allData = new List<float[]>();

	[HideInInspector]
	public bool isResponseReady = false;

	private int inputSize;

	// Start is called before the first frame update
	void Awake()
    {
        if(TfLiteManager.Instance == null)
        {
			TfLiteManager.Instance = this;
        }

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		interpreter = new Interpreter(FileUtil.LoadFile(modelName), options);
		maxFrames = interpreter.GetInputTensorInfo(0).shape[1];
		inputSize = interpreter.GetInputTensorInfo(0).shape[2];
	}

	public void AddDataToList(object singleFrameData)
    {
		var listdata = (float[])singleFrameData;
		allData.Add(listdata);
		if(allData.Count > maxFrames)
        {
			allData.RemoveAt(0);
        }

		recordingFrameNumber++;
	}

	public void StartRecording()
    {
		//Clear Data
		data = new float[1, maxFrames, inputSize, 1];
		allData = new List<float[]>();

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

	private void CloseFileIfExists()
	{
		string path = Application.persistentDataPath + "/" + sessionNumber + "_landmarks.txt"; //dir to be changed accordingly

		//if file doesn't exist, than no need to write the final line
		if (!File.Exists(path))
			return;

		StreamWriter sWriter = new StreamWriter(path, true);
		sWriter.Write("}");
		sWriter.Close();
	}

	private string RunModel()
    {
		outputs = new float[1, 5];


		if (allData.Count < maxFrames)
        {
			var middleData = allData[allData.Count / 2];
			int middleDataIndex = allData.Count / 2;
			int framesToAdd = maxFrames - allData.Count;
			for (int i = 0; i < framesToAdd; i++) {
				allData.Insert(middleDataIndex, middleData);
			}
        }

		for(int frameNumber = 0; frameNumber < maxFrames; frameNumber++)
        {
			for(int mediapipevalue = 0; mediapipevalue < inputSize; mediapipevalue++)
            {
				data[0, frameNumber, mediapipevalue, 0] = allData[frameNumber][mediapipevalue];
            }
        }

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		interpreter = new Interpreter(FileUtil.LoadFile(modelName), options);

		var info = interpreter.GetInputTensorInfo(0);

		Debug.Log("Input " + data[0,10,10,0]);

		// Allocate input buffer
		interpreter.AllocateTensors();

		interpreter.SetInputTensorData(0, data);

		// Blackbox!!
		interpreter.Invoke();

		// Debug.Log("Output index " + interpreter.GetOutputTensorIndex(20));

		// Get data
		interpreter.GetOutputTensorData(0, outputs);

		//label1: 
		float max = 0f;
		string answer = "";
		for (int i = 0; i < 5; i++)
		{
			if (outputs[0, i] > max)
			{
				max = outputs[0, i];
				answer = LABELS[i];

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
			sWriter.Write("{\"" + recordingFrameNumber + "\": " + landmarks);
		}
		else
		{
			sWriter.Write(",\"" + recordingFrameNumber + "\": " + landmarks);
		}
		sWriter.Close();
		recordingFrameNumber++;
	}
}
