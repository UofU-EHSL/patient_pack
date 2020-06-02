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
            Awrr.modValue = 0;
            foreach(vital_mod vm in Awrr.mods)
            {
                Awrr.modValue += vm.CurrentValue;
            }
            airway_respiratory_rate vitalScript = Awrr.Vital.GetComponent<airway_respiratory_rate>();
            vitalScript.BreathsPerMinute = vitalScript.StartingBreathsPerMinute + Awrr.modValue;
        }

        if (BloodVolume.useMod)
        {
            BloodVolume.modValue = 0;
            foreach (vital_mod vm in BloodVolume.mods)
            {
                BloodVolume.modValue += vm.CurrentValue;
            }
            blood_volume vitalScript = BloodVolume.Vital.GetComponent<blood_volume>();
            vitalScript.volume = vitalScript.StartingVolume + BloodVolume.modValue;
        }

        if (Systolic.useMod)
        {
            Systolic.modValue = 0;
            foreach (vital_mod vm in Systolic.mods)
            {
                Systolic.modValue += vm.CurrentValue;
            }
            blood_pressure vitalScript = Systolic.Vital.GetComponent<blood_pressure>();
            vitalScript.BloodPressureSystolic = vitalScript.StartingSystolic + Systolic.modValue;
        }

        if (Diastolic.useMod)
        {
            Diastolic.modValue = 0;
            foreach (vital_mod vm in Diastolic.mods)
            {
                Diastolic.modValue += vm.CurrentValue;
            }
            blood_pressure vitalScript = Diastolic.Vital.GetComponent<blood_pressure>();
            vitalScript.BloodPressureDiastolic = vitalScript.StartingDiastolic + Diastolic.modValue;
        }

        if (HeartRate.useMod)
        {
            HeartRate.modValue = 0;
            foreach (vital_mod vm in HeartRate.mods)
            {
                HeartRate.modValue += vm.CurrentValue;
            }
            electrocardiogram vitalScript = HeartRate.Vital.GetComponent<electrocardiogram>();
            vitalScript.BeatsPerMinute = vitalScript.StartingBeatsPerMinute + HeartRate.modValue;
        }

        if (Capnography.useMod)
        {
            Capnography.modValue = 0;
            foreach (vital_mod vm in Capnography.mods)
            {
                Capnography.modValue += vm.CurrentValue;
            }
            capnography vitalScript = Capnography.Vital.GetComponent<capnography>();
            vitalScript.co2 = vitalScript.StartingCO2 + Capnography.modValue;
        }

        if (OxygenSaturation.useMod)
        {
            OxygenSaturation.modValue = 0;
            foreach (vital_mod vm in OxygenSaturation.mods)
            {
                OxygenSaturation.modValue += vm.CurrentValue;
            }
            oxygen_saturation vitalScript = OxygenSaturation.Vital.GetComponent<oxygen_saturation>();
            vitalScript.OxygenSaturation = vitalScript.StartingOxygentSaturation + OxygenSaturation.modValue;
        }

        if (LungVolume.useMod)
        {
            LungVolume.modValue = 0;
            foreach (vital_mod vm in LungVolume.mods)
            {
                LungVolume.modValue += vm.CurrentValue;
            }
            //lung_volume vitalScript = LungVolume.Vital.GetComponent<lung_volume>();
            //vitalScript.volume = vitalScript.StartingVolume + LungVolume.modValue;
        }

        if (Temperature.useMod)
        {
            Temperature.modValue = 0;
            foreach (vital_mod vm in Temperature.mods)
            {
                Temperature.modValue += vm.CurrentValue;
            }
            temperature vitalScript = Temperature.Vital.GetComponent<temperature>();
            vitalScript.temp = vitalScript.StartingTemp + Temperature.modValue;
        }
    }


}
