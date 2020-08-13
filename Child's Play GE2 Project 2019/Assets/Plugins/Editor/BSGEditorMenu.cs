/// <summary>
/// Description: Add a menu to the editor (BSGMenu > SceneS in build editor)
///              Build setting editor, add same scene multiple time in the build settings
/// Author: Alexandre Lepage
/// Date: 08 Oct 2018
/// Update: 23 Jan 2019 Change directory to  'Assets/plugins/[...]' from just 'Assets/[...]'
///                     Added return to line 62 to prevent null at line 68.
///         19 Dec 2018. Now works properly with Unity 2018.3
/// GitHub: https://github.com/GrisWoldDiablo
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BSGWindow : EditorWindow
{
    private Vector2 scrollBarPos;
    private static GameObject bsgObject;
    private static BSGButtonScript inspectorGUICode;
    private static Editor customEditor;

    [MenuItem("BSGMenu/Scenes in Build Generator")]
    static void Init()
    {
        BSGWindow myWindow = (BSGWindow)GetWindow(typeof(BSGWindow));
        myWindow.titleContent = new GUIContent("Build Scene Generator");
        bsgObject = new GameObject("BSG");
        bsgObject.hideFlags = HideFlags.HideInHierarchy;
        bsgObject.AddComponent<BSGButtonScript>();
        inspectorGUICode = bsgObject.GetComponent<BSGButtonScript>();
        customEditor = Editor.CreateEditor(inspectorGUICode);
        inspectorGUICode.Populate();
    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("CLOSE", GUILayout.MaxWidth(100), GUILayout.Height(25)))
        {
            this.Close();
            return;
        }
        GUILayout.EndHorizontal();
        scrollBarPos = GUILayout.BeginScrollView(scrollBarPos, false, true, GUILayout.ExpandHeight(true));
        
        
        if (bsgObject != null)
        {            
            if (inspectorGUICode != null)
            {
                
                customEditor.OnInspectorGUI();
            }
            else
            {
                Debug.Log("BSGButtonScript | Not Found!");
            }
        }
        else
        {
            Debug.Log("Error! | Close BSG window and retry.");
        }

        GUILayout.EndScrollView();
    }

    private void OnDestroy()
    {
        if (bsgObject != null)
        {
            DestroyImmediate(bsgObject);
        }
    }

    private void OnDisable()
    {
        if (bsgObject != null)
        {
            DestroyImmediate(bsgObject);
        }
    }
}
