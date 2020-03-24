using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class oxygen_saturation : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    public float OxygenSaturation;
    public Text text;
    public int o2MinValue;
    public UnityEvent o2Min;
    public int o2MaxValue;
    public UnityEvent o2Max;
    ///
    ///
    /// 
    ///
    public patient_line_renderer line_renderer;
    public AnimationCurve spo2_line;
    public float value = 1;
    public float compression;
    public float move_speed;
    public bool active;
    private float nextActionTime = 0.0f;
    private float active_time;
    public airway_respiratory_rate awrr;


    void FixedUpdate()
    {


        if (OxygenSaturation >= o2MinValue)
        {
            Keyframe[] keys = spo2_line.keys; // Get a copy of the array
            keys[0].value = 0;
            keys[1].value = OxygenSaturation / 10;
            keys[2].value = (OxygenSaturation / 10) / 5;
            keys[3].value = (OxygenSaturation / 10) * .4f;
            keys[4].value = 0;
            keys[5].value = 0;
            keys[5].time = 60 / awrr.BreathsPerMinute;

            spo2_line.keys = keys;
        }
        else
        {
            Keyframe[] keys = spo2_line.keys; // Get a copy of the array
            keys[0].value = 0;
            keys[1].value = 0;
            keys[2].value = 0;
            keys[3].value = 0;
            keys[4].value = 0;
            keys[5].value = 0;
            keys[5].time = 5;

            spo2_line.keys = keys;
        }
        if (OxygenSaturation > o2MaxValue)
        {
            text.text = "100";
            o2Max.Invoke();
        }
        else if (OxygenSaturation < o2MinValue)
        {
            text.text = "--";
            o2Min.Invoke();
        }
        else
        {
            text.text = OxygenSaturation.ToString("F0");
        }


        if (Time.time > nextActionTime)
        {
            if (awrr.BreathsPerMinute <= 1)
            {
                nextActionTime += 1;
                active = true;
            }
            else
            {
                nextActionTime += 60 / awrr.BreathsPerMinute;
                active = true;
            }
        }
        if (active == true)
        {
            active_time = Time.time;
            active = false;
        }

        if (Time.time - active_time < spo2_line.length)
        {
            line_renderer.AddPoint(Time.time * compression, spo2_line.Evaluate((Time.time - active_time) * (compression + move_speed)));
        }
        else
        {
            line_renderer.AddPoint(Time.time * compression, 0);
        }
    }
}
