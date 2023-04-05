using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlowLite;

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

    public static string[] LABELS = { "dad", "elephant", "red", "where", "yellow" };

	private Interpreter interpreter;


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

	public void EmptyData()
    {
		data = new float[1, maxFrames, 63, 1];
	}

    public void RunModel()
    {
		outputs = new float[1, 5];

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
	}
}
