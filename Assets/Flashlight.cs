using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject flashLight;

    public float sensitivity = 100;

    Vector2 _sensitivity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        _sensitivity = new Vector2(Screen.height, Screen.width) / sensitivity;

        Vector2 centered = (Vector2)Input.mousePosition - (new Vector2(Screen.width / 2f, Screen.height / 2f));

        centered = new Vector2(-centered.y, centered.x);

        flashLight.transform.localRotation = Quaternion.Euler(centered/_sensitivity);

    }
}
