using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        cam = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePos;
    }
}
