using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class experienceUI : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    void OnEnable()
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
        // if (isMaxLevel == true)
        // {
        //     expSlider.value = 100;
        //     return;
        // }
        
        // expSlider.value = (float)exp / (float)100;
        Debug.Log($"[Exp UI] RECEIVED EVENT: {exp}");
        float percentage = (float)exp / globalConstants.experienceToLevelUp;
        expSlider.value = percentage;
    }

    void playerLevelUp(int level)
    {
        // if (level >= globalConstants.maxLevel)
        // {
        //     isMaxLevel = true;
        //     levelText.text = "MAX Level";
        //     return;
        // }
        // else levelText.text = "Level " + level;
        if (level >= globalConstants.maxLevel)
        {
            levelText.text = "MAX Level";
            expSlider.value = 1f;
        }
        else levelText.text = "Level " + level;
    }

    public void loadData(gameData data)
    {
        experienceGained(data.playerExp);
        playerLevelUp(data.playerLvl);

    }

    public void saveData(gameData data) { }
}
