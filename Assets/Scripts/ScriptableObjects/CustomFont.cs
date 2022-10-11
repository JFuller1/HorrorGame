using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Font", menuName = "ScriptableObjects/CustomFont", order = 1)]
public class CustomFont : ScriptableObject
{
    public Sprite[] characters;
}
