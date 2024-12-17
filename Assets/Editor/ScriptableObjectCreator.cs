using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;

public class ScriptableObjectCreator : EditorWindow
{
    private Vector2 scrollPosition;
    private string searchQuery = "";
    private Type selectedType;
    private string fileName = "";
    private string selectedPath = "Assets";
    private List<Type> filteredTypes;
    private string lastSearchQuery = "";
    private bool needsRepaint;

    [MenuItem("Tools/Scriptable Object Creator")]
    public static void ShowWindow()
    {
        var window = GetWindow<ScriptableObjectCreator>("SO Creator");
        window.OnEnable();
    }

    [MenuItem("Assets/Create/Scriptable Objects", priority = 0)]
    private static void CreateFromContextMenu()
    {
        var window = GetWindow<ScriptableObjectCreator>("SO Creator");
        window.selectedPath = GetSelectedPath();
        window.OnEnable();
    }

    private void OnEnable()
    {
        CacheScriptableObjectTypes();
        filteredTypes = GetScriptableObjectTypes().ToList();
        searchQuery = "";
        lastSearchQuery = "";
        needsRepaint = true;
    }

    private static List<Type> GetScriptableObjectTypes()
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(ScriptableObject)) && !type.IsAbstract)
            .OrderBy(type => type.Name)
            .ToList();
    }

    private void CacheScriptableObjectTypes()
    {
        EditorApplication.projectChanged += OnProjectChanged;
    }

    private void OnProjectChanged()
    {
        filteredTypes = GetScriptableObjectTypes().ToList();
        needsRepaint = true;
    }

    private void OnDisable()
    {
        EditorApplication.projectChanged -= OnProjectChanged;
    }

    private static string GetSelectedPath()
    {
        string path = "Assets";

        foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (!string.IsNullOrEmpty(path) && File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
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
                filteredTypes = GetScriptableObjectTypes().ToList();
            }
            else
            {
                string lowerQuery = searchQuery.ToLower();
                filteredTypes = GetScriptableObjectTypes()
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

        var projectBrowserType = Type.GetType("UnityEditor.ProjectBrowser,UnityEditor");
        if (projectBrowserType != null)
        {
            EditorWindow projectWindow = EditorWindow.GetWindow(projectBrowserType);
            var isLocked = (bool)projectBrowserType.GetProperty("isLocked",
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance)
                .GetValue(projectWindow);

            if (!isLocked)
            {
                Selection.activeObject = asset;
            }
        }

        fileName = "";
        needsRepaint = true;
    }
}