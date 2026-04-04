using UnityEngine;
using UnityEngine.Audio;

public class soundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public static soundMixerManager instance {get; private set;}

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one Sound Mixer Manager in the scene. Removing duplicate..");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

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
