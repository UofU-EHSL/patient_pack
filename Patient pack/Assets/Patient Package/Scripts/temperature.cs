using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;

public class temperature : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    public float temp;
    public Text text;
    public int max;
    public UnityEvent maxEvent;
    public int min;
    public UnityEvent minEvent;
    // Update is called once per frame
    
    void Update()
    {
        text.text = temp.ToString("F0");

        if (temp> max)
        {
            maxEvent.Invoke();
        }
        else if (temp < min)
        {
            minEvent.Invoke();
        }


        
    }
}
