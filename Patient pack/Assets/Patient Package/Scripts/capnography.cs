using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class capnography : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    private float nextActionTime = 0.0f;

    [HideInInspector]
    public float StartingCO2;
    public float co2 = 1;
    public float baseCO2 = 1;
    public float vertical_scale = 1;
    public float compression;
    public float move_speed;
    public bool active;
    private float active_time;
    public float expiratie;
    public float inspiratie;

    //line rendered for making the real readout
    public patient_line_renderer line_renderer;
    public AnimationCurve co2_line;
    public TextMeshProUGUI digital_readout;
    public airway_respiratory_rate awrr;


    [Header("MIN AND MAX")]
    public float maxValue;
    public UnityEvent max;
    public float minValue;
    public UnityEvent min;

    

    // Start is called before the first frame update
    void Start()
    {
        StartingCO2 = co2;
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {
       


        if (awrr.BreathsPerMinute >= 1)
        {
            Keyframe[] keys = co2_line.keys; // Get a copy of the array
            keys[2].time = 30 / awrr.BreathsPerMinute;
            keys[3].time = 30 / awrr.BreathsPerMinute + .5f;
            keys[4].time = 60 / awrr.BreathsPerMinute;

            keys[0].value = co2 / 100;
            keys[1].value = vertical_scale;
            keys[2].value = vertical_scale;
            keys[3].value = co2 / 100;
            keys[4].value = co2 / 100;
            co2_line.keys = keys;
        }
        else
        {
            Keyframe[] keys = co2_line.keys; // Get a copy of the array
            //keys[1].time = .01f;
            keys[2].time = 30 / 15;
            keys[3].time = 15 / 15;
            keys[4].time = 60 / 15;

            keys[0].value = co2 / 100;
            keys[1].value = co2 / 100;
            keys[2].value = co2 / 100;
            keys[3].value = co2 / 100;
            keys[4].value = co2 / 100;
            co2_line.keys = keys;
        }

        if (co2 > maxValue)
        {
            max.Invoke();
        }
        else if (co2 < minValue)
        {
            min.Invoke();
        }

        digital_readout.text = Mathf.RoundToInt(co2).ToString("F0");
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
        //vibration value limit

        if (active == true)
        {
            active_time = Time.time;
            active = false;
        }

        if (Time.time - active_time < co2_line.length)
        {
            line_renderer.AddPoint((Time.time * compression), co2_line.Evaluate((Time.time - active_time) * (compression + move_speed)));
        }
        else
        {
            line_renderer.AddPoint(Time.time * compression, co2 / 50);
        }
    }
}
