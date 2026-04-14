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

    void Start()
    {
        LoadVolumeSettings();
    }

    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void setSoundFXVolume(float volume)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(volume) * 20f);
        PlayerPrefs.SetFloat("soundFXVolume", volume);
    }

    void LoadVolumeSettings()
    {
        // Load and apply each setting
        setMasterVolume(PlayerPrefs.GetFloat("masterVolume", 0.75f));
        setMusicVolume(PlayerPrefs.GetFloat("musicVolume", 0.75f));
        setSoundFXVolume(PlayerPrefs.GetFloat("soundFXVolume", 0.75f));
    }

    public void SaveVolumeSettings()
    {
        PlayerPrefs.Save();
    }
}
