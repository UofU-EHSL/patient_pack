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

    void Update()
    {
        int i = 0;
        foreach (Statement item in statements)
        {
            if (globalPatient.treatmentsString.Contains(item.TreatmentOrCondition))
            {
                statements[i].satisfied = true;
            }
            else
            {
                statements[i].satisfied = false;
            }
            i++;
        }
    }
}
