using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class blood_pressure : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    [Header("SYSTOLIC")]
    public TextMeshProUGUI systolicText;
    public float BloodPressureSystolic;
    public int systolicMinValue;
    public UnityEvent systolicMin;
    public int systolicMaxValue;
    public UnityEvent systolicMax;
    [HideInInspector]
    public float StartingSystolic;

    [Header("DIASTOLIC")]
    public TextMeshProUGUI diastolicText;
    public float BloodPressureDiastolic;
    public int diastolicMinValue;
    public UnityEvent diastolicMin;
    public int diastolicMaxValue;
    public UnityEvent diastolicMax;
    [HideInInspector]
    public float StartingDiastolic;

    [Header("MEAN")]
    public TextMeshProUGUI mean;
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

    [Header("DEATH")]
    public bool systolicDeath;
    public Vector2 systolicDeathValue;
    public float systolicDeathTime;
    public bool diastolicDeath;
    public Vector2 diastolicDeathValue;
    public float diastolicDeathTime;
    public UnityEvent death;

    private string valuesString = "";
    private float nextActionTime = 0.0f;
    private float active_time;


    private void Start()
    {
        StartingDiastolic = BloodPressureDiastolic;
        StartingSystolic = BloodPressureSystolic;
    }
    //other
    private void FixedUpdate()
    {
        if (systolicDeath)
        {
            if (BloodPressureSystolic <= systolicDeathValue.x || BloodPressureSystolic >= systolicDeathValue.y)
            {
                systolicDeathTime -= Time.deltaTime;
            }
        }
        if (diastolicDeath)
        {
            if (BloodPressureDiastolic <= diastolicDeathValue.x || BloodPressureDiastolic >= diastolicDeathValue.y)
            {
                systolicDeathTime -= Time.deltaTime;
            }
        }
        if (systolicDeathTime <= 0 || diastolicDeathTime <= 0)
        {
            death.Invoke();
        }

        float bpm = 60;
        //systolid max and min events
        if (BloodPressureSystolic > systolicMaxValue)
        {
            systolicMax.Invoke();
        }
        else if (BloodPressureSystolic < systolicMinValue || bpm == 0)
        {
            systolicMin.Invoke();
            systolicText.text = "--";
        }
        else
        {
            systolicText.text = BloodPressureSystolic.ToString("F0");
        }
        //diastolic max and min events
        if (BloodPressureDiastolic > diastolicMaxValue)
        {
            diastolicMax.Invoke();
        }
        else if (BloodPressureDiastolic < diastolicMinValue || bpm == 0)
        {
            diastolicMin.Invoke();
            diastolicText.text = "--";
        }
        else
        {
            diastolicText.text = BloodPressureDiastolic.ToString("F0");
        }
        //mean max and min events
        if (((BloodPressureDiastolic + BloodPressureSystolic) / 2) > meanMaxValue)
        {
            meanMax.Invoke();
        }
        else if (((BloodPressureDiastolic + BloodPressureSystolic) / 2) < meanMinValue || bpm == 0)
        {
            meanMin.Invoke();
            valuesString = "(--)";
        }
        else
        {
            valuesString = "(" + ((BloodPressureDiastolic + BloodPressureSystolic) / 2).ToString("F0") + ")";
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