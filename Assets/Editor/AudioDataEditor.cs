using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioData))]
public class AudioDataEditor : Editor
{
    private SerializedProperty _keysProp;
    private SerializedProperty _valuesProp;

    private void OnEnable()
    {
        _keysProp = serializedObject.FindProperty("_keys");
        _valuesProp = serializedObject.FindProperty("_values");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Audio ID", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Audio Clip", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < _keysProp.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_keysProp.GetArrayElementAtIndex(i), GUIContent.none);
            EditorGUILayout.PropertyField(_valuesProp.GetArrayElementAtIndex(i), GUIContent.none);
            if (GUILayout.Button("-", GUILayout.Width(20)))
            {
                _keysProp.DeleteArrayElementAtIndex(i);
                _valuesProp.DeleteArrayElementAtIndex(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Entry"))
        {
            _keysProp.arraySize++;
            _valuesProp.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
