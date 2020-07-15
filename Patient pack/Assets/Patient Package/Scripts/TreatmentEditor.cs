using UnityEngine;
using UnityEditor;

public class NewAssessment : EditorWindow
{
    public int page = 0;
    public GameObject assessment;
    Treatment TreatmentScript;
    TreatmentScriptEditor Treatment;
    // Add menu named "My Window" to the Window menu

    [MenuItem("Patient Pack/New Assessment")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        NewAssessment window = (NewAssessment)EditorWindow.GetWindow(typeof(NewAssessment));
        window.Show();
    }
    public void multiMedia()
    {
        TreatmentScript.hasMultiMedia = EditorGUILayout.Toggle("Use multi-media?", TreatmentScript.hasMultiMedia);
        GUILayout.BeginVertical(EditorStyles.helpBox);
        if (TreatmentScript.hasMultiMedia == true)
        {
            TreatmentScript.image = (Material)EditorGUILayout.ObjectField(TreatmentScript.image, typeof(Material), true);
            if (TreatmentScript.image != null)
            {
                TreatmentScript.size = EditorGUILayout.Vector2Field("Media size (cm): ", TreatmentScript.size);
            }
        }
        GUILayout.EndVertical();
    }
    void OnGUI()
    {
        Color defaultcolor = GUI.color;
        if (TreatmentScript == null)
        {
            assessment = new GameObject();
            assessment.AddComponent<Treatment>();
            TreatmentScript = (Treatment)assessment.GetComponent<Treatment>();

            TreatmentScript.notes = "";
            TreatmentScript.TreatmentCategory = new System.Collections.Generic.List<category>();
            TreatmentScript.chanceOfSuccess = 100;
            TreatmentScript.EnableModels = new System.Collections.Generic.List<GameObject>();
            TreatmentScript.DisableModels = new System.Collections.Generic.List<GameObject>();
            TreatmentScript.vitalMods = new System.Collections.Generic.List<vital_mod>();
        }
        else
        {
            TreatmentScript.name = EditorGUILayout.TextField("Name: ", TreatmentScript.name);
            TreatmentScript.required = EditorGUILayout.Toggle("required: ", TreatmentScript.required);
            GUILayout.Label("Text result");
            TreatmentScript.successCaption = EditorGUILayout.TextArea(TreatmentScript.successCaption, GUILayout.MinHeight(50));
            TreatmentScript.TimeItTakesDoctor = EditorGUILayout.FloatField("Time it takes doctor", TreatmentScript.TimeItTakesDoctor);
            TreatmentScript.hasSuccessAudio = EditorGUILayout.Toggle("Use audio", TreatmentScript.hasSuccessAudio);
            if (TreatmentScript.hasSuccessAudio == true)
            {
                EditorGUI.indentLevel++;
                TreatmentScript.SuccessAudioClip = (AudioClip)EditorGUILayout.ObjectField(TreatmentScript.SuccessAudioClip, typeof(AudioClip), true);
                EditorGUI.indentLevel--;
            }
            multiMedia();
            GUILayout.Label("Categories: ");
            GUILayout.BeginVertical(EditorStyles.helpBox);
            for (int count = 0; count < TreatmentScript.TreatmentCategory.Count; count++)
            {
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                TreatmentScript.TreatmentCategory[count] = (category)EditorGUILayout.EnumPopup(TreatmentScript.TreatmentCategory[count]);
                var style = new GUIStyle(GUI.skin.button);
                style.normal.textColor = Color.white;
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("Remove", style))
                {
                    RemoveCategory(count);
                }
                GUI.backgroundColor = defaultcolor;
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add category"))
            {
                AddCategory();
            }
            GUILayout.EndVertical();
        }

        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        if (GUILayout.Button("Create Assessment"))
        {
            Close();
        }
        if (GUILayout.Button("Close"))
        {
            DestroyImmediate(assessment.gameObject);
            Close();
        }
        GUILayout.EndHorizontal();
    }
    public void AddCategory()
    {
        if (TreatmentScript.TreatmentCategory.Count != 0)
        {
            TreatmentScript.TreatmentCategory.Add(TreatmentScript.TreatmentCategory[TreatmentScript.TreatmentCategory.Count - 1]);
        }
        else
        {
            TreatmentScript.TreatmentCategory.Add(new category());
        }
    }
    public void RemoveCategory(int index)
    {
        TreatmentScript.TreatmentCategory.RemoveAt(index);
    }
}

public class NewTreatment : EditorWindow
{
    public GameObject Treatment;
    Treatment TreatmentScript;

    // Add menu named "My Window" to the Window menu

    [MenuItem("Patient Pack/New Treatment")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        NewTreatment window = (NewTreatment)EditorWindow.GetWindow(typeof(NewTreatment));
        window.Show();
    }
    void OnGUI()
    {
        if (TreatmentScript == null)
        {
            Treatment = new GameObject();
            Treatment.AddComponent<Treatment>();
            TreatmentScript = (Treatment)Treatment.GetComponent<Treatment>();

            TreatmentScript.notes = "";
            TreatmentScript.TreatmentCategory = new System.Collections.Generic.List<category>();
            TreatmentScript.chanceOfSuccess = 100;
            TreatmentScript.EnableModels = new System.Collections.Generic.List<GameObject>();
            TreatmentScript.DisableModels = new System.Collections.Generic.List<GameObject>();
        }
        else
        {
            TreatmentScript.name = EditorGUILayout.TextField("Name: ", TreatmentScript.name);
        }

        GUILayout.BeginHorizontal(EditorStyles.helpBox);
        if (GUILayout.Button("Create Treatment"))
        {
            Close();
        }
        if (GUILayout.Button("Close"))
        {
            DestroyImmediate(Treatment.gameObject);
            Close();
        }
        GUILayout.EndHorizontal();
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
            GUILayout.Space(15);
            models();
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
        Color defaultColor = GUI.color;
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
        else
        {
            myTarget.isBad = EditorGUILayout.Toggle("Shouldn't be used: ", myTarget.isBad);
            if (myTarget.isBad == true || myTarget.required == true)
            {
                GUILayout.Label("Debrief when they did it wrong:");
                myTarget.badString = EditorGUILayout.TextArea(myTarget.badString, GUILayout.MinHeight(50));
            }
        }



        GUILayout.Label("Categories: ");
        GUILayout.BeginVertical(EditorStyles.helpBox);
        for (int count = 0; count < myTarget.TreatmentCategory.Count; count++)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            myTarget.TreatmentCategory[count] = (category)EditorGUILayout.EnumPopup(myTarget.TreatmentCategory[count]);
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.white;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove", style))
            {
                RemoveCategory(count);
            }
            GUI.backgroundColor = defaultColor;
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("Add category"))
        {
            AddCategory();
        }
        GUILayout.EndVertical();



        multiMedia();
        GUILayout.EndVertical();


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
        myTarget.hasMultiMedia = EditorGUILayout.Toggle("Use multi-media?", myTarget.hasMultiMedia);
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
    public void models()
    {
        Color defaultColor = GUI.color;
        GUILayout.BeginHorizontal(EditorStyles.helpBox);
            GUILayout.BeginVertical(EditorStyles.helpBox);
                headerStyle.fontSize = 15; //change the font size
                headerStyle.fontStyle = FontStyle.Bold;
                GUILayout.Label("Enable models: ", headerStyle);
                for (int count = 0; count < myTarget.EnableModels.Count; count++)
                {
                    GUILayout.BeginHorizontal(EditorStyles.helpBox);
                    myTarget.EnableModels[count] = (GameObject)EditorGUILayout.ObjectField(myTarget.EnableModels[count], typeof(GameObject), true);
                    var style = new GUIStyle(GUI.skin.button);
                    style.normal.textColor = Color.white;
                    GUI.backgroundColor = Color.red;
                    if (GUILayout.Button("Remove", style))
                        {
                            RemoveModelEnable(count);
                        }
                    GUI.backgroundColor = defaultColor;
                    GUILayout.EndHorizontal();
                }
                if (GUILayout.Button("Add model to enable"))
                {
                    AddModelEnable();
                }
            GUILayout.EndVertical();
        /////
            GUILayout.BeginVertical(EditorStyles.helpBox);
            headerStyle.fontSize = 15; //change the font size
            headerStyle.fontStyle = FontStyle.Bold;
            GUILayout.Label("Disable models: ", headerStyle);
            for (int count = 0; count < myTarget.DisableModels.Count; count++)
            {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            myTarget.DisableModels[count] = (GameObject)EditorGUILayout.ObjectField(myTarget.DisableModels[count], typeof(GameObject), true);
            var style = new GUIStyle(GUI.skin.button);
            style.normal.textColor = Color.white;
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Remove", style))
                {
                    RemoveModelDisable(count);
                }
            GUI.backgroundColor = defaultColor;
            GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add model to disable"))
            {
                AddModelDisable();
            }
            GUILayout.EndVertical();
        GUILayout.EndHorizontal();
    }
    public void RemoveModelEnable(int index)
    {
        myTarget.EnableModels.RemoveAt(index);
    }

    public void AddModelEnable()
    {
        if (myTarget.EnableModels.Count != 0)
        {
            myTarget.EnableModels.Add(myTarget.EnableModels[myTarget.EnableModels.Count - 1]);
        }
        else
        {
            myTarget.EnableModels.Add(myTarget.gameObject);
        }
    }
    public void AddCategory()
    {
        if (myTarget.TreatmentCategory.Count != 0)
        {
            myTarget.TreatmentCategory.Add(myTarget.TreatmentCategory[myTarget.TreatmentCategory.Count - 1]);
        }
        else
        {
            myTarget.TreatmentCategory.Add(new category());
        }
    }
    public void RemoveCategory(int index)
    {
        myTarget.TreatmentCategory.RemoveAt(index);
    }
    public void RemoveModelDisable(int index)
    {
        myTarget.DisableModels.RemoveAt(index);
    }
    public void AddModelDisable()
    {
        if (myTarget.DisableModels.Count != 0)
        {
            myTarget.DisableModels.Add(myTarget.DisableModels[myTarget.DisableModels.Count - 1]);
        }
        else
        {
            myTarget.DisableModels.Add(myTarget.gameObject);
        }
    }
}