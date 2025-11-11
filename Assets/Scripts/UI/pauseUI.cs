using System.Collections;
using System.Collections.Generic;
using Ink.Parsed;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseUI : MonoBehaviour, IDataPersistence
{
    [Header("Pause Menus")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private GameObject normalPauseMenu;
    [SerializeField] private GameObject inProgressPauseMenu;

    [Header("Status Panel")]
    [SerializeField] private TextMeshProUGUI chapterText;
    [SerializeField] private TextMeshProUGUI expLevel;
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI expRequiredToLevelUp;

    [Header("Navigation Menu Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject quitAndSaveButton;

    private void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onPausePressed += pausePressed;
    }

    private void OnDisable()
    {

        gameEventsManager.instance.inputEvents.onPausePressed -= pausePressed;
    }


    void pausePressed(inputEventContext inputEventContext)
    {
        pauseToggle();

        // check if gameplay is other than main
        if (inputEventContext.Equals(inputEventContext.DEFAULT))
        {
            normalPauseMenu.SetActive(true);
            inProgressPauseMenu.SetActive(false);
        }
        else
        {
            inProgressPauseMenu.SetActive(true);
            normalPauseMenu.SetActive(false);
        }
    }

    // also for continue button on pause menu
    public void pauseToggle()
    {
        if (contentParent.activeInHierarchy)
        {
            contentParent.SetActive(false);

        }
        else
        {
            contentParent.SetActive(true);
        }
    }

    public void loadData(gameData data)
    {
        chapterText.text = data.playerChapter.ToString();
        expLevel.text = data.playerExp.ToString();
        expRequiredToLevelUp.text = globalConstants.experienceToLevelUp - data.playerExp + " left to level up";
    }

    public void saveData(gameData data)
    {

    }

    public void quitAndSavePressed()
    {
        dataPersistenceManager.instance.saveGame();
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
