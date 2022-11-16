using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool movement = false;
    public Transform tr;

    void Start()
    {
        
    }

    void Update()
    {
        if(!movement)
        {
            HandleRotation();
            HandleMovement();
        }
    }

    void HandleRotation()
    {
        if(Input.GetKeyDown("a"))
        {
            Quaternion target = Quaternion.Euler(tr.eulerAngles.x, tr.eulerAngles.y - 90f, 0);
            StartCoroutine(PerformRotation(target));
        } else if (Input.GetKeyDown("d"))
        {
            Quaternion target = Quaternion.Euler(tr.eulerAngles.x, tr.eulerAngles.y + 90f, 0);
            StartCoroutine(PerformRotation(target));
        }
    }

    IEnumerator PerformRotation(Quaternion targetRotation)
    {
        movement = true;
        float progress = 0f;
        float speed = 0.5f;
        float snap = 0.3f;

        while (progress < 1f)
        {
            tr.rotation = Quaternion.Slerp(tr.rotation, targetRotation, progress);
            progress += Time.deltaTime * speed;

            if (progress <= snap)
            {
                yield return null;
            }
        }
        movement = false;
        yield return null;
    }

    void HandleMovement()
    {

    }
}
