using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_library : MonoBehaviour
{
    public Condition[] children;

    public void updateChildren()
    {
        children = GetComponentsInChildren<Condition>();

        foreach (Condition child in children)
        {
            child.NameUpdates();
        }

    }
}
