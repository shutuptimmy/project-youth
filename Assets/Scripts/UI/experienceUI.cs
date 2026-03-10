using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class experienceUI : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;
    // [SerializeField] private TextMeshProUGUI currentLevelText;


    private void OnEnable()
    {
        gameEventsManager.instance.playerEvents.onPlayerExperienceChange += experienceGained;
        gameEventsManager.instance.playerEvents.onPlayerLevelChange += playerLevelUp;
    }

    void OnDisable()
    {
        gameEventsManager.instance.playerEvents.onPlayerExperienceChange -= experienceGained;
        gameEventsManager.instance.playerEvents.onPlayerLevelChange -= playerLevelUp;
    }

    void experienceGained(int exp)
    {
        expSlider.value = (float)exp / (float)100;
        // expText.text = exp + " / " + 100;
    }

    void playerLevelUp(int level)
    {
        levelText.text = "Level " + level;
    }

    public void loadData(gameData data)
    {
        experienceGained(data.playerExp);
        playerLevelUp(data.playerLvl);

    }

    public void saveData(gameData data) { }
}
