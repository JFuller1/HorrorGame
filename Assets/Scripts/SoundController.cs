using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.Events;

public class SoundController : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference audio;
    FMOD.Studio.EventInstance audioEvent;

    Transform slLocation;

    [Header("Occlusion Options")]
    public LayerMask OcclusionLayer = 1;

    bool isOn = false;
    bool soundPlaying;

    public bool playOnStart;

    public UnityEvent ToggleOn;
    public UnityEvent ToggleOff;

    void Start()
    {
        slLocation = GameObject.FindObjectOfType<StudioListener>().transform;
        audioEvent = FMODUnity.RuntimeManager.CreateInstance(audio);

        if (playOnStart)
        {
            PlaySound();
        }
    }

    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(audioEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());

        if (soundPlaying)
        {
            RaycastHit hit;
            Physics.Linecast(transform.position, slLocation.position, out hit, OcclusionLayer);

            if (hit.collider.name == "Player")
            {
                Occlusion(0f);
                Debug.DrawLine(transform.position, slLocation.position, Color.blue);
            }
            else
            {
                Occlusion(1f);
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
        }
    }

    void Occlusion(float value)
    {
        audioEvent.setParameterByName("WallMuffle", value);
    }

    IEnumerator Delay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void PlaySound(int delay = 0) 
    {
        StartCoroutine(Delay(delay));

        audioEvent.start();
        soundPlaying = true;
    }

    public void StopSound()
    {
        audioEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    public void ToggleSound()
    {
        if (soundPlaying == true)
        {
            ToggleOn.Invoke();
            soundPlaying = false;
        }
        else
        {
            ToggleOff.Invoke();
            soundPlaying = true;
        }
    }


    //TOGGLE FUNCTIONS

    float open = 0f;

    public void OpenWindow()
    {
        while(open < 1)
        {
            open += 0.05f;
            audioEvent.setParameterByName("Muffled", open);
            StartCoroutine(Delay(0.1f));
        }

        //play window opening sound too
    }

    public void CloseWindow()
    {
        while (open > 0)
        {
            open -= 0.05f;
            audioEvent.setParameterByName("Muffled", open);
            StartCoroutine(Delay(0.1f));
        }
    }
}
