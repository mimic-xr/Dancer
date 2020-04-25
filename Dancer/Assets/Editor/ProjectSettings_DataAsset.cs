using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ProjectSettings_DataAsset
{
    [MenuItem("Assets/Create/Audio Module/Project Settings")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<ProjectSettings>();
    }
}