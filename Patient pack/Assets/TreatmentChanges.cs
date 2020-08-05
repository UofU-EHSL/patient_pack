using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.Events;

[Serializable]
public class Statement
{
    public bool satisfied;
    public string TreatmentOrCondition;
    public enum usability { Used, NotUsed }
    public usability Types;
}



public class TreatmentChanges : MonoBehaviour
{
    public List<Statement> statements;

    [SerializeField]
    public UnityEvent doThis;

    private void Start()
    {
        for (int i = 0; i < statements.Count; ++i)
        {
            if (statements[i].Types == Statement.usability.NotUsed)
            {
                if (globalPatient.treatmentsString.Contains(statements[i].TreatmentOrCondition))
                {
                    statements[i].satisfied = false;
                    return;
                }
                else
                {
                    statements[i].satisfied = true;
                }
            }
            else if (statements[i].Types == Statement.usability.Used)
            {
                if (globalPatient.treatmentsString.Contains(statements[i].TreatmentOrCondition))
                {
                    statements[i].satisfied = true;
                }
                else
                {
                    statements[i].satisfied = false;
                    return;
                }
            }
        }
        if (statements[statements.Count].satisfied == true)
        {
            doThis.Invoke();
        }
    }
}


[CustomEditor(typeof(TreatmentChanges))]
public class TreatmentChangesEditor : Editor
{
    TreatmentChanges myTarget;
    private void OnEnable()
    {
        myTarget = (TreatmentChanges)target;
    }

    
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Categories: ");
        GUILayout.BeginVertical();
        for (int count = 0; count < myTarget.statements.Count; count++)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("x"))
            {
                myTarget.statements.RemoveAt(count);
            }
            GUILayout.Label("If ");
            myTarget.statements[count].TreatmentOrCondition = EditorGUILayout.TextField(myTarget.statements[count].TreatmentOrCondition);
            GUILayout.Label(" is ");
            myTarget.statements[count].Types = (Statement.usability)EditorGUILayout.EnumPopup(myTarget.statements[count].Types);
            if (count == myTarget.statements.Count-1)
            {
                GUILayout.Label(" then ");
            }
            else
            {
                GUILayout.Label(" and ");
            }
            GUILayout.EndHorizontal();
        }
        myTarget.doThis = EditorGUILayout.PropertyField(myTarget.doThis);
        if (GUILayout.Button("Add category"))
        {
            myTarget.statements.Add(new Statement());
        }
        GUILayout.EndVertical();
    }
}