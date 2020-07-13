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
        Color defaultColor = GUI.color;
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.white;
        style.hover.textColor = Color.white;
        
        if (page == 0)
        {
            BasicInfo();
            GUI.backgroundColor = Color.black;
                if (GUILayout.Button("Menu options →", style))
                {
                    Page(1);
                }
            GUI.backgroundColor = defaultColor;
        }
        else if (page == 1)
        {
            MenuOption();
            GUI.backgroundColor = Color.black;
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (GUILayout.Button("← Basic info", style))
                {
                    Page(0);
                }
                if (GUILayout.Button("Vital options →", style))
                {
                    Page(2);
                }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = defaultColor;
        }
        else if (page == 2)
        {
            vitalOptions();
            GUI.backgroundColor = Color.black;
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (GUILayout.Button("← Menu options", style))
                {
                    Page(1);
                }
                if (GUILayout.Button("Dev tools →", style))
                {
                    Page(3);
                }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = defaultColor;
        }
        else if (page == 3)
        {
            DevOptions();
            GUI.backgroundColor = Color.black;
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (GUILayout.Button("← Vital options", style))
                {
                    Page(2);
                }
                if (GUILayout.Button("Basic info →", style))
                {
                    Page(0);
                }
            GUILayout.EndHorizontal();
            GUI.backgroundColor = defaultColor;
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

    // In-Editor function to add an AudioClip to the audioClips list.
    public void AddVitalIssue(int vital)
    {
        if (myTarget.vitalMods[vital].fixes_issues == null)
        {
            myTarget.vitalMods[vital].fixes_issues = new System.Collections.Generic.List<vital_mod.issue>();
        }
        myTarget.vitalMods[vital].fixes_issues.Add(new vital_mod.issue());
    }
    // In-Editor function to remove an AudioClip from the audioClips list.
    public void RemoveVitalIssue(int vital, int issue)
    {
        myTarget.vitalMods[vital].fixes_issues.RemoveAt(issue);
    }

    private GUIStyle headerStyle = new GUIStyle();
    public void BasicInfo()
    {
        //Basic info
        headerStyle.fontSize = 20; //change the font size
        headerStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Basic info", headerStyle);
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
        multiMedia();
    }

    public void MenuOption()
    {
        //Menu options
        headerStyle.fontSize = 20; //change the font size
        headerStyle.fontStyle = FontStyle.Bold;

        GUILayout.Label("Menu options", headerStyle);
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
        EditorGUI.indentLevel++;
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
        EditorGUI.indentLevel--;

        GUILayout.EndHorizontal();
        GUILayout.Space(25);
    }

    public void vitalOptions()
    {
        Color defaultColor = GUI.color;
        headerStyle.fontSize = 20; //change the font size
        headerStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Vital options", headerStyle);

        GUILayout.BeginVertical(EditorStyles.helpBox);
        if (myTarget.vitalMods != null)
        {
            for (int count = 0; count < myTarget.vitalMods.Count; count++)
            {

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Label(myTarget.vitalMods[count].vitalToMod.ToString());

                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.white;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove change", style))
                    {
                        RemoveVital(count);
                    }
                GUI.backgroundColor = defaultColor;
                GUILayout.EndHorizontal();

                EditorGUI.indentLevel++;

                myTarget.vitalMods[count].vitalToMod = (vital_mod.vitalType)EditorGUILayout.EnumPopup("Vital type: ", myTarget.vitalMods[count].vitalToMod);

                if (myTarget.vitalMods[count].StartCurve == null)
                {
                    myTarget.vitalMods[count].StartCurve = new AnimationCurve();
                }
                if (myTarget.vitalMods[count].EndCurve == null)
                {
                    myTarget.vitalMods[count].EndCurve = new AnimationCurve();
                }
                myTarget.vitalMods[count].StartCurve = EditorGUILayout.CurveField("Start curve: ", myTarget.vitalMods[count].StartCurve);
                myTarget.vitalMods[count].EndCurve = EditorGUILayout.CurveField("End curve: ", myTarget.vitalMods[count].EndCurve);


                EditorGUI.indentLevel--;
                GUILayout.EndVertical();
            }
        }
        GUILayout.Space(15);

        if (GUILayout.Button("Add Vital change"))
        {
            AddVital();
        }
        GUILayout.EndVertical();

        GUILayout.Space(30);
        GUILayout.Label("Fixes vital from issue", headerStyle);
        if (myTarget.vitalMods.Count > 0)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            if (myTarget.vitalMods[0].fixes_issues != null)
            {
                for (int count2 = 0; count2 < myTarget.vitalMods[0].fixes_issues.Count; count2++)
                {

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.indentLevel++;
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    //GUILayout.Label("Instantly fixes " + myTarget.vitalMods[count].fixes_issues[count2].name.ToString());


                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    //GUILayout.Label(myTarget.vitalMods[0].fixes_issues[count2].name.ToString());

                    var style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.white;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove instant fix to an issue", style))
                        {
                            RemoveVitalIssue(0, count2);
                        }
                    GUILayout.EndHorizontal();
                    GUI.backgroundColor = defaultColor;
                    GUILayout.EndHorizontal();
                    myTarget.vitalMods[0].fixes_issues[count2].name = EditorGUILayout.TextField("Issue name: ", myTarget.vitalMods[0].fixes_issues[count2].name);
                    myTarget.vitalMods[0].fixes_issues[count2].vitalType = (vital_mod.vitalType)EditorGUILayout.EnumPopup("Vital type: ", myTarget.vitalMods[0].fixes_issues[count2].vitalType);

                    EditorGUI.indentLevel--;
                    GUILayout.EndVertical();

                }
            }
            GUILayout.Space(15);
            if (GUILayout.Button("Add instant fix to an issue"))
            {
                AddVitalIssue(0);
            }

            GUILayout.EndVertical();
        }


        GUI.backgroundColor = defaultColor;
        GUI.color = defaultColor;

        GUILayout.Space(15);
    }

    public void DevOptions()
    {
        GUILayout.Label("Dev tools", EditorStyles.boldLabel);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Test option", EditorStyles.boldLabel);
        GUILayout.EndVertical();

        // Start of the notes section
        GUILayout.Label("Notes:", EditorStyles.boldLabel);
        myTarget.notes = EditorGUILayout.TextArea(myTarget.notes, GUILayout.MinHeight(100));
        // end of the notes section
    }

    public void multiMedia()
    {
        myTarget.hasMultiMedia = EditorGUILayout.Toggle("Use ulti-media?", myTarget.hasMultiMedia);
        GUILayout.BeginVertical(EditorStyles.helpBox);
            if (myTarget.hasMultiMedia == true)
            {
                myTarget.image = (Material)EditorGUILayout.ObjectField(myTarget.image, typeof(Material), true);
                if (myTarget.image != null)
                {
                    myTarget.size = EditorGUILayout.Vector2Field("Media size (cm): ", myTarget.size);
                }
            }
        GUILayout.EndVertical();
    }

    public void Page(int GoToPage)
    {
        page = GoToPage;
    }
}