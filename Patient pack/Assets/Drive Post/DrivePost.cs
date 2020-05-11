using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

[System.Serializable]
public class DataPoint
{
    public GameObject text;
    public string form_id;
    public string data;
}

public class DrivePost : MonoBehaviour
{
    public DataPoint[] item;
    public bool test;
    public void Submit()
    {
        foreach (DataPoint subData in item)
        {
            if (subData.text.GetComponent<Text>())
            {
                if (subData.text.GetComponent<Text>().text == "/")
                {
                    return;
                }
            }
        }
        StartCoroutine(Post());
    }

    public void Update()
    {
        if (test == true)
        {
            Submit();
            test = false;
        }
    }
    [SerializeField]
    public string BASE_URL;
    IEnumerator Post()
    {
        WWWForm form = new WWWForm();

        foreach (DataPoint thing in item)
        {
            if (thing.text.GetComponent<Text>())
            {
                thing.data = thing.text.GetComponent<Text>().text;
            }
            else if (thing.text.GetComponent<TextMeshProUGUI>())
            {
                thing.data = thing.text.GetComponent<TextMeshProUGUI>().text;
            }
            else
            {
                thing.data = thing.text.name;
            }
            form.AddField(thing.form_id, thing.data);
        }

        byte[] rawData = form.data;
        UnityWebRequest www = UnityWebRequest.Post(BASE_URL + "/formResponse", form);
        yield return www.SendWebRequest();
    }
}
