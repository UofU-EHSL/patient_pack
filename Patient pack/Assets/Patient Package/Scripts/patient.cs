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
    public GameObject Vital_mods;
    public Treatment[] treatments;
    public vital[] Vitals_visuals;
    public electrocardiogram ecg;
    public float ecg_mod;
    public oxygen_saturation o2;
    public float o2_mod;
    public capnography Co2;
    public float Co2_mod;
    public airway_respiratory_rate awrr;
    public float awrr_mod;
    public blood_pressure bp;
    public float bp_systolic_mod;
    public float bp_dyastolic_mod;
    public temperature temp;
    public float temp_mod;
    public blood_volume bloodVolume;
    public float bloodVolume_mod;

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
        treatments = GetComponentsInChildren<Treatment>();
    }
}