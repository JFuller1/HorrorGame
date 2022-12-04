using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InteractionTypes
{
    View,
    Move,
    Talk,
}

[System.Serializable]
public struct StateIcon
{
    public InteractionTypes state;
    public Sprite icon;
}

public class InteractionManager : MonoBehaviour
{

    public Sprite defaultCursor;

    public StateIcon[] stateIcon;

    public DialogManager dialogManager;

    //public ViewManager viewManager;

    public Image cursorGraphic;
    private InteractionTypes currentType;

    public LayerMask mask;

    Dictionary<InteractionTypes, Sprite> iconDictionary = new Dictionary<InteractionTypes, Sprite>();

    Camera cam;

    private float UpscaledMouse;

    private void Awake()
    {
        for (int i = 0; i < stateIcon.Length; i++)
        {
            //Debug.Log(stateIcon[i].state); 
            iconDictionary.Add(stateIcon[i].state, stateIcon[i].icon);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        UpscaledMouse = Screen.width / 256;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition/UpscaledMouse);

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, mask))
        {
            Interactable interactionObject = hit.collider.gameObject.GetComponent<Interactable>();
            cursorGraphic.sprite = iconDictionary[interactionObject.interactionType];

            if (Input.GetMouseButtonDown(0))
            {
                interactionObject.triggerEvents.Invoke();
            }

        }
        else
        {
            cursorGraphic.sprite = defaultCursor;
        }

    }
}
