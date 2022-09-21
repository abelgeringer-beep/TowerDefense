using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapBrain))]
public class MapBrainInspector : Editor
{
    private MapBrain mapBrain;

    private void OnEnable()
    {
        mapBrain = (MapBrain)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(Application.isPlaying)
        {
            GUI.enabled = !mapBrain.IsAlgorythmRunning;
            if (GUILayout.Button("Run genetic algorythm"))
            {
                mapBrain.RunAlgorythm();
            }
        }
    }
}
