using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public Camera camera;

    public void TriggerView(Vector2 coords)
    {
        Debug.Log(coords);
        camera.transform.position = new Vector3(coords.x, coords.y, -10f);
    }
}
