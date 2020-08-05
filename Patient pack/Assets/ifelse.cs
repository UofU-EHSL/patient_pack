using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct Statement
{
    public bool satisfied;
    public string TreatmentOrCondition;
    public enum type {AND, OR, NOT, none}
    public type Type;
}

public class ifelse : MonoBehaviour
{
    public Statement[] statements;
    public patient patientScript;
    public List<Treatment> builtList;

    // Start is called before the first frame update
    void Start()
    {
        builtList = globalPatient.treatments;
    }

    // Update is called once per frame
    void Update()
    {
        builtList = globalPatient.treatments;
    }
}
