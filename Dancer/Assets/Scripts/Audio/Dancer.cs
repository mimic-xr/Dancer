using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dancer : MonoBehaviour
{
    public Animator _Dancer;
    public AnimatorOverrideController _AnimationOverride;
    public int _Switch;

    [Header("Debug")]
    public string _SelectedDance;
    public int _DancerBMP = 107;
    public float _DesiredSpeed;    // Speed Required to match BMP
    public float _PreviousSpeed;   // Store Previous Speed for Lerp
    public float _CurrentSpeed;    // Current Speed of Dancer
}
