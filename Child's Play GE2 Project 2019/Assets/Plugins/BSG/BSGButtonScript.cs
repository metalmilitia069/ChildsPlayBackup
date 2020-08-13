/// <summary>
/// Description: Add a menu to the editor (BSGMenu > SceneS in build editor)
///              Build setting editor, add same scene multiple time in the build settings
/// Author: Alexandre Lepage
/// Date: 08 Oct 2018
/// Update: 19 Dec 2018. Now works properly with Unity 2018.3
/// GitHub: https://github.com/GrisWoldDiablo
/// </summary>

#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class BSGButtonScript : MonoBehaviour {

    public EditorBuildSettingsScene[] buildScenes;
    [SerializeField] private List<SceneAsset> currentScenes;
    [SerializeField] private List<SceneAsset> customScenes;

    private string errors;
    public string Errors { get { return errors; } }

    public void Populate(bool clear = true)
    {
        if (clear)
        {
            Clear();
            errors += "---Result---\n";
        }
        else
        {
            errors += "\n";
        }
        currentScenes = new List<SceneAsset>();
        customScenes = new List<SceneAsset>();
        buildScenes = EditorBuildSettings.scenes;
        foreach (var item in buildScenes)
        {
            string itemGUID = item.guid.ToString();
            string itemPath = AssetDatabase.GUIDToAssetPath(itemGUID);
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(itemPath);
            currentScenes.Add(sceneAsset);
            customScenes.Add(sceneAsset);
        }
        errors += "Current scene in build list populated.";
        
    }
    
    public void AddScene()
    {
        errors = "Generating new build settings.\n---Errors---";
        Debug.ClearDeveloperConsole();
        if (customScenes.Count == 0)
        {
            errors += "\nCustom Scenes list empty.";
            Debug.Log("Custom Scenes list empty");
            return;
        }

        var newSettings = new EditorBuildSettingsScene[customScenes.Count];
        int index = 0;
        int csIndex = -1;
        foreach (var item in customScenes)
        {
            csIndex++;
            if (item == null)
            {
                errors += "\nElement "+ csIndex +" | Missing Scene Asset.";
                continue;
            }
            string foundGUID = null;
            long foundLocalID;            
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(item, out foundGUID, out foundLocalID))
            {
                var sceneToAdd = new EditorBuildSettingsScene(new GUID(foundGUID), true);
                Debug.Log("Added: " + sceneToAdd.path + " : " + foundGUID);
                newSettings[index++] = sceneToAdd;
            }
            else
            {
                errors += "\ni:" + csIndex + "Missing GUID. Skipping Scene";
                Debug.Log("Missing GUID");
            }
        }
        if (index == 0)
        {
            errors += "\nNo Scene Assets found.";
            Debug.Log("No Scene Assets found");
            return;
        }
        var newSettingsResize = new EditorBuildSettingsScene[index];
        index = 0;
        foreach (var item in newSettings)
        {
            if (item != null)
            {
                newSettingsResize[index++] = item; 
            }
        }
        EditorBuildSettings.scenes = newSettingsResize;
        if (errors == "Generating new build settings.\n---Errors---")
        {
            errors += "\nNo Error Found.";
        }
        errors += "\n---Result---\n" + index +" scene(s) added."+"\nBuild settings completed.";
        Populate(false);
    }

    public void Clear()
    {
        errors = string.Empty;
    }
    public void DecreaseListSize()
    {
        if (customScenes.Count != 0)
        {
            customScenes.RemoveAt(customScenes.Count - 1); 
        }
    }

    public void IncreaseListSize()
    {
        customScenes.Add(null);
    }
}
#endif