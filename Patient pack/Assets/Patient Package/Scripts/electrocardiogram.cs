using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class electrocardiogram : MonoBehaviour
{
    private Color color;
    public LineRenderer line_renderer;
    public patient_line_renderer custom_line_renderer;
    public Text[] text_items;
    // Start is called before the first frame update
    void Start()
    {
        line_renderer.endColor = color;
        line_renderer.startColor = color;
        custom_line_renderer.color = color;
        foreach (Text single in text_items)
        {
            single.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
