using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITfLiteManager
{
    public void StartRecording();

    public string StopRecording();

    public bool IsRecording();

    public void AddDataToList(List<float> v);

    public void SaveToFile(string landmarks);
}
