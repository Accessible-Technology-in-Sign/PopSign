using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FramrateDebugger : MonoBehaviour
{
    private float frequency = 1.0f;
    private string fps;

    [SerializeField] private TMPro.TMP_Text fpsDisplay;

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
            fpsDisplay.text = fps;
        }
    }
}