using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow
{
    public int page = 0;
    Treatment myTarget;
    TreatmentScriptEditor Treatment;
    // Add menu named "My Window" to the Window menu
    [MenuItem("Patient Pack/New Treatment")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
    }

    void OnGUI()
    {

    }
}

[CustomEditor(typeof(Treatment))]
public class TreatmentScriptEditor : Editor
{
    public int page = 0;
    Treatment myTarget;
    private void OnEnable()
    {
        myTarget = (Treatment)target;
    }
    public override void OnInspectorGUI()
    {
        if (page == 0)
        {
            BasicInfo();
            if (GUILayout.Button("Next →"))
            {
                Next();
            }
        }
        else if (page == 1)
        {
            MenuOption();
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (GUILayout.Button("← Prev"))
                {
                    Prev();
                }
                if (GUILayout.Button("Next →"))
                {
                    Next();
                }
            GUILayout.EndHorizontal();
        }
        else if (page == 2)
        {
            vitalOptions();
            if (GUILayout.Button("← Prev"))
            {
                Prev();
            }
        }
    }
    // In-Editor function to add an AudioClip to the audioClips list.
    public void AddVital()
    {
        myTarget.vitalMods.Add(new vital_mod());
    }
    // In-Editor function to remove an AudioClip from the audioClips list.
    public void RemoveVital(int index)
    {
        myTarget.vitalMods.RemoveAt(index);
    }

    public void BasicInfo()
    {
        //Basic info
        GUILayout.Space(25);
        GUILayout.Label("Basic info", EditorStyles.boldLabel);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        myTarget.name = EditorGUILayout.TextField("Name: ", myTarget.name);
        // Treatment categories go here
        myTarget.chanceOfSuccess = EditorGUILayout.Slider("Success rate: ", myTarget.chanceOfSuccess, 0.0f, 100.0f);
        myTarget.required = EditorGUILayout.Toggle("This is required: ", myTarget.required);
        if (myTarget.required == true)
        {
            EditorGUI.indentLevel++;
            myTarget.preRequiredTreatment = EditorGUILayout.TextField("Pre-required treatment: ", myTarget.preRequiredTreatment);
            EditorGUI.indentLevel--;
        }
        myTarget.isBad = EditorGUILayout.Toggle("Shouldn't be used: ", myTarget.isBad);
        if (myTarget.isBad == true || myTarget.required == true)
        {
            GUILayout.Label("Debrief when they did it wrong:");
            myTarget.badString = EditorGUILayout.TextArea(myTarget.badString, GUILayout.MinHeight(50));
        }

        GUILayout.EndVertical();
        GUILayout.Space(25);
    }

    public void MenuOption()
    {
        //Menu options
        GUILayout.Label("Menu options", EditorStyles.boldLabel);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        myTarget.disableAfterUsed = EditorGUILayout.Toggle("Single-use action: ", myTarget.disableAfterUsed);
        if (myTarget.disableAfterUsed == false)
        {
            EditorGUI.indentLevel++;
            myTarget.TreatmentTimeout = EditorGUILayout.FloatField("Reset time: ", myTarget.TreatmentTimeout);
            EditorGUI.indentLevel--;
        }
        // doctor reset time
        myTarget.TimeItTakesDoctor = EditorGUILayout.FloatField("Time it takes the doctor (seconds): ", myTarget.TimeItTakesDoctor);
        EditorGUI.indentLevel++;
        if (myTarget.chanceOfSuccess > 0)
        {
            GUILayout.Label("Success caption:");
            myTarget.successCaption = EditorGUILayout.TextArea(myTarget.successCaption, GUILayout.MinHeight(30));
            myTarget.hasSuccessAudio = EditorGUILayout.Toggle("Has success audio: ", myTarget.hasSuccessAudio);
            if (myTarget.hasSuccessAudio == true)
            {
                EditorGUI.indentLevel++;
                myTarget.SuccessAudioClip = (AudioClip)EditorGUILayout.ObjectField(myTarget.SuccessAudioClip, typeof(AudioClip), true);
                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.indentLevel--;
        if (myTarget.chanceOfSuccess < 100)
        {
            GUILayout.Label("Fail caption:");
            myTarget.failCaption = EditorGUILayout.TextArea(myTarget.failCaption, GUILayout.MinHeight(30));
            myTarget.hasFailAudio = EditorGUILayout.Toggle("Has failed audio: ", myTarget.hasFailAudio);
            EditorGUI.indentLevel++;
            if (myTarget.hasFailAudio == true)
            {
                EditorGUI.indentLevel++;
                myTarget.FailAudioClip = (AudioClip)EditorGUILayout.ObjectField(myTarget.FailAudioClip, typeof(AudioClip), true);
                EditorGUI.indentLevel--;
            }
        }


        GUILayout.EndHorizontal();
        GUILayout.Space(25);
    }

    public void vitalOptions()
    {
        //Vital options
        GUILayout.Label("Vital options", EditorStyles.boldLabel);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        for (int count = 0; count < myTarget.vitalMods.Count; count++)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.Label(myTarget.vitalMods[count].vitalToMod.ToString());
            if (GUILayout.Button("Remove change"))
            {
                RemoveVital(count);
            }
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel++;
            EditorGUILayout.EnumPopup("Vital type: ", myTarget.vitalMods[count].vitalToMod);
            myTarget.vitalMods[count].vitalToMod = (myTarget.vitalMods[count].vitalToMod);

            myTarget.vitalMods[count].StartCurve = EditorGUILayout.CurveField("Start curve: ", myTarget.vitalMods[count].StartCurve);
            myTarget.vitalMods[count].EndCurve = EditorGUILayout.CurveField("End curve: ", myTarget.vitalMods[count].EndCurve);
            EditorGUI.indentLevel--;
            GUILayout.EndVertical();
        }
        GUILayout.Space(30);
        if (GUILayout.Button("Add Vital change"))
        {
            AddVital();
        }
        GUILayout.EndHorizontal();

        GUILayout.Label("Dev tools", EditorStyles.boldLabel);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Test option", EditorStyles.boldLabel);
        GUILayout.EndVertical();

        // Start of the notes section
        GUILayout.Label("Notes:", EditorStyles.boldLabel);
        myTarget.notes = EditorGUILayout.TextArea(myTarget.notes, GUILayout.MinHeight(100));
        // end of the notes section
    }

    public void Next()
    {
        page++;
    }

    public void Prev()
    {
        page--;
    }
}

