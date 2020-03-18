using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : MonoBehaviour
{

    [TextArea(15, 20)]
    public string notes;
    public vital_mod[] vitalMods;
    // Start is called before the first frame update
    public void NameUpdates()
    {
        foreach (vital_mod item in vitalMods)
        {
            item.name = item.vitalToMod.ToString();
        }
    }
}
