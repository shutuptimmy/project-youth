using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pauseUI : MonoBehaviour //, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private soundsMenuUI soundsMenuUI;

    [Header("Pause Menus")]
    [SerializeField] private GameObject normalPauseMenu;
    [SerializeField] private GameObject inProgressPauseMenu;

    // [Header("Status Panel")]
    // [SerializeField] private TextMeshProUGUI chapterText;
    // [SerializeField] private TextMeshProUGUI expLevel;
    // [SerializeField] private Slider expBar;
    // [SerializeField] private TextMeshProUGUI expRequiredToLevelUp;

    [Header("Navigation Menu Buttons")]
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject resumeButton;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject quitAndSaveButton;

    private void OnEnable()
    {
        gameEventsManager.instance.inputEvents.onPausePressed += pausePressed;
    }

    private void OnDisable()
    {

        gameEventsManager.instance.inputEvents.onPausePressed -= pausePressed;
    }


    void pausePressed()
    {
        inputEventContext currentContext = gameEventsManager.instance.inputEvents.inputEventContext;
        pauseToggle();

        // check if gameplay is other than main then set to main pause menu, otherwise to partial pause menu
        if (currentContext.Equals(inputEventContext.DEFAULT))
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
            Time.timeScale = 1f;
            gameEventsManager.instance.playerEvents.EnablePlayerMovement();
        }
        else
        {
            contentParent.SetActive(true);
            Time.timeScale = 0f;
            gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        }
    }

    public void SettingsToggle()
    {
        soundsMenuUI.activateMenu();
    }

    public void quitAndSavePressed()
    {
        Time.timeScale = 1f;
        dataPersistenceManager.instance.saveGame();
        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }
}
