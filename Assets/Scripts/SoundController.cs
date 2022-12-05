using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public EventReference audio;
    public string sound;
    FMOD.Studio.EventInstance audioEvent;
    void Start()
    {
        audioEvent = FMODUnity.RuntimeManager.CreateInstance(sound);
        //remove once conditional works
        audioEvent.start();
       
    }

    void Update()
    {
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(audioEvent, GetComponent<Transform>(), GetComponent<Rigidbody>());
    }

    void PlaySound() 
    { 
    
    }

    void StopSound()
    {

    }

}
