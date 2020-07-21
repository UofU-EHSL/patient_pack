using UnityEngine;
using UnityEditor;

public class NewTreatment : EditorWindow
{
    public int page = 0;
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
            TreatmentScript.vitalMods = new System.Collections.Generic.List<vital_mod>();
        }
        else
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
                GUILayout.BeginHorizontal();
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
                GUILayout.BeginHorizontal();
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
                GUILayout.BeginHorizontal();
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





    public void AddVital()
    {
        TreatmentScript.vitalMods.Add(new vital_mod());
    }
    // In-Editor function to remove an AudioClip from the audioClips list.
    public void RemoveVital(int index)
    {
        TreatmentScript.vitalMods.RemoveAt(index);
    }

    // In-Editor function to add an AudioClip to the audioClips list.
    public void AddVitalIssue(int vital)
    {
        if (TreatmentScript.vitalMods[vital].fixes_issues == null)
        {
            TreatmentScript.vitalMods[vital].fixes_issues = new System.Collections.Generic.List<vital_mod.issue>();
        }
        TreatmentScript.vitalMods[vital].fixes_issues.Add(new vital_mod.issue());
    }
    // In-Editor function to remove an AudioClip from the audioClips list.
    public void RemoveVitalIssue(int vital, int issue)
    {
        TreatmentScript.vitalMods[vital].fixes_issues.RemoveAt(issue);
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
        TreatmentScript.name = EditorGUILayout.TextField("Name: ", TreatmentScript.name);
        // Treatment categories go here
        TreatmentScript.chanceOfSuccess = EditorGUILayout.Slider("Success rate: ", TreatmentScript.chanceOfSuccess, 0.0f, 100.0f);
        TreatmentScript.required = EditorGUILayout.Toggle("This is required: ", TreatmentScript.required);
        if (TreatmentScript.required == true)
        {
            EditorGUI.indentLevel++;
            TreatmentScript.preRequiredTreatment = EditorGUILayout.TextField("Pre-required treatment: ", TreatmentScript.preRequiredTreatment);
            EditorGUI.indentLevel--;
        }
        else
        {
            TreatmentScript.isBad = EditorGUILayout.Toggle("Shouldn't be used: ", TreatmentScript.isBad);
            if (TreatmentScript.isBad == true || TreatmentScript.required == true)
            {
                GUILayout.Label("Debrief when they did it wrong:");
                TreatmentScript.badString = EditorGUILayout.TextArea(TreatmentScript.badString, GUILayout.MinHeight(50));
            }
        }



        GUILayout.Label("Categories: ");
        GUILayout.BeginVertical();
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
        TreatmentScript.disableAfterUsed = EditorGUILayout.Toggle("Single-use action: ", TreatmentScript.disableAfterUsed);
        if (TreatmentScript.disableAfterUsed == false)
        {
            EditorGUI.indentLevel++;
            TreatmentScript.TreatmentTimeout = EditorGUILayout.FloatField("Reset time: ", TreatmentScript.TreatmentTimeout);
            EditorGUI.indentLevel--;
        }
        // doctor reset time
        TreatmentScript.TimeItTakesDoctor = EditorGUILayout.FloatField("Time it takes the doctor (seconds): ", TreatmentScript.TimeItTakesDoctor);
        EditorGUI.indentLevel++;
        if (TreatmentScript.chanceOfSuccess > 0)
        {
            GUILayout.Label("Success caption:");
            TreatmentScript.successCaption = EditorGUILayout.TextArea(TreatmentScript.successCaption, GUILayout.MinHeight(30));
            TreatmentScript.hasSuccessAudio = EditorGUILayout.Toggle("Has success audio: ", TreatmentScript.hasSuccessAudio);
            if (TreatmentScript.hasSuccessAudio == true)
            {
                EditorGUI.indentLevel++;
                TreatmentScript.SuccessAudioClip = (AudioClip)EditorGUILayout.ObjectField(TreatmentScript.SuccessAudioClip, typeof(AudioClip), true);
                EditorGUI.indentLevel--;
            }
        }
        EditorGUI.indentLevel--;
        EditorGUI.indentLevel++;
        if (TreatmentScript.chanceOfSuccess < 100)
        {
            GUILayout.Label("Fail caption:");
            TreatmentScript.failCaption = EditorGUILayout.TextArea(TreatmentScript.failCaption, GUILayout.MinHeight(30));
            TreatmentScript.hasFailAudio = EditorGUILayout.Toggle("Has failed audio: ", TreatmentScript.hasFailAudio);
            EditorGUI.indentLevel++;
            if (TreatmentScript.hasFailAudio == true)
            {
                EditorGUI.indentLevel++;
                TreatmentScript.FailAudioClip = (AudioClip)EditorGUILayout.ObjectField(TreatmentScript.FailAudioClip, typeof(AudioClip), true);
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
        if (TreatmentScript.vitalMods != null)
        {
            for (int count = 0; count < TreatmentScript.vitalMods.Count; count++)
            {

                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUILayout.Label(TreatmentScript.vitalMods[count].vitalToMod.ToString());

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

                TreatmentScript.vitalMods[count].vitalToMod = (vital_mod.vitalType)EditorGUILayout.EnumPopup("Vital type: ", TreatmentScript.vitalMods[count].vitalToMod);

                if (TreatmentScript.vitalMods[count].StartCurve == null)
                {
                    TreatmentScript.vitalMods[count].StartCurve = new AnimationCurve();
                }
                if (TreatmentScript.vitalMods[count].EndCurve == null)
                {
                    TreatmentScript.vitalMods[count].EndCurve = new AnimationCurve();
                }
                TreatmentScript.vitalMods[count].StartCurve = EditorGUILayout.CurveField("Start curve: ", TreatmentScript.vitalMods[count].StartCurve);
                TreatmentScript.vitalMods[count].EndCurve = EditorGUILayout.CurveField("End curve: ", TreatmentScript.vitalMods[count].EndCurve);


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
        if (TreatmentScript.vitalMods.Count > 0)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            if (TreatmentScript.vitalMods[0].fixes_issues != null)
            {
                for (int count2 = 0; count2 < TreatmentScript.vitalMods[0].fixes_issues.Count; count2++)
                {

                    GUILayout.BeginVertical(EditorStyles.helpBox);
                    EditorGUI.indentLevel++;
                    GUILayout.BeginHorizontal();
                    //GUILayout.Label("Instantly fixes " + TreatmentScript.vitalMods[count].fixes_issues[count2].name.ToString());


                    GUILayout.BeginHorizontal();
                    //GUILayout.Label(TreatmentScript.vitalMods[0].fixes_issues[count2].name.ToString());

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
                    TreatmentScript.vitalMods[0].fixes_issues[count2].name = EditorGUILayout.TextField("Issue name: ", TreatmentScript.vitalMods[0].fixes_issues[count2].name);
                    TreatmentScript.vitalMods[0].fixes_issues[count2].vitalType = (vital_mod.vitalType)EditorGUILayout.EnumPopup("Vital type: ", TreatmentScript.vitalMods[0].fixes_issues[count2].vitalType);

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
        TreatmentScript.notes = EditorGUILayout.TextArea(TreatmentScript.notes, GUILayout.MinHeight(100));
        // end of the notes section
    }

    public void multiMedia()
    {
        TreatmentScript.hasMultiMedia = EditorGUILayout.Toggle("Use multi-media?", TreatmentScript.hasMultiMedia);

        if (TreatmentScript.hasMultiMedia == true)
        {
            GUILayout.BeginVertical(EditorStyles.helpBox);
            TreatmentScript.image = (Material)EditorGUILayout.ObjectField(TreatmentScript.image, typeof(Material), true);
            if (TreatmentScript.image != null)
            {
                TreatmentScript.size = EditorGUILayout.Vector2Field("Media size (cm): ", TreatmentScript.size);
            }
            GUILayout.EndVertical();
        }

    }

    public void Page(int GoToPage)
    {
        page = GoToPage;
    }
    public void models()
    {
        Color defaultColor = GUI.color;
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical(EditorStyles.helpBox);
        headerStyle.fontSize = 15; //change the font size
        headerStyle.fontStyle = FontStyle.Bold;
        GUILayout.Label("Enable models: ", headerStyle);
        for (int count = 0; count < TreatmentScript.EnableModels.Count; count++)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            TreatmentScript.EnableModels[count] = (GameObject)EditorGUILayout.ObjectField(TreatmentScript.EnableModels[count], typeof(GameObject), true);
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
        for (int count = 0; count < TreatmentScript.DisableModels.Count; count++)
        {
            GUILayout.BeginHorizontal(EditorStyles.helpBox);
            TreatmentScript.DisableModels[count] = (GameObject)EditorGUILayout.ObjectField(TreatmentScript.DisableModels[count], typeof(GameObject), true);
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
        TreatmentScript.EnableModels.RemoveAt(index);
    }

    public void AddModelEnable()
    {
        if (TreatmentScript.EnableModels.Count != 0)
        {
            TreatmentScript.EnableModels.Add(TreatmentScript.EnableModels[TreatmentScript.EnableModels.Count - 1]);
        }
        else
        {
            TreatmentScript.EnableModels.Add(TreatmentScript.gameObject);
        }
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
    public void RemoveModelDisable(int index)
    {
        TreatmentScript.DisableModels.RemoveAt(index);
    }
    public void AddModelDisable()
    {
        if (TreatmentScript.DisableModels.Count != 0)
        {
            TreatmentScript.DisableModels.Add(TreatmentScript.DisableModels[TreatmentScript.DisableModels.Count - 1]);
        }
        else
        {
            TreatmentScript.DisableModels.Add(TreatmentScript.gameObject);
        }
    }





}