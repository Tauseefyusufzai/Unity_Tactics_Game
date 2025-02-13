using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObstacleData))]
public class ObstacleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObstacleData data = (ObstacleData)target;

        EditorGUI.BeginChangeCheck(); // Track the changes

        for (int y = 9; y >= 0; y--) // Iterate the rows from top to bottom
        {
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < 10; x++) // Iterate the columns from the left to right
            {
                bool newValue = EditorGUILayout.Toggle(data.obstacles[y].row[x], GUILayout.Width(20));
                if (newValue != data.obstacles[y].row[x])
                {
                    data.obstacles[y].row[x] = newValue;
                    EditorUtility.SetDirty(data);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        if (EditorGUI.EndChangeCheck()) 
        {
            AssetDatabase.SaveAssets(); // Force save the asset file
            Debug.Log("ObstacleData Saved!"); // Debugging
        }
    }
}
