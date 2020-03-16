using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class vital
{
    public Color color;
    public LineRenderer line_renderer;  
    public patient_line_renderer custom_line_renderer;
    public Text[] text_items;
}

[ExecuteInEditMode]
public class patient : MonoBehaviour
{
    public vital[] Vitals_visuals;
    public electrocardiogram ecg;
    public oxygen_saturation o2;
    public capnography Co2;
    public airway_respiratory_rate awrr;
    public blood_pressure bp;
    public temperature temp;
    public blood_volume bloodVolume;

    public void updateViz()
    {
        foreach (vital single in Vitals_visuals)
        {
            single.line_renderer.endColor = single.color;
            single.line_renderer.startColor = single.color;
            single.custom_line_renderer.color = single.color;
            foreach (Text one in single.text_items)
            {
                one.color = single.color;
            }
        }
    }
    public void Start()
    {
        updateViz();
    }
    public void Update()
    {
    #if UNITY_EDITOR
        updateViz();
    #endif
    }
}