using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume));
    }

    public void setSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume));

    }
}
