using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doctor_manager : MonoBehaviour
{
    public int number_of_doctors;
    public GameObject single_doctor;

    public GameObject active_doctor;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < number_of_doctors; i++)
        {
            Instantiate(single_doctor, this.gameObject.transform);
        }
        Destroy(single_doctor);
    }
}
