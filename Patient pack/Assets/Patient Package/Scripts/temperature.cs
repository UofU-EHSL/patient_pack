using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class temperature : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;
    [HideInInspector]
    public float StartingTemp;
    public float temp;
    public TextMeshProUGUI text;
    public int max;
    public UnityEvent maxEvent;
    public int min;
    public UnityEvent minEvent;
    // Update is called once per frame

    private void Start()
    {
        StartingTemp = temp;
    }
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
