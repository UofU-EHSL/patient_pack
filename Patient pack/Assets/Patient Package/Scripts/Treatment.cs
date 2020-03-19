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
    public float CurrentValue;
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

    public void NameUpdates()
    {
        foreach (vital_mod item in vitalMods)
        {
            item.name = item.vitalToMod.ToString();
        }
    }

    public void Update()
    {
        if (gameObject.transform.name == "Vital mods")
        {
            foreach (vital_mod vital in vitalMods)
            {
                if (vital.isActive)
                {
                    vital.CurrentValue = vital.StartCurve.Evaluate(vital.timeSinceTreatmentBegan);
                }
            }
        }
    }
}
