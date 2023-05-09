using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FramrateDebugger : MonoBehaviour
{
    private float frequency = 1.0f;
    private string fps;

    [SerializeField] private TMPro.TMP_Text fpsDisplay;
    [SerializeField] private Text fpsDisplayNormalText;

    void Start()
    {
        StartCoroutine(FPS());
    }

    private IEnumerator FPS()
    {
        for (; ; )
        {
            // Capture frame-per-second
            int lastFrameCount = Time.frameCount;
            float lastTime = Time.realtimeSinceStartup;
            yield return new WaitForSeconds(frequency);
            float timeSpan = Time.realtimeSinceStartup - lastTime;
            int frameCount = Time.frameCount - lastFrameCount;

            // Display it

            fps = "" + Mathf.RoundToInt(frameCount / timeSpan);
            if (fpsDisplay != null)
                fpsDisplay.text = fps;
            else
                fpsDisplayNormalText.text = fps;
        }
    }
}