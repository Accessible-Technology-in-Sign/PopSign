using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITfLiteManager
{
    public void StartRecording();

    public string StopRecording();

    public bool IsRecording();

    public void AddDataToList(object v);

    public void SaveToFile(string landmarks);
}
