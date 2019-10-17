using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and Prefab overrides.
[CustomEditor(typeof(UFOData))]
[CanEditMultipleObjects]
public class MyPlayerEditor : Editor
{
    SerializedProperty score;
    SerializedProperty color;
    //SerializedProperty direction;
    SerializedProperty scale;

    void OnEnable()
    {
        // Setup the SerializedProperties.
        score = serializedObject.FindProperty("score");
        color = serializedObject.FindProperty("color");
        //direction = serializedObject.FindProperty("direction");
        scale = serializedObject.FindProperty("scale");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        // Show the custom GUI controls.
        EditorGUILayout.PropertyField(score, new GUIContent("score"));
        EditorGUILayout.PropertyField(color, new GUIContent("color"));
        //EditorGUILayout.PropertyField(scale, new GUIContent("direction"));
        EditorGUILayout.PropertyField(scale, new GUIContent("scale"));

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }

    // Custom GUILayout progress bar.
    void ProgressBar(float value, string label)
    {
        // Get a rect for the progress bar using the same margins as a textfield:
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}