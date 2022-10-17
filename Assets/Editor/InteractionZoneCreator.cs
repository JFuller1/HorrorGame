using System;
using UnityEngine;
using UnityEditor;

public class InteractionZoneCreator : EditorWindow
{

    InteractionTypes obj_type;

    string obj_name;

   // Vector2 obj_location;

    TextAsset text;

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

        //obj_location = EditorGUILayout.Vector2Field("Location", obj_location);        

        switch (obj_type)
        {
            case InteractionTypes.View:
                GUILayout.Label("object to view");
                text = null;
                break;
            case InteractionTypes.Move:
                GUILayout.Label("scene to move");
                text = null;
                break;
            case InteractionTypes.Talk:
                text = (TextAsset)EditorGUILayout.ObjectField("Dialogue JSON File",text, typeof(TextAsset), true);
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


            switch (obj_type)
            {
                case InteractionTypes.View:
                    GUILayout.Label("object to view");
                    text = null;
                    break;
                case InteractionTypes.Move:
                    GUILayout.Label("scene to move");
                    text = null;
                    break;
                case InteractionTypes.Talk:
                    obj_new.AddComponent<DialogueContainer>().dialogue = text;
                    break;
                default:
                    break;
            }

        }

    }

}
