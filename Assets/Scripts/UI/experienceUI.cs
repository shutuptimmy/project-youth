using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class experienceUI : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    private bool isMaxLevel = false;
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
        if (isMaxLevel == true)
        {
            expSlider.value = 100;
            return;
        }
        else expSlider.value = (float)exp / (float)100;
    }

    void playerLevelUp(int level)
    {
        if (level >= globalConstants.maxLevel)
        {
            isMaxLevel = true;
            levelText.text = "MAX Level";
            return;
        }
        else levelText.text = "Level " + level;
    }

    public void loadData(gameData data)
    {
        playerLevelUp(data.playerLvl);
        experienceGained(data.playerExp);

    }

    public void saveData(gameData data) { }
}
