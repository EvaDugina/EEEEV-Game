#if UNITY_EDITOR
using System;
using System.IO;
using System.Globalization;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
 
[InitializeOnLoad]
public class AutoSaveScene
{
    private const string SAVE_FOLDER = "Editor/AutoSaves";
 
    private static DateTime lastSaveTime = DateTime.Now;
    private static TimeSpan updateInterval;
    
    private static int minuteInterval = 15;

    static AutoSaveScene()
    {
        EditorApplication.playModeStateChanged += LogPlayModeState;

        EnsureAutoSavePathExists();

        // Register for autosaves.
        // Change this number to modify the autosave interval.
        RegisterOnEditorUpdate(minuteInterval);
    }

    public static void RegisterOnEditorUpdate(int interval)
    {
        Debug.Log ("Enabling AutoSave");
 
        updateInterval = new TimeSpan(0, interval, 0);
        EditorApplication.update += OnUpdate;
    }
 
    /// 
    /// Makes sure the target save path exists.
    /// 
    private static void EnsureAutoSavePathExists()
    {
        var path = Path.Combine(Application.dataPath, SAVE_FOLDER);
 
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
 
    /// 
    /// Saves a copy of the currently open scene.
    /// 
    private static void SaveScene()
    {
        Debug.Log("Auto saving scene: " + EditorSceneManager.GetActiveScene().name);
 
        EnsureAutoSavePathExists();
 
        // Get the new saved scene name.
        var newName = GetNewSceneName(EditorSceneManager.GetActiveScene().name);
        var folder = Path.Combine("Assets", SAVE_FOLDER);
 
        EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), Path.Combine(folder, newName), true);
        //     EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();

    }
 
    /// 
    /// Helper method that creates a new scene name.
    /// 
    private static string GetNewSceneName(string originalSceneName)
    {
        var scene = Path.GetFileNameWithoutExtension(originalSceneName);
 
        return $"{scene}_{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)}.unity";
    }
 
    private static void OnUpdate()
    {
        if ((DateTime.Now - lastSaveTime) >= updateInterval)
        {
            SaveScene();
            lastSaveTime = DateTime.Now;
        }
    }

    private static void LogPlayModeState(PlayModeStateChange state)
    {
        if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying) return;
        
        // Save the scene and the assets.
        Debug.Log("Auto-saving all open scenes... " + state);
        EditorSceneManager.SaveOpenScenes();
        AssetDatabase.SaveAssets();
    }
}

#endif