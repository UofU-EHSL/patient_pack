using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class blood_pressure : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    [Header("SYSTOLIC")]
    public float BloodPressureSystolic;
    public int systolicMinValue;
    public UnityEvent systolicMin;
    public int systolicMaxValue;
    public UnityEvent systolicMax;
    [HideInInspector]
    public float StartingSystolic;
    
    [Header("DIASTOLIC")]
    public float BloodPressureDiastolic;
    public int diastolicMinValue;
    public UnityEvent diastolicMin;
    public int diastolicMaxValue;
    public UnityEvent diastolicMax;
    [HideInInspector]
    public float StartingDiastolic;

    [Header("MEAN")]
    public Text mean;
    public int meanMinValue;
    public UnityEvent meanMin;
    public int meanMaxValue;
    public UnityEvent meanMax;
    [Header("LINE RENDERER")]
    // none of this is used yet
    public patient_line_renderer line_renderer;
    public AnimationCurve abp_line;
    public float ABP = 1;
    public float vertical_scale;
    public float compression;
    public float move_speed;
    public bool active;
    private float nextActionTime = 0.0f;
    private float active_time;

    private string valuesString = "";

    private void Start()
    {
        StartingDiastolic = BloodPressureDiastolic;
        StartingSystolic = BloodPressureSystolic;
    }
    //other
    private void FixedUpdate()
    {
        float bpm = 60;
        //systolid max and min events
        if (BloodPressureSystolic > systolicMaxValue)
        {
            systolicMax.Invoke();
        }
        else if (BloodPressureSystolic < systolicMinValue || bpm == 0)
        {
            systolicMin.Invoke();
            valuesString = "--/";
        }
        else
        {
            valuesString = BloodPressureSystolic.ToString("F0") + "/";
        }
        //diastolic max and min events
        if (BloodPressureDiastolic > diastolicMaxValue)
        {
            diastolicMax.Invoke();
        }
        else if (BloodPressureDiastolic < diastolicMinValue || bpm == 0)
        {
            diastolicMin.Invoke();
            valuesString += "--";
        }
        else
        {
            valuesString += BloodPressureDiastolic.ToString("F0");
        }
        //mean max and min events
        if (((BloodPressureDiastolic + BloodPressureSystolic) / 2) > meanMaxValue)
        {
            meanMax.Invoke();
        }
        else if (((BloodPressureDiastolic + BloodPressureSystolic) / 2) < meanMinValue || bpm == 0)
        {
            meanMin.Invoke();
            valuesString += "\n(--)";
        }
        else
        {
            valuesString += "\n(" + ((BloodPressureDiastolic + BloodPressureSystolic) / 2).ToString("F0") + ")";
        }

        mean.text = valuesString;


        ///////this is the "other" section
        ///
        if (Time.time > nextActionTime)
        {
            nextActionTime += 60 / ABP;
            active = true;
        }


        //vibration value limit

        if (active == true)
        {
            active_time = Time.time;
            active = false;
        }

        if (Time.time - active_time < abp_line.length)
        {
            line_renderer.AddPoint(Time.time * compression, abp_line.Evaluate(((Time.time - active_time) * (compression + move_speed)) * vertical_scale));
        }
        else
        {
            line_renderer.AddPoint(Time.time * compression, 0);
        }
    }
}