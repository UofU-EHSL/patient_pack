using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[System.Serializable]
public class vital
{
    public bool showVital;
    public float value;
    public Color color;
    public Color initColor;
    public float low_value;
    public float high_value;
    public AudioSource AudioSource;
    public AudioSource[] otherAudio;
    public GameObject backPlane;
    public AlarmType Alarm_level;
    public LineRenderer line_renderer;
    public patient_line_renderer custom_line_renderer;
    public TextMeshProUGUI[] text_items;

    public enum AlarmType
    {
        High,
        Medium,
        Low,
        None
    }
}

public static class globalPatient
{
    public static List<Treatment> treatments;
}

[System.Serializable]
public class vitalMod
{
    public string name;
    public GameObject vitalGameobject;
    public float Value_mod;
}


public class patient : MonoBehaviour
{
    public GameObject Vital_mods;
    public vital[] Vitals_visuals;
    public vitalMod[] mods;
    public List<Treatment> modItems;
    public Color NoColor;
    public Color LowColor;
    public Color MidColor;
    public Color HighColor;
    public AudioClip LowAudio;
    public AudioClip MidAudio;
    public AudioClip HighAudio;

    public void updateViz()
    {
        foreach (vital single in Vitals_visuals)
        {
            if (single.showVital == false)
            {
                single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = NoColor;
                if (single.AudioSource)
                {
                    single.AudioSource.clip = null;
                }
                foreach (AudioSource singleAudio in single.otherAudio)
                {
                    singleAudio.volume = 0;
                }
                if (single.line_renderer)
                {
                    single.line_renderer.endColor = NoColor;
                    single.line_renderer.startColor = NoColor;
                    single.custom_line_renderer.color = NoColor;
                }
                foreach (TextMeshProUGUI text in single.text_items)
                {
                    text.color = NoColor;
                }
            }
            else
            {
                foreach (AudioSource singleAudio in single.otherAudio)
                {
                    singleAudio.volume = 1.0f;
                }
                if (single.line_renderer)
                {
                    single.line_renderer.endColor = single.color;
                    single.line_renderer.startColor = single.color;
                    single.custom_line_renderer.color = single.color;
                }

                int newValue;
                Int32.TryParse(single.text_items[0].text, out newValue);
                single.value = (float)newValue;

                foreach (TextMeshProUGUI one in single.text_items)
                {
                    one.color = single.color;
                }


                //setting up alarms
                if (single.value >= single.high_value)
                {
                    if (single.Alarm_level == vital.AlarmType.High)
                    {
                        single.color = NoColor;
                        single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = HighColor;
                        single.AudioSource.clip = HighAudio;
                        if (single.AudioSource.isPlaying == false)
                        {
                            single.AudioSource.Play();
                            single.AudioSource.loop = true;
                        }
                    }
                    else if (single.Alarm_level == vital.AlarmType.Medium)
                    {
                        single.color = NoColor;
                        single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = MidColor;
                        single.AudioSource.clip = MidAudio;
                        if (single.AudioSource.isPlaying == false)
                        {
                            single.AudioSource.Play();
                            single.AudioSource.loop = true;
                        }
                    }
                    else if (single.Alarm_level == vital.AlarmType.Low)
                    {
                        single.color = NoColor;
                        single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = LowColor;
                        single.AudioSource.clip = LowAudio;
                        if (single.AudioSource.isPlaying == false)
                        {
                            single.AudioSource.Play();
                            single.AudioSource.loop = true;
                        }
                    }
                }
                else if (single.value <= single.low_value)
                {
                    if (single.Alarm_level == vital.AlarmType.High)
                    {
                        single.color = NoColor;
                        single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = HighColor;
                        single.AudioSource.clip = HighAudio;
                        if (single.AudioSource.isPlaying == false)
                        {
                            single.AudioSource.Play();
                            single.AudioSource.loop = true;
                        }
                    }
                    else if (single.Alarm_level == vital.AlarmType.Medium)
                    {
                        single.color = NoColor;
                        single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = MidColor;
                        single.AudioSource.clip = MidAudio;
                        if (single.AudioSource.isPlaying == false)
                        {
                            single.AudioSource.Play();
                            single.AudioSource.loop = true;
                        }
                    }
                    else if (single.Alarm_level == vital.AlarmType.Low)
                    {
                        single.color = NoColor;
                        single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = LowColor;
                        single.AudioSource.clip = LowAudio;
                        if (single.AudioSource.isPlaying == false)
                        {
                            single.AudioSource.Play();
                            single.AudioSource.loop = true;
                        }
                    }
                }
                else
                {
                    single.color = single.initColor;
                    single.backPlane.GetComponent<MeshRenderer>().sharedMaterial.color = NoColor;
                    single.AudioSource.clip = null;
                }
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

    public void showHide(int VitalsVisualsNumber)
    {
        Vitals_visuals[VitalsVisualsNumber].showVital = !Vitals_visuals[VitalsVisualsNumber].showVital;
    }

    public void Start()
    {
        foreach (vital single in Vitals_visuals)
        {
            single.initColor = single.color;
            Debug.Log("length:" + single.text_items[0].text.Length + " value:" + single.text_items[0].text + " vital:" + single.text_items[0].transform.parent.name);
        }
        updateViz();
    }
    public void Update()
    {
        globalPatient.treatments = new List<Treatment>(Vital_mods.GetComponentsInChildren<Treatment>());
        modItems = globalPatient.treatments;
        updateViz();
    }
}