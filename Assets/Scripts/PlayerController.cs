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
            Quaternion target = Quaternion.Euler(0, tr.eulerAngles.y - 90f, 0);
            StartCoroutine(PerformRotation(target));
        } else if (Input.GetKeyDown("d"))
        {
            Quaternion target = Quaternion.Euler(0, tr.eulerAngles.y + 90f, 0);
            StartCoroutine(PerformRotation(target));
        }
    }

    void HandleMovement()
    {
        if(Input.GetKeyDown("w"))
        {
            RaycastHit hit;

            if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f))
            {
                if(hit.transform.CompareTag("Node"))
                {
                    StartCoroutine(PerformMovement());
                }
            }
        }
    }

    IEnumerator PerformRotation(Quaternion targetRotation)
    {
        movement = true;
        float progress = 0f;
        float speed = 0.5f;
        float snap = 0.2f;

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

    IEnumerator PerformMovement()
    {
        movement = true;
        float time = 0f;
        float speed = 5f;
        while(time < 1)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            time += Time.deltaTime * speed;
            yield return null;
        }
        yield return new WaitForSeconds(0.05f);
        movement = false;
    }
}
