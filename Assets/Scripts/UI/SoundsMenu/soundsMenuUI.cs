using UnityEngine;
using UnityEngine.UI;

public class soundsMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject contentParent;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundFXSlider;

    void Start()
    {
        masterSlider.onValueChanged.RemoveAllListeners();
        masterSlider.onValueChanged.AddListener(soundMixerManager.instance.setMasterVolume);

        musicSlider.onValueChanged.RemoveAllListeners();
        musicSlider.onValueChanged.AddListener(soundMixerManager.instance.setMusicVolume);

        soundFXSlider.onValueChanged.RemoveAllListeners();
        soundFXSlider.onValueChanged.AddListener(soundMixerManager.instance.setSoundFXVolume);
    }

    public void activateMenu()
    {
        contentParent.SetActive(true);
    }

    public void deactivateMenu()
    {
        contentParent.SetActive(false);
    }


}
