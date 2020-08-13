/// <summary>
/// Description: Add a menu to the editor (BSGMenu > SceneS in build editor)
///              Build setting editor, add same scene multiple time in the build settings
/// Author: Alexandre Lepage
/// Date: 08 Oct 2018
/// Update: 19 Dec 2018. Now works properly with Unity 2018.3
/// GitHub: https://github.com/GrisWoldDiablo
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;

[CustomEditor(typeof(BSGButtonScript))]
public class BSGInspectorButton : Editor {
    
    public override void OnInspectorGUI()
    {

        BSGButtonScript myScript = (BSGButtonScript)target;

        serializedObject.Update();
        GUILayout.Label("Current scenes in Build", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("currentScenes"), true);
		
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
        GUILayout.Label("Current settings", EditorStyles.boldLabel);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Get scenes list", GUILayout.MaxWidth(175), GUILayout.Height(25)))
        {
            myScript.Populate();
        }
		GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUI.BeginChangeCheck();

        GUILayout.Space(10.0f);
        GUILayout.Label("Custom scenes in Build", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("List size: ", GUILayout.Height(25));
        if (GUILayout.Button("-", EditorStyles.miniButtonLeft, GUILayout.MaxWidth(25), GUILayout.Height(25)))
        {
            myScript.DecreaseListSize();
        }
        if (GUILayout.Button("+", EditorStyles.miniButtonRight, GUILayout.MaxWidth(25), GUILayout.Height(25)))
        {
            myScript.IncreaseListSize();
        } 
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("customScenes"), true);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("Custom scenes in Build", EditorStyles.boldLabel, GUILayout.MaxWidth(175));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Apply new settings", GUILayout.MaxWidth(175), GUILayout.Height(25)))
        {
            myScript.AddScene();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Label("Info Ouput");
        GUILayout.TextArea(myScript.Errors);
        if (GUILayout.Button("Clear", GUILayout.Width(50)))
        {
            myScript.Clear();
        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties();
        }
    }
}
