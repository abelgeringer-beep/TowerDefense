using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorInspector : Editor
{
    public MapGenerator map;

    private void OnEnable()
    {
        map = (MapGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(Application.isPlaying)
        {
            if(GUILayout.Button("Generate new map"))
            {
                map.GenerateNewMap();
            }
        }
    }
}
