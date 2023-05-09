using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mediapipe.Unity.CoordinateSystem;
using System.IO;

using Stopwatch = System.Diagnostics.Stopwatch;

namespace Mediapipe.Unity.Tutorial
{
  public class HandTrackingLandmarks : MonoBehaviour
  {
    [SerializeField] private TextAsset _configAsset;
    [SerializeField] private RawImage _screen;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _fps;

    private CalculatorGraph _graph;
    private ResourceManager _resourceManager;

    private WebCamTexture _webCamTexture;
    private Texture2D _inputTexture;
    private Color32[] _inputPixelData;
    private Texture2D _outputTexture;
    private Color32[] _outputPixelData;

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

      var webCamDevice = WebCamTexture.devices[defaultSource];
      
      _webCamTexture = new WebCamTexture(webCamDevice.name, _width, _height, _fps);
      _webCamTexture.Play();

      yield return new WaitUntil(() => _webCamTexture.width > 16);

      _screen.rectTransform.sizeDelta = new Vector2(_width, _height);

      _inputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
      _inputPixelData = new Color32[_width * _height];
      _outputTexture = new Texture2D(_width, _height, TextureFormat.RGBA32, false);
      _outputPixelData = new Color32[_width * _height];

      _screen.texture = _outputTexture;

      _resourceManager = new StreamingAssetsResourceManager();
      yield return _resourceManager.PrepareAssetAsync("hand_landmark_full.bytes");
      yield return _resourceManager.PrepareAssetAsync("hand_landmark_lite.bytes");
      yield return _resourceManager.PrepareAssetAsync("hand_recrop.bytes");
      yield return _resourceManager.PrepareAssetAsync("handedness.txt");

      var stopwatch = new Stopwatch();

      _graph = new CalculatorGraph(_configAsset.text);
      var outputVideoStream = new OutputStream<ImageFramePacket, ImageFrame>(_graph, "output_video");
      var handLandmarksStream = new OutputStream<NormalizedLandmarkListVectorPacket, List<NormalizedLandmarkList>>(_graph, "hand_landmarks");
      outputVideoStream.StartPolling().AssertOk();
      handLandmarksStream.StartPolling().AssertOk();

      var sidePacket = new SidePacket();
      sidePacket.Emplace("input_horizontally_flipped", new BoolPacket(false));
      sidePacket.Emplace("input_rotation", new IntPacket(0));
      sidePacket.Emplace("input_vertically_flipped", new BoolPacket(true));
      sidePacket.Emplace("num_hands", new IntPacket(1));

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

        if (outputVideoStream.TryGetNext(out var outputVideo))
        {
          if (outputVideo.TryReadPixelData(_outputPixelData))
          {
            _outputTexture.SetPixels32(_outputPixelData);
            _outputTexture.Apply();
            
          }
        }

        if (handLandmarksStream.TryGetNext(out var handLandmarks))
        {
          if (handLandmarks != null && handLandmarks.Count > 0)
          {
            foreach (var landmarks in handLandmarks)
            {
              // top of the head
              var topOfHead = landmarks.Landmark[8];
              Debug.Log($"Unity Local Coordinates: {screenRect.GetPoint(topOfHead)}, Image Coordinates: {topOfHead}");
            }
          }
        }
      }
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
    }
  }
}
