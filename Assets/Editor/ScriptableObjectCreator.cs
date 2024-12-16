using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

public class ScriptableObjectCreator : EditorWindow
{
    private Vector2 scrollPosition;
    private string searchQuery = "";
    private Type selectedType;
    private string fileName = "";
    private string selectedPath = "Assets";

    private static List<Type> cachedTypes;
    private List<Type> filteredTypes;
    private string lastSearchQuery = "";
    private bool needsRepaint;

    [MenuItem("Tools/Scriptable Object Creator")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectCreator>("SO Creator");
    }

    [MenuItem("Assets/Create/Scriptable Objects", priority = 0)]
    private static void CreateFromContextMenu()
    {
        string path = GetSelectedPath();
        ScriptableObjectCreator window = GetWindow<ScriptableObjectCreator>("SO Creator");
        window.selectedPath = path;
    }

    private void OnEnable()
    {
        if (cachedTypes == null)
        {
            CacheScriptableObjectTypes();
        }
        filteredTypes = cachedTypes;
    }

    private static void CacheScriptableObjectTypes()
    {
        cachedTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsAbstract)
            .OrderBy(type => type.Name)
            .ToList();
    }

    private static string GetSelectedPath()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && System.IO.File.Exists(path))
            {
                path = System.IO.Path.GetDirectoryName(path);
            }
            break;
        }
        return path;
    }

    private void OnGUI()
    {
        EditorGUI.BeginChangeCheck();

        DrawSearchBar();

        if (EditorGUI.EndChangeCheck() || needsRepaint)
        {
            UpdateFilteredTypes();
            needsRepaint = false;
        }

        DrawTypeList();
        DrawCreationControls();
    }

    private void DrawSearchBar()
    {
        EditorGUILayout.Space();
        searchQuery = EditorGUILayout.TextField("Search", searchQuery);
    }

    private void UpdateFilteredTypes()
    {
        if (searchQuery != lastSearchQuery)
        {
            lastSearchQuery = searchQuery;
            if (string.IsNullOrEmpty(searchQuery))
            {
                filteredTypes = cachedTypes;
            }
            else
            {
                string lowerQuery = searchQuery.ToLower();
                filteredTypes = cachedTypes
                    .Where(type => type.Name.ToLower().Contains(lowerQuery))
                    .ToList();
            }
            Repaint();
        }
    }

    private void DrawTypeList()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Select Type", EditorStyles.boldLabel);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        foreach (var type in filteredTypes)
        {
            if (GUILayout.Toggle(selectedType == type, type.Name, EditorStyles.radioButton))
            {
                if (selectedType != type)
                {
                    selectedType = type;
                    GUI.changed = true;
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    private void DrawCreationControls()
    {
        EditorGUILayout.Space();

        EditorGUI.BeginChangeCheck();
        fileName = EditorGUILayout.TextField("File Name", fileName);

        if (GUILayout.Button("Select Path"))
        {
            string newPath = EditorUtility.OpenFolderPanel("Select Path", "Assets", "");
            if (!string.IsNullOrEmpty(newPath))
            {
                selectedPath = FileUtil.GetProjectRelativePath(newPath);
            }
        }

        EditorGUILayout.LabelField("Selected Path:", selectedPath);
        EditorGUILayout.Space();

        GUI.enabled = selectedType != null && !string.IsNullOrEmpty(fileName);
        if (GUILayout.Button("Create Scriptable Object"))
        {
            CreateScriptableObject();
            GUIUtility.ExitGUI();
        }
        GUI.enabled = true;
    }

    private void CreateScriptableObject()
    {
        if (selectedType == null || string.IsNullOrEmpty(fileName)) return;

        var asset = ScriptableObject.CreateInstance(selectedType);
        string assetPath = $"{selectedPath}/{fileName}.asset";

        assetPath = AssetDatabase.GenerateUniqueAssetPath(assetPath);

        AssetDatabase.CreateAsset(asset, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;

        fileName = "";
        needsRepaint = true;
    }
}