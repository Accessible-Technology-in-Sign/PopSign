using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe;
using Mediapipe.Unity;
using Mediapipe.Unity.CoordinateSystem;
using System.IO;

using Stopwatch = System.Diagnostics.Stopwatch;

public class HolisticMediaPipe : MonoBehaviour
{
    [SerializeField] private TextAsset _configAssetCPU;
    [SerializeField] private TextAsset _configAssetGPU;
    [SerializeField] private RawImage _screen;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _fps;
    [SerializeField] private MultiHandLandmarkListAnnotationController _multiHandLandmarksAnnotationController;
#if UNITY_EDITOR
    private bool useGPU = false;
#else
    private bool useGPU = true;
#endif

    private CalculatorGraph _graph;

    private WebCamTexture _webCamTexture;
    private Texture2D _inputTexture;
    private Color32[] _inputPixelData;

    private IEnumerator Start()
    {
        if (WebCamTexture.devices.Length == 0)
        {
            throw new System.Exception("Web Camera devices are not found");
        }

        int defaultSource = 0;
            
        for (int i = 0; i < WebCamTexture.devices.Length; i++)
        {
            if(WebCamTexture.devices[i].isFrontFacing == true)
            {
                defaultSource = i;
                break;
            }
        }

        var webcamDevice = WebCamTexture.devices[defaultSource];

        _webCamTexture = new WebCamTexture(webcamDevice.name, _width, _height, _fps);
        _webCamTexture.Play();

        yield return new WaitUntil(() => _webCamTexture.width > 16);
        if (useGPU)
        {
            yield return GpuManager.Initialize();

            if (!GpuManager.IsInitialized)
            {
                throw new System.Exception("Failed to initialize GPU resources");
            }
        }

        _screen.rectTransform.sizeDelta = new Vector2(_width, _height);
            
        _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
        _inputPixelData = new Color32[_width * _height];
            
        _screen.texture = _webCamTexture;
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("face_detection_short_range.bytes");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("face_landmark.bytes");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("hand_landmark_full.bytes");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("hand_recrop.bytes");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("handedness.txt");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("palm_detection_full.bytes");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("pose_detection.bytes");
        yield return MediapipeResourceManager.Instance.resourceManager.PrepareAssetAsync("pose_landmark_full.bytes");

        var stopwatch = new Stopwatch();

        if (useGPU)
        {
            _graph = new CalculatorGraph(_configAssetGPU.text);
            _graph.SetGpuResources(GpuManager.GpuResources).AssertOk();
        }
        else
        {
            _graph = new CalculatorGraph(_configAssetCPU.text);
        }
        var poseLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "pose_landmarks");
        poseLandmarksStream.StartPolling().AssertOk();
        var leftHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "left_hand_landmarks");
        leftHandLandmarksStream.StartPolling().AssertOk();
        var rightHandLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "right_hand_landmarks");
        rightHandLandmarksStream.StartPolling().AssertOk();
        var faceLandmarksStream = new OutputStream<NormalizedLandmarkListPacket, NormalizedLandmarkList>(_graph, "face_landmarks");
        faceLandmarksStream.StartPolling().AssertOk();


        var sidePacket = new SidePacket();
        sidePacket.Emplace("input_horizontally_flipped", new BoolPacket(false));
        sidePacket.Emplace("output_horizontally_flipped", new BoolPacket(false));
        sidePacket.Emplace("input_rotation", new IntPacket(0));
        sidePacket.Emplace("output_rotation", new IntPacket(0));
        sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
        sidePacket.Emplace("output_vertically_flipped", new BoolPacket(true));

        _graph.StartRun(sidePacket).AssertOk();

        stopwatch.Start();

        var screenRect = _screen.GetComponent<RectTransform>().rect;

        while (true)
        {
            _inputTexture.SetPixels32(_webCamTexture.GetPixels32(_inputPixelData));
            var imageFrame = new ImageFrame(ImageFormat.Types.Format.Srgba, _width, _height, _width * 4, _inputTexture.GetRawTextureData<byte>());
            var currentTimestamp = stopwatch.ElapsedTicks / (System.TimeSpan.TicksPerMillisecond / 1000);
            _graph.AddPacketToInputStream("input_video", new ImageFramePacket(imageFrame, new Timestamp(currentTimestamp))).AssertOk();

            yield return new WaitForEndOfFrame();

            poseLandmarksStream.TryGetNext(out var poseLandmarks);
            leftHandLandmarksStream.TryGetNext(out var leftHandLandmarks);
            faceLandmarksStream.TryGetNext(out var faceLandmarks);

            if(rightHandLandmarksStream.TryGetNext(out var rightHandLandmarks))
            {
                List<NormalizedLandmarkList> rightHandList = new List<NormalizedLandmarkList>();
                rightHandList.Add(rightHandLandmarks);

                Debug.Log(rightHandLandmarks.Landmark[0].X + " " + rightHandLandmarks.Landmark[0].Y + " " + rightHandLandmarks.Landmark[0].Z);

                _multiHandLandmarksAnnotationController.DrawNow(rightHandList);
            }
            else
            {
                _multiHandLandmarksAnnotationController.DrawNow(null);
            }
            

        }
    }

    private void SaveToFile(NormalizedLandmarkList landmarks)
    {
        string path = Application.persistentDataPath + "/" + TfLiteManager.Instance.sessionNumber + "_landmarks.txt"; //dir to be changed accordingly
        if (TfLiteManager.Instance.recordingFrameNumber == 0)
        {
            File.WriteAllText(path, string.Empty);
        }
        StreamWriter sWriter = new StreamWriter(path, true);
        if (TfLiteManager.Instance.recordingFrameNumber == 0)
        {
            sWriter.Write("{\"" + TfLiteManager.Instance.recordingFrameNumber + "\": " + landmarks);
        }
        else
        {
            sWriter.Write(",\"" + TfLiteManager.Instance.recordingFrameNumber + "\": " + landmarks);
        }
        sWriter.Close();
    }

    private void OnDestroy()
    {
        if (_webCamTexture != null)
        {
            _webCamTexture.Stop();
        }
        if (_graph != null)
        {
            try
            {
                _graph.CloseInputStream("input_video").AssertOk();
                _graph.WaitUntilDone().AssertOk();
            }
            finally
            {
                _graph.Dispose();
            }
        }
        if (useGPU)
        {
            GpuManager.Shutdown();
        }
    }
}


