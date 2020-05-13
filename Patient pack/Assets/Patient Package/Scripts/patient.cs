using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[System.Serializable]
public class vital
{
    public Color color;
    public LineRenderer line_renderer;
    public patient_line_renderer custom_line_renderer;
    public TextMeshProUGUI[] text_items;
}

[System.Serializable]
public class vitalMod
{
    public string name;
    public GameObject vitalGameobject;
    public float Value_mod;
}

[ExecuteInEditMode]
public class patient : MonoBehaviour
{
    public GameObject Vital_mods;
    public Treatment[] treatments;
    public vital[] Vitals_visuals;
    public vitalMod[] mods;

    public void updateViz()
    {
        foreach (vital single in Vitals_visuals)
        {
            single.line_renderer.endColor = single.color;
            single.line_renderer.startColor = single.color;
            single.custom_line_renderer.color = single.color;
            foreach (TextMeshProUGUI one in single.text_items)
            {
                one.color = single.color;
            }
        }

        foreach (vitalMod single in mods)
        {
            single.name = single.vitalGameobject.name;
        }
    }

    public void updateMods()
    {

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