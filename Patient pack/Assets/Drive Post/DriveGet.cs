using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

public class DriveGet : MonoBehaviour
{
    public string getURL;
    [TextArea(20, 20)]
    public string data;
    void Start()
    {
        // A correct website page.
        StartCoroutine(GetRequest(getURL));

        // A non-existing page.
        StartCoroutine(GetRequest("https://error.html"));
    }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split();
            int page = pages.Length - 1;

            
            string input = pages[page];
            string regex = "(\\<.*\\>)|(\".*\")|('.*')|(\\(.*\\))";
            string output = Regex.Replace(input, regex, "");
            pages[page] = input;
            

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                data = webRequest.downloadHandler.text;
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
}
