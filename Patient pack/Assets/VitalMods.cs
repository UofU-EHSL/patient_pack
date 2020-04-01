using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Mod
{
    public bool useMod = false;
    public float modValue = 0f;
    public GameObject Vital;
    public List<vital_mod> mods = new List<vital_mod>(); 
}
public class VitalMods : MonoBehaviour
{
    
    [TextArea(15, 20)]
    public string notes;

    public GameObject Patient;

    [Header("SYSTOLIC")]
    public Mod Systolic;

    [Header("DIASTOLIC")]
    public Mod Diastolic;

    [Header("HEART RATE")]
    public Mod HeartRate;

    [Header("OXYGEN SATURATION")]
    public Mod OxygenSaturation;

    [Header("CAPNOGRAPHY")]
    public Mod Capnography;

    [Header("AIRWAY RESPIRATORY RATE")]
    public Mod Awrr;

    [Header("TEMPERATURE")]
    public Mod Temperature;

    [Header("BLOOD VOLUME")]
    public Mod BloodVolume;

    [Header("LUNG VOLUME")]
    public Mod LungVolume;


   
    public void AddTreatment(Treatment t)
    {
        
        foreach (vital_mod vm in t.vitalMods)
        {
            switch (vm.vitalToMod)
            {
                case vital_mod.vitalType.Awrr:
                    if (!Awrr.mods.Contains(vm))
                        Awrr.mods.Add(vm);
                    break;
                case vital_mod.vitalType.BloodVolume:
                    if (!BloodVolume.mods.Contains(vm))
                        BloodVolume.mods.Add(vm);
                    break;
                case vital_mod.vitalType.BP_diastolic:
                    if (!Diastolic.mods.Contains(vm))
                        Diastolic.mods.Add(vm);
                    break;
                case vital_mod.vitalType.BP_systolic:
                    if (!Systolic.mods.Contains(vm))
                        Systolic.mods.Add(vm);
                    break;
                case vital_mod.vitalType.CO2:
                    if (!Capnography.mods.Contains(vm))
                        Capnography.mods.Add(vm);
                    break;
                case vital_mod.vitalType.ecg:
                    if (!HeartRate.mods.Contains(vm))
                        HeartRate.mods.Add(vm);
                    break;
                case vital_mod.vitalType.LungVolume:
                    if (!LungVolume.mods.Contains(vm))
                        LungVolume.mods.Add(vm);
                    break;
                case vital_mod.vitalType.O2:
                    if (!OxygenSaturation.mods.Contains(vm))
                        OxygenSaturation.mods.Add(vm);
                    break;
                case vital_mod.vitalType.Temp:
                    if (!Temperature.mods.Contains(vm))
                        Temperature.mods.Add(vm);
                    break;
            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (Awrr.useMod)
        {
            float tempMod = 0;
            foreach(vital_mod vm in Awrr.mods)
            {
                tempMod += vm.CurrentValue;
            }
            airway_respiratory_rate vitalScript = Awrr.Vital.GetComponent<airway_respiratory_rate>();
            vitalScript.BreathsPerMinute = vitalScript.StartingBreathsPerMinute + tempMod;
        }

        if (BloodVolume.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in BloodVolume.mods)
            {
                tempMod += vm.CurrentValue;
            }
            blood_volume vitalScript = Awrr.Vital.GetComponent<blood_volume>();
            vitalScript.volume = vitalScript.StartingVolume + tempMod;
        }

        if (Systolic.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in Systolic.mods)
            {
                tempMod += vm.CurrentValue;
            }
            blood_pressure vitalScript = Awrr.Vital.GetComponent<blood_pressure>();
            vitalScript.BloodPressureSystolic = vitalScript.StartingSystolic + tempMod;
        }

        if (Diastolic.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in Diastolic.mods)
            {
                tempMod += vm.CurrentValue;
            }
            blood_pressure vitalScript = Awrr.Vital.GetComponent<blood_pressure>();
            vitalScript.BloodPressureDiastolic = vitalScript.StartingDiastolic + tempMod;
        }

        if (Capnography.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in Capnography.mods)
            {
                tempMod += vm.CurrentValue;
            }
            capnography vitalScript = Awrr.Vital.GetComponent<capnography>();
            vitalScript.co2 = vitalScript.StartingCO2 + tempMod;
        }

        if (OxygenSaturation.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in OxygenSaturation.mods)
            {
                tempMod += vm.CurrentValue;
            }
            oxygen_saturation vitalScript = Awrr.Vital.GetComponent<oxygen_saturation>();
            vitalScript.OxygenSaturation = vitalScript.StartingOxygentSaturation + tempMod;
        }

        if (LungVolume.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in LungVolume.mods)
            {
                tempMod += vm.CurrentValue;
            }
            lung_volume vitalScript = Awrr.Vital.GetComponent<lung_volume>();
            vitalScript.volume = vitalScript.StartingVolume + tempMod;
        }

        if (Temperature.useMod)
        {
            float tempMod = 0;
            foreach (vital_mod vm in Temperature.mods)
            {
                tempMod += vm.CurrentValue;
            }
            temperature vitalScript = Awrr.Vital.GetComponent<temperature>();
            vitalScript.temp = vitalScript.StartingTemp + tempMod;
        }
    }


}
