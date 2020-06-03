using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[System.Serializable]
public class vital_mod
{
    public string name;
    public enum vitalType // your custom enumeration
    {
        BP_systolic,
        BP_diastolic,
        ecg,
        O2,
        CO2,
        Awrr,
        Temp,
        BloodVolume,
        LungVolume,
        none
    };

    public vitalType vitalToMod;
    public AnimationCurve StartCurve;
    public AnimationCurve EndCurve;

    [System.Serializable]
    public class issue
    {
        public string name;
        public vitalType vitalType;
    }

    public issue[] fixes_isses;
    public UnityEvent WhenApplied;
    public UnityEvent WhenFinished;
    public bool finished = false;
    public string TimeItTakes;

    [Header("Auto generated values")]
    public bool isActive = true;
    public float timeSinceTreatmentBegan;
    public float StartTime;
    public float CurrentValue;

    public void SetInactive()
    {
        //Adjusts the come down time speed. 
        Keyframe[] keys = EndCurve.keys;
        Debug.Log("SET INACTIVE: " + this.name);
        if (keys.Length > 0)
        {


            float start = keys[0].time;
            float end = keys[1].time;
            float diffTime = end - start;
            float max = keys[0].value;
            float min = keys[1].value;
            float diffValue = max - min;

            float percentage = CurrentValue / diffValue;

            keys[0].value = CurrentValue;
            keys[1].time = (int)(end * percentage);

            EndCurve.keys = keys;

        }

        timeSinceTreatmentBegan = 0;
        StartTime = Time.time;
        isActive = false;

    }

    public void SetActive()
    {
        isActive = true;
        timeSinceTreatmentBegan = 0;
        StartTime = Time.time;
    }
}


public class Treatment : MonoBehaviour
{
    public enum category // your custom enumeration
    {
        Start,
        Airway,
        Breathing,
        Circulation,
        Treatments,
        DiagnosticTest,
        PhysicalExam,
        Complete,
        Condition,
        none,
        debug
    };

    [TextArea(15, 20)]
    public string notes;
    public float TimeItTakesDoctor;

    [Header("Activation system")]
    public float TreatmentTimeout;
    public float TimeTillActive;
    public bool disableAfterUsed;

    public category TreatmentCategory;

    public int chanceOfSuccess = 100;
    [TextArea(3, 20)]
    public string failCaption;
    [TextArea(3, 20)]
    public string successCaption;
    private bool treatmentFailed = false;
    public vital_mod[] vitalMods;

    public GameObject doctor_manager;
    private GameObject active_doctor;

    private AudioSource audioClip;
    [TextArea(3, 20)]
    public string audioCaption;


    [Header("Multi-Media")]
    public Material image;
    public VideoClip videoClip;
    public Vector2 size;

    private void Start()
    {
        if (gameObject.transform.parent.name == "Vital mods")
        {
            TimeTillActive = TreatmentTimeout;

            if (this.gameObject.GetComponent<AudioSource>())
            {
                audioClip = this.gameObject.GetComponent<AudioSource>();
            }

            int rand = Random.Range(1, 101);
            if (rand <= chanceOfSuccess)
            {

                VitalMods mods = GameObject.FindGameObjectWithTag("Mod").GetComponent<VitalMods>();
                mods.AddTreatment(this);
                foreach (vital_mod vm in vitalMods)
                {
                    vm.SetActive();
                    if (vm.fixes_isses.Length > 0)
                    {
                        foreach (vital_mod.issue i in vm.fixes_isses)
                        {
                            //search through the vital mod's children then find the one with the name
                            //set the vm in that treatment to inactive. 
                            Treatment[] treatments = mods.gameObject.transform.GetComponentsInChildren<Treatment>();
                            foreach (Treatment t in treatments)
                            {
                                if (t.name.Contains(i.name))
                                {

                                    foreach (vital_mod v in t.vitalMods)
                                    {
                                        if (v.vitalToMod == i.vitalType)
                                        {
                                            Debug.Log("HERE");
                                            v.SetInactive();
                                        }
                                    }

                                }
                            }
                        }
                    }
                    vm.WhenApplied.Invoke();
                }
            }
            else
            {
                treatmentFailed = true;

            }
        }
    }

    public void SetCaption()
    {
        if (gameObject.transform.parent.name == "Vital mods" && successCaption.Length > 1)
        {
            audioCaption = successCaption;
        }
        else if (failCaption.Length > 1)
        {
            audioCaption = failCaption;
        }
    }

    public void NameUpdates()
    {
        foreach (vital_mod item in vitalMods)
        {
            item.name = item.vitalToMod.ToString();
        }
    }

    public void FixedUpdate()
    {

        if (gameObject.transform.parent.name == "Vital mods" && !treatmentFailed)
        {
            if (TimeTillActive > 0)
            {
                TimeTillActive = TimeTillActive - Time.deltaTime;
            }
            else
            {
                foreach (vital_mod vital in vitalMods)
                {
                    if (vital.finished == false && vital.isActive)
                    {
                        vital.WhenFinished.Invoke();
                        vital.finished = true;
                    }
                }
            }
            foreach (vital_mod vital in vitalMods)
            {
                if (vital.isActive)
                {
                    vital.timeSinceTreatmentBegan = Time.time - vital.StartTime;
                    vital.CurrentValue = vital.StartCurve.Evaluate(vital.timeSinceTreatmentBegan);
                    if (vital.StartCurve.keys.Length > 0 && vital.StartCurve.Evaluate(vital.timeSinceTreatmentBegan) == vital.StartCurve.keys[vital.StartCurve.keys.Length - 1].value)
                    {
                        //vital.SetInactive();
                    }
                }
                else if (vital.EndCurve != null)
                {
                    vital.timeSinceTreatmentBegan = Time.time - vital.StartTime;
                    vital.CurrentValue = vital.EndCurve.Evaluate(vital.timeSinceTreatmentBegan);
                }
            }
        }
        else
        {
            if (TimeTillActive > 0)
            {
                TimeTillActive = TimeTillActive - Time.deltaTime;
            }
        }
    }
}

