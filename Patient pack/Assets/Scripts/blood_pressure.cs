using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class blood_pressure : MonoBehaviour
{
    [Header("SYSTOLIC")]
    public float BloodPressureSystolic;
    public int systolicMinValue;
    public UnityEvent systolicMin;
    public int systolicMaxValue;
    public UnityEvent systolicMax;
    public Text systolicText;
    [Header("DIASTOLIC")]
    public float BloodPressureDiastolic;
    public int diastolicMinValue;
    public UnityEvent diastolicMin;
    public int diastolicMaxValue;
    public UnityEvent diastolicMax;
    public Text diastolicText;
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

    //other
    private void Update()
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
            systolicText.text = "--/";
        }
        else
        {
            systolicText.text = BloodPressureSystolic.ToString("F0") + "/";
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
            mean.text = "(--)";
        }
        else
        {
            mean.text = "(" + ((BloodPressureDiastolic + BloodPressureSystolic) / 2).ToString("F0") + ")";
        }




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