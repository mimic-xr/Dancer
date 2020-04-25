using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour
{
    [SerializeField]
    public ProjectSettings _ProjectSettings;

    [Header("Audio")]
    public AudioSource Source;
    private AudioClip targetClip;
    public int bpm; //BMP of the audio playing
    public float ad_bmp;       //Adjusted BMP of the song
    public int Max_Bmp = 140;  //Max Danceable speed 
    public int Min_Bmp = 90;   //Min Danceable speed 
    [Header("Dancer")]
    //public List<Dancer> _Dancer = new List<Dancer>();
    public Dancer[] _Dancer;
    public float BMP_Search_Range = 7f;
    public DanceList[] _DanceList;
    public List<DanceList.Dance> _AllDances = new List<DanceList.Dance>();
    private List<DanceList.Dance> _TempDances = new List<DanceList.Dance>();

    [Header("Debug")]
    public float RangeMax;
    public float RangeMin;

    static float T = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Dancer dancer in _Dancer)
        {
            dancer._Dancer.runtimeAnimatorController = dancer._AnimationOverride;
        }

        CreateAnimationList();

        targetClip = Source.clip;
        Debug.Log(targetClip);

        for (int i = 0; i < Microphone.devices.Length; i++)
        {
            print(Microphone.devices[i]);
        }
        if (Microphone.devices.Length > 0)
        {
            _ProjectSettings.selectedDevice = Microphone.devices[0].ToString();
            Source.clip = Microphone.Start(_ProjectSettings.selectedDevice, true, _ProjectSettings.RecordTime, AudioSettings.outputSampleRate);
            while (!(Microphone.GetPosition(null) > 0))
            {

            }
        }
        StartCoroutine("DoCheck");
    }

    // Update is called once per frame
    void Update()
    {
        targetClip = Source.clip;

        foreach (Dancer dancer in _Dancer)
        {
            dancer._DesiredSpeed = ad_bmp / dancer._DancerBMP;
            dancer._Dancer.speed = dancer._CurrentSpeed;
            // Smooth Transition to new speed
            if (dancer._CurrentSpeed != dancer._DesiredSpeed)
            {
                LerpSpeed(dancer);
            }
        }


        if (Input.GetKeyDown("space"))
        {
            ChangeAnimation();
        }

    }

    void CreateAnimationList()
    {
        for (int i = 0; i < _DanceList.Length; i++)
        {
            for (int c = 0; c < _DanceList[i]._Dances.Length; c++)
            {
                _AllDances.Add(_DanceList[i]._Dances[c]);
            }
        }
    }
    public void ChangeAnimation()
    {
        _TempDances.Clear();

        // AREA TO SEARCH
        RangeMax = ad_bmp + BMP_Search_Range;
        RangeMin = ad_bmp - BMP_Search_Range;

        // LIST AVAILABLE DANCES
        for (int i = 0; i < _AllDances.Count; i++)
        {
            float _CheckBMP = (float)_AllDances[i].Bmp;
            print(_CheckBMP);
            if (_CheckBMP < RangeMax)
            {
                if ( _CheckBMP > RangeMin)
                {
                    _TempDances.Add(_AllDances[i]);
                }
            }
        }
        foreach (Dancer dancer in _Dancer)
        {
            if (_TempDances.Count == 0)
            {
                dancer._SelectedDance = "None Found";
            }

            int selection = Random.Range(0, _TempDances.Count);
            if (dancer._Switch == 0)
            {
                dancer._AnimationOverride["B"] = _TempDances[selection]._Clip;
                dancer._Dancer.SetBool("Switch_0", true);
                dancer._Dancer.SetBool("Switch_1", false);
                dancer._Switch = 1;
            }
            else
            {
                dancer._AnimationOverride["A"] = _TempDances[selection]._Clip;
                dancer._Dancer.SetBool("Switch_1", true);
                dancer._Dancer.SetBool("Switch_0", false);
                dancer._Switch = 0;
            }

            dancer._SelectedDance = _TempDances[selection]._Dance;
            //SET SPEED
            dancer._PreviousSpeed = dancer._CurrentSpeed;
            dancer._DancerBMP = _TempDances[selection].Bmp;
            dancer._CurrentSpeed = ad_bmp / (_TempDances[selection].Bmp * dancer._CurrentSpeed);
            dancer._DesiredSpeed = ad_bmp / _TempDances[selection].Bmp;
        }
    }

    IEnumerator DoCheck()
    {
        for (; ; )
        {
            Find_bmp();
            yield return new WaitForSeconds(5f);
            Find_bmp();
            yield return new WaitForSeconds(5f);
            Find_bmp();
            ChangeAnimation();
            yield return new WaitForSeconds(5f);
        }
    }

    void Find_bmp()
    {
        int i  = 0;
        while(i<2)
        {
            bpm = UniBpmAnalyzer.AnalyzeBpm(targetClip);
            i = i + 1;
        }
        if(bpm<Min_Bmp)
        {
            ad_bmp = bpm * 2;
        }
        else if (bpm > Max_Bmp)
        {
            ad_bmp = bpm / 2;
        }
        else
        {
            ad_bmp = bpm;
        }
        foreach (Dancer dancer in _Dancer)
        {
            dancer._PreviousSpeed = dancer._CurrentSpeed;
            dancer._DesiredSpeed = ad_bmp / dancer._DancerBMP;
        }

    }

    void LerpSpeed(Dancer a)
    {
        a._CurrentSpeed = (Mathf.Lerp(a._PreviousSpeed,a._DesiredSpeed, T));
        T += _ProjectSettings.TransitionSpeed * Time.deltaTime;
        if (T > 1.0f)
        {
            a._CurrentSpeed = a._DesiredSpeed;
            T = 0.0f;
        }
    }
}
