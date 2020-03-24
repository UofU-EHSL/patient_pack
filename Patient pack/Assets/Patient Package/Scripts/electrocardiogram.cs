using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class electrocardiogram : MonoBehaviour
{
    /* private Color color;
     public LineRenderer line_renderer;
     public patient_line_renderer custom_line_renderer;
     public Text[] text_items;
     // Start is called before the first frame update
     void Start()
     {
         line_renderer.endColor = color;
         line_renderer.startColor = color;
         custom_line_renderer.color = color;
         foreach (Text single in text_items)
         {
             single.color = color;
         }
     }
     */
    [TextArea(15, 20)]
    public string notes;

    [Header("ECG")]
    public Text bpmText;
    public int bpmMinValue;
    public UnityEvent bpmMin;
    public int bpmMaxValue;
    public UnityEvent bpmMax;
    [Header("LINE RENDERER")]
    // none of this is used yet
    public patient_line_renderer line_renderer;
    public AnimationCurve bpm_line;
    public float ABP = 1;
    public float vertical_scale;
    public float compression;
    public float move_speed;
    public bool active;
    private float nextActionTime = 0.0f;
    private float active_time;

    public float PWave_height;
    public float Q_depth;
    public float R_height;
    public float S_depth;
    public float STSegment;
    public float TWave;
    public float temp_wait;
    //other
    private void FixedUpdate()
    {
        float bpm = 60;
        bpmText.text = bpm.ToString();


        Keyframe[] keys = bpm_line.keys;
        keys[1].value = PWave_height;
        keys[4].value = Q_depth;
        keys[5].value = R_height;
        keys[6].value = S_depth;
        keys[8].time = STSegment;
        keys[9].value = TWave;
        //keys[10].time = temp_wait;

        bpm_line.keys = keys;


       
        


        if (Time.time > nextActionTime)
        {
            if (bpm <= bpmMinValue)
            {
                nextActionTime += 1 / bpmMinValue;
                active = true;
            }
            else
            {
                nextActionTime += 60 / bpm;
                active = true;
            }
        }

        //vibration value limit
        if (active == true)
        {
            active_time = Time.time;
            active = false;
        }
        if (Time.time - active_time < bpm_line.length)
        {
            line_renderer.AddPoint(Time.time * compression, bpm_line.Evaluate((Time.time - active_time) * (compression + move_speed)));
           
        }
        else
        {
            line_renderer.AddPoint(Time.time * compression, 0);
        }
    }
}
