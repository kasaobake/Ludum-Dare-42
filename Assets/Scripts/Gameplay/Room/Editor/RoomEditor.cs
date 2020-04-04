using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RoomGeneratorComponent))]
public class RoomEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        RoomGeneratorComponent roomGeneratorComponent = this.target as RoomGeneratorComponent;

        if (DrawDefaultInspector())
        {
            roomGeneratorComponent.Generate();
        }

        if (GUILayout.Button("Regenerate"))
        {
            roomGeneratorComponent.Generate();
        }
    }
}
