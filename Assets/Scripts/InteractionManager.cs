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

    public float interactionDistance = 1.5f;

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

    Interactable interactionObject;

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

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, mask))
        {

            if (hit.transform.CompareTag("Interactable"))
            {

                interactionObject = hit.collider.gameObject.GetComponent<Interactable>();
                cursorGraphic.sprite = iconDictionary[interactionObject.interactionType];

                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("interaction triggered");
                    StartCoroutine(TriggerEvents());
                }

            }
            else
            {
                cursorGraphic.sprite = defaultCursor;
            }
        }
        else
        {
            cursorGraphic.sprite = defaultCursor;
        }

    }

    IEnumerator TriggerEvents()
    {
        yield return new WaitForEndOfFrame();
        
        interactionObject.triggerEvents.Invoke();
    }

}
