using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class airway_respiratory_rate : MonoBehaviour
{
    [TextArea(15, 20)]
    public string notes;

    public bool isBreathing;
    [HideInInspector]
    public float StartingBreathsPerMinute;
    public float BreathsPerMinute;
    public TextMeshProUGUI text;
    public int max;
    public UnityEvent maxEvent;
    public int min;
    public UnityEvent minEvent;
    // Update is called once per frame
    private void Start()
    {
        StartingBreathsPerMinute = BreathsPerMinute;
    }
    void Update()
    {
        text.text = BreathsPerMinute.ToString("F0");

        if (BreathsPerMinute > max)
        {
            maxEvent.Invoke();
        }
        else if (BreathsPerMinute < min)
        {
            minEvent.Invoke();
        }


        /*float temp_mod = 0;

        Component[] affects = GetComponent<patient>().treatments;
        foreach (Component treatment in affects)
        {
            if (treatment.GetComponent<awrr_mod>())
            {
                temp_mod += treatment.GetComponent<awrr_mod>().valueChange;
            }
        }

        BreathsPerMinute = temp_breathsPerMinute + temp_mod;
        */
    }
}
