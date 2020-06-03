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
    [Header("DEATH")]
    public bool Death;
    public float DeathValue;
    public float DeathTime;
    public UnityEvent death;

    private void Start()
    {
        StartingVolume = volume;
    }
    void Update()
    {
        if (Death && volume <= DeathValue)
        {
            DeathTime -= Time.deltaTime;
        }
        if (DeathTime <= 0)
        {
            death.Invoke();
        }
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
