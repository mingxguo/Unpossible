using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PathCreation;

[CustomEditor(typeof(TubeGenerator))]
public class TubeGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TubeGenerator generator = (TubeGenerator)target;
        if (GUILayout.Button("Generate Tube from path"))
        {
            generator.GenerateTube(false);
        }
        if (GUILayout.Button("Generate Tube from file"))
        {
            generator.GenerateTube(true);
        }
    }
}
