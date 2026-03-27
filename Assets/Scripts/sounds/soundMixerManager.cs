using UnityEngine;
using UnityEngine.Audio;

public class soundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
    }
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
    }

    public void setSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);

    }
}
