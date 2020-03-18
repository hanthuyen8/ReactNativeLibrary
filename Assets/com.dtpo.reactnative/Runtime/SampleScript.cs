using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Game;

public class SampleScript : MonoBehaviour
{
    public Text _debugText1;

    public void SendRecord()
    {
        ReactNative.RecordAudio("sister", 3, ResultAudio);
    }

    public void ResultAudio(bool result)
    {
        _debugText1.text = result.ToString();
    }

}