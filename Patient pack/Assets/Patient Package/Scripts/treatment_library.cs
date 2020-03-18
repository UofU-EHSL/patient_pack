using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treatment_library : MonoBehaviour
{
    public Treatment[] children;

    public void updateChildren()
    {
        children = GetComponentsInChildren<Treatment>();

        foreach (Treatment child in children)
        {
            child.NameUpdates();
        }
        
    }
}