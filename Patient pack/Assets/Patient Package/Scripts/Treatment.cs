using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        LungVolume
    };

    public vitalType vitalToMod;
    public AnimationCurve StartCurve;
    public AnimationCurve EndCurve;
    public string[] fixes_isses;
    public UnityEvent WhenApplied;
    public UnityEvent WhenFinished;
    public string TimeItTakes;

    [Header("Auto generated values")]
    public bool isActive = true;
    public float timeSinceTreatmentBegan;
    public float StartTime;
    public float CurrentValue;

    public void SetInactive()
    {
        isActive = false;
        timeSinceTreatmentBegan = 0;
        StartTime = Time.time;
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
        Treatmetns,
        DiagnosticTest,
        PhysicalExam,
        Complete
    };

    [TextArea(15, 20)]
    public string notes;
    public category TreatmentCategory;

    public vital_mod[] vitalMods;

    public GameObject doctor_manager;
    private GameObject active_doctor;

    private void Start()
    {
        VitalMods mods = GameObject.FindGameObjectWithTag("Mod").GetComponent<VitalMods>();
        mods.AddTreatment(this);
        foreach(vital_mod vm in vitalMods)
        {
            vm.SetActive();
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
        if (gameObject.transform.parent.name == "Vital mods")
        {
            
            foreach (vital_mod vital in vitalMods)
            {
                if (vital.isActive)
                {
                    vital.timeSinceTreatmentBegan = Time.time - vital.StartTime;
                    vital.CurrentValue = vital.StartCurve.Evaluate(vital.timeSinceTreatmentBegan);
                    if(vital.StartCurve.Evaluate(vital.timeSinceTreatmentBegan) == vital.StartCurve.keys[vital.StartCurve.keys.Length-1].value)
                    {
                        vital.SetInactive();
                    }
                }
                else
                {
                    vital.timeSinceTreatmentBegan = Time.time - vital.StartTime;
                    vital.CurrentValue = vital.EndCurve.Evaluate(vital.timeSinceTreatmentBegan);
                }
            }
        }
    }
}
