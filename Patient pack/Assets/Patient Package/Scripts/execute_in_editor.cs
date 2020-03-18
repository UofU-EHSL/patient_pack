using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class execute_in_editor : MonoBehaviour
{
    public UnityEvent things;

    private void Start()
    {
        things.Invoke();
    }
    void Update()
    {
#if UNITY_EDITOR
        things.Invoke();
#endif
    }
}
