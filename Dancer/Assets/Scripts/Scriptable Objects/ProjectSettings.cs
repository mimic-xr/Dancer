using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectSettings : ScriptableObject
{
    public string _SettingPreset;

    [Header("Input Settings")]
    public string selectedDevice = "USB Audio Device";

    [Header("BMP Analyser")]
    public int RecordTime = 10;   //How long to record audio for to analyse
    public float CheckFreq = 5;  //How frequently (seconds) to update the BMP
    public float TransitionSpeed = 0.8f;
}
