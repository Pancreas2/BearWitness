using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MapMenu))]
public class MapMenuEditor : Editor
{
    override public void OnInspectorGUI()
    {
        MapMenu menu = (MapMenu)target;
        if (GUILayout.Button("Log Compass Position"))
        {
            menu.LogCompassPosition(); // how do i call this?
        }
        DrawDefaultInspector();
    }
}