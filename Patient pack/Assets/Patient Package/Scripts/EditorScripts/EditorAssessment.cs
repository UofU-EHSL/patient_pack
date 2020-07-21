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
                GUI.backgroundColor = defaultcolor;
                GUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add category"))
            {
                AddCategory();
            }
            GUILayout.EndVertical();
        }

        GUILayout.Space(15);
        GUILayout.BeginHorizontal();
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
