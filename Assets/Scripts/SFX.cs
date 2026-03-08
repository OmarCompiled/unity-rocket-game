using System;
using UnityEngine;

[Serializable]
public class SFX
{
    [SerializeField]
    private AudioSource audioSource;

    public 
    void Play()
    {
        if(!audioSource.isPlaying) audioSource.Play();
    }

    public
    void Stop()
    {
        audioSource.Stop();
    }
}
