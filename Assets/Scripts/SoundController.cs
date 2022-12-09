using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [Header("FMOD Event")]
    public EventReference audio;
    FMOD.Studio.EventInstance audioEvent;

    Transform slLocation;

    [Header("Occlusion Options")]
    public LayerMask OcclusionLayer = 1;

    bool isOn;
    bool soundPlaying;

    public bool playOnStart;

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

        float val;
        audioEvent.getParameterByName("WallMuffle", out val);

        Debug.Log(val);
    }

    IEnumerator Delay(int delay)
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

    void ToggleSound()
    {
        if (soundPlaying == true)
        {
            soundPlaying = false;
        }
        else
        {
            soundPlaying = true;
        }
    }

}
