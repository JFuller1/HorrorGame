using System;
using UnityEngine;
using UnityEditor;

public class InteractionZoneCreator : EditorWindow
{

    InteractionTypes obj_type;

    string obj_name;

    Vector2 obj_location;

    [MenuItem("Window/InteractionZoneCreator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow<InteractionZoneCreator>("InteractionZoneCreator");
    }

    private void OnGUI()
    {
        //Window code

        obj_name = EditorGUILayout.TextField("Name", obj_name);

        obj_type = (InteractionTypes)EditorGUILayout.EnumPopup("Type", obj_type);

        obj_location = EditorGUILayout.Vector2Field("Location", obj_location);        

        switch (obj_type)
        {
            case InteractionTypes.View:
                GUILayout.Label("object to view");
                break;
            case InteractionTypes.Move:
                GUILayout.Label("scene to move");
                break;
            case InteractionTypes.Talk:
                GUILayout.Label("insert dialog here");
                break;
            default:
                break;
        }

        if(GUILayout.Button("Generate Zone"))
        {
            if (!GameObject.Find("Zone Container"))
            {
                new GameObject("Zone Container");
            }

            GameObject obj_new = new GameObject(obj_name);

            obj_new.transform.SetParent(GameObject.Find("Zone Container").transform);
            obj_new.tag = "Interactable";

            Interactable obj_interactable = obj_new.AddComponent<Interactable>();
            obj_interactable.interactionType = obj_type;
            
            PolygonCollider2D obj_shape = obj_new.AddComponent<PolygonCollider2D>();
            obj_shape.CreatePrimitive(4, Vector2.one);

        }

    }

}
