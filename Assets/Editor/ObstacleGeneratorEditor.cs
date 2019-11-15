using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleGenerator))]
public class ObstacleGeneratorEditor : Editor
{

    private int next_obstacle = 0;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ObstacleGenerator generator = (ObstacleGenerator)target;
        string[] names = new string[generator.obstacles.Length];
        for (int i = 0; i < generator.obstacles.Length; ++i)
        {
            names[i] = generator.obstacles[i].name;
        }

        next_obstacle = EditorGUILayout.Popup("Next obstacle", next_obstacle, names);
        if (GUILayout.Button("Generate Next"))
        {
            generator.AddObstacle(next_obstacle);
        }
    }
}
