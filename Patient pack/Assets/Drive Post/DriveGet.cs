using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

[System.Serializable]
public class removeBetween
{
    public string start;
    public string end;
}
public class DriveGet : MonoBehaviour
{
    public string getURL;
    [TextArea(20, 20)]
    public string data;
    public removeBetween[] regexFilter;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest(getURL));

        // A non-existing page.
        StartCoroutine(GetRequest("https://error.html"));
    }

    string RemoveBetween(string s, string begin, string end)
    {
        Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", begin, end));
        return regex.Replace(s, string.Empty);
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split();
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                data = webRequest.downloadHandler.text;
                foreach (removeBetween filter in regexFilter)
                {
                    data = RemoveBetween(data, filter.start, filter.end);
                }

                
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
