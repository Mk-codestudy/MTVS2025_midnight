using System.Collections.Generic;
using UnityEngine;

public class DinosaurSounds : MonoBehaviour
{
    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public List<AudioClip> audios;

    public void AudioOne()
    {
        audioSource.clip = audios[0];
        audioSource.Play();

        print("AudioOne");
    }

    public void AudioTwo()
    {
        audioSource.clip = audios[1];
        audioSource.Play();
        print("AudioTwo");

    }
}
