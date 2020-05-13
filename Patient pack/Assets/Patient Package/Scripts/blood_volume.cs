using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class blood_volume : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    [HideInInspector]
    public float StartingVolume;
    public float volume;
    public TextMeshProUGUI text;
    public int max;
    public UnityEvent maxEvent;
    public int min;
    public UnityEvent minEvent;
    // Update is called once per frame

    private void Start()
    {
        StartingVolume = volume;
    }
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
