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
        if (gameObject.transform.parent.name == "Vital mods")
        {
            VitalMods mods = GameObject.FindGameObjectWithTag("Mod").GetComponent<VitalMods>();
            mods.AddTreatment(this);
            foreach (vital_mod vm in vitalMods)
            {
                vm.SetActive();
                if(vm.fixes_isses.Length > 0)
                {
                    foreach(vital_mod.issue i in vm.fixes_isses)
                    {
                        //search through the vital mod's children then find the one with the name
                        //set the vm in that treatment to inactive. 
                        Treatment[] treatments = mods.gameObject.transform.GetComponentsInChildren<Treatment>();
                        foreach(Treatment t in treatments)
                        {
                            if (t.name.Contains(i.name)){
                                
                                foreach(vital_mod v in t.vitalMods)
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
            }
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
                        //vital.SetInactive();
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
