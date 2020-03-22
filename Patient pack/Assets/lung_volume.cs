using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class lung_volume : MonoBehaviour
{
    public float volume;
    public Text text;
    public int max;
    public UnityEvent maxEvent;
    public int min;
    public UnityEvent minEvent;
    // Update is called once per frame

    void Update()
    {
        text.text = volume.ToString("F0");

        if (volume > max)
        {
            maxEvent.Invoke();
        }
        else if (volume < min)
        {
            minEvent.Invoke();
        }
    }
}
