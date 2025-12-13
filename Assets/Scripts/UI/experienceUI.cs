using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class experienceUI : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI expText;
    [SerializeField] private TextMeshProUGUI currentLevelText;


    private void OnEnable()
    {
        gameEventsManager.instance.playerEvents.onPlayerExperienceChange += experienceGained;
        gameEventsManager.instance.playerEvents.onPlayerLevelChange += playerLevelUp;
    }
    void experienceGained(int exp)
    {
        expSlider.value = (float)exp / (float)100;
        expText.text = exp + " / " + 100;
    }

    void playerLevelUp(int lvl)
    {

        currentLevelText.text = "Level " + lvl;
    }

    public void loadData(gameData data)
    {
        this.expText.text = data.playerExp.ToString();
        this.currentLevelText.text = data.playerLvl.ToString();
    }

    public void saveData(gameData data)
    {
    }
}
