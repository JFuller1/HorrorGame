using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    bool movement = false;
    public Transform tr;

    Vector3 target;
    float elapsedTime = 0f;
    float duration = 3f;

    List<string> footsteps = new List<string>();
    char[] sounds = { '1', '2', '3', '4', '5' };
    int previousNum = -1;
    int soundNum = -2;

    void Start()
    {
        foreach(char c in sounds)
        {
            footsteps.Add($"event:/Person Ambience/Footsteps/step{c}");
        }
    }

    void Update()
    {
        if (!movement)
        {
            HandleRotation();
            HandleMovement();
        }
    }

    void HandleRotation()
    {
        if (Input.GetKeyDown("a"))
        {
            Quaternion target = Quaternion.Euler(0, tr.eulerAngles.y - 90f, 0);
            StartCoroutine(PerformRotation(target));
        }
        else if (Input.GetKeyDown("d"))
        {
            Quaternion target = Quaternion.Euler(0, tr.eulerAngles.y + 90f, 0);
            StartCoroutine(PerformRotation(target));
        }
    }

    void HandleMovement()
    {

        if (Input.GetKeyDown("w"))
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 1f))
            {

                target = hit.transform.position;

                if (hit.transform.CompareTag("Node"))
                {

                    //movement = true;
                    PlayRandomFootstep();
                    StartCoroutine(PerformMovement(hit.transform.position));
                }
            }
        }

    }


    IEnumerator PerformRotation(Quaternion targetRotation)
    {
        movement = true;
        float progress = 0f;
        float speed = 5f;

        Quaternion start = tr.rotation;

        while (progress < 1f)
        {
            progress += Time.deltaTime * speed;
            //progress =  Mathf.Clamp(progress, 0f, 1f);

            tr.rotation = Quaternion.Lerp(start, targetRotation, Mathf.SmoothStep(0,1, progress));
            
            yield return null;
            Debug.Log(progress);
        }
        movement = false;
        yield return null;
    }

    IEnumerator PerformMovement(Vector3 end)
    {
        movement = true;
        float time = 0f;
        float speed = 5f;

        Vector3 start = transform.position;

        while (time < 1)
        {
            time += Time.deltaTime * speed;
            transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0,1, time));
            
            yield return null;
        }
        //yield return new WaitForSeconds(0.05f);
        movement = false;
        yield return null;
    }

    void PlayRandomFootstep()
    {
        if (previousNum != null)
        {
            do
            {
                soundNum = Random.Range(0, footsteps.Count);
            } while (soundNum == previousNum);
        }

        previousNum = soundNum;
        FMODUnity.RuntimeManager.PlayOneShot(footsteps[soundNum]);
    }
}
