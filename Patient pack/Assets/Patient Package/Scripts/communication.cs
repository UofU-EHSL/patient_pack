using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class communication : MonoBehaviour
{
    [Header("Text communication")]
    public bool display_text;
    public GameObject doctor_talk_text;
    [TextArea(15, 20)]
    public string text;

    [Header("Audio communication")]
    public bool play_audio;
    public AudioClip audio_clip;

    [Header("Other information")]
    public doctor_manager doctor_manager;
    private GameObject active_doctor;

    public void audioPlayer()
    {
        active_doctor.GetComponent<AudioSource>().clip = audio_clip;
        active_doctor.GetComponent<AudioSource>().Play();
    }

    public void Update()
    {
        active_doctor = doctor_manager.gameObject;
    }
}
