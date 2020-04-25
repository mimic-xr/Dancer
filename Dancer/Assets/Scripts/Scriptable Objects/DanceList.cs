using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceList : ScriptableObject
{
    public string _DanceGenre;
    public int _GenreID;

    [System.Serializable]
    public class Dance
    {
        public string _Dance;
        public AnimationClip _Clip;
        public int Bmp;
        public int Length;
    }

    public Dance[] _Dances;
}