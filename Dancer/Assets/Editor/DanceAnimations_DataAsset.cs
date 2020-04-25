using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DanceAnimations_DataAsset
{
    [MenuItem("Assets/Create/Audio Module/Dance List")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<DanceList>();
    }
}