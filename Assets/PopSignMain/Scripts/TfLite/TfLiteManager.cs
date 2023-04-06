using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;
using System.IO;

public class TfLiteManager : MonoBehaviour
{
    public static TfLiteManager Instance;

	[SerializeField, FilePopup("*.tflite")] string modelName;

	[HideInInspector]
    public float[,,,] data;
    
	[HideInInspector]
	public float[,] outputs = new float[1, 5];

	[HideInInspector]
	public int maxFrames;

	[HideInInspector]
	public bool isCapturingMediaPipeData = false;

	[HideInInspector]
	public int sessionNumber = 0;

	[HideInInspector]
	public int recordingFrameNumber = 0;

	public static string[] LABELS = { "dad", "elephant", "red", "where", "yellow" };

	private Interpreter interpreter;
	private float timer = 0f;

	public List<List<float>> allData = new List<List<float>>();


    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

		var options = new InterpreterOptions()
		{
			threads = 1,
		};
		interpreter = new Interpreter(FileUtil.LoadFile(modelName), options);
		maxFrames = interpreter.GetInputTensorInfo(0).shape[1];
	}

	public void AddDataToList(List<float> singleFrameData)
    {
		allData.Add(singleFrameData);
		if(allData.Count > maxFrames)
        {
			allData.RemoveAt(0);
        }
    }

	public void StartRecording()
    {
		//Clear Data
		data = new float[1, maxFrames, 63, 1];
		allData = new List<List<float>>();

		isCapturingMediaPipeData = true;
		sessionNumber++;
		recordingFrameNumber = 0;
	}

	public string StopRecording()
    {
		isCapturingMediaPipeData = false;
		timer = 0;
		StartCoroutine(ReadFile());

		return RunModel();
	}

	private IEnumerator ReadFile()
	{
		yield return new WaitForEndOfFrame();
		string path = Application.persistentDataPath + "/" + sessionNumber + "_landmarks.txt"; //dir to be changed accordingly
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
			for(int mediapipevalue = 0; mediapipevalue < 63; mediapipevalue++)
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

		Debug.Log("Output index " + interpreter.GetOutputTensorIndex(20));

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
		if (TfLiteManager.Instance.isCapturingMediaPipeData)
		{
			timer += Time.deltaTime;
		}
	}
}
