using UnityEngine;
using UnityEditor;
using System.Collections;


[CustomEditor(typeof(FireSpreadManager))]
public class FireSpreadManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        FireSpreadManager script = (FireSpreadManager)target;
        if (GUILayout.Button("Build Grid"))
            script.Build();
    }
}
