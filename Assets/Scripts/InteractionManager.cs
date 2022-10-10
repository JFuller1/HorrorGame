using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public SpriteRenderer cursorGraphic;

    Dictionary<InteractionTypes, Sprite> iconDictionary = new Dictionary<InteractionTypes, Sprite>();

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

    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.tag == "Interactable")
            {
                cursorGraphic.sprite = iconDictionary[hit.collider.GetComponent<Interactable>().interactionType];
            }

        }
        else
        {
            cursorGraphic.sprite = defaultCursor;
        }
    }
}
