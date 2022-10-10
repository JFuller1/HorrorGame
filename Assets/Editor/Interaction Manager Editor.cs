using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InteractionManager))]
public class InteractionManagerEditor : Editor
{

    InteractionManager manager;


    public void OnEnable()
    {
        manager = (InteractionManager)target;
        
        if (Enum.GetValues(typeof(InteractionTypes)).Length != manager.stateIcon.Length)
        {
            manager.stateIcon = new StateIcon[Enum.GetValues(typeof(InteractionTypes)).Length];
        }

    }

    public override void OnInspectorGUI()
    {

        base.OnInspectorGUI();

        int index = 0;

        foreach (InteractionTypes type in Enum.GetValues(typeof(InteractionTypes)))
        {
            manager.stateIcon[index].icon = manager.stateIcon[index].icon;
            manager.stateIcon[index].state = type;

            index++;

        }

        if (GUILayout.Button("Open Zone Creator"))
        {
            EditorWindow.GetWindow<InteractionZoneCreator>("InteractionZoneCreator");
        }

        EditorUtility.SetDirty(manager);
    }

}
