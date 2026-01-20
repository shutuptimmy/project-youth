using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class backToEarthManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private rocketController rocket;

    [Header("Quest Step")]
    [SerializeField] private spaceQuestStep spaceQuestStep;
    private bool isQuestStepPresent;

    [Header("Menu Panel")]
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    private float timeElapsed;
    private bool isGameActive = false;
    private bool playerHasWon = false;

    IEnumerator Start()
    {
        mainContentParent.SetActive(false);
        gameplayGameObject.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();
        yield return new WaitForSeconds(1f);

        isQuestStepPresent = spaceQuestStep == null;
        Debug.Log("Quest Step Status: " + spaceQuestStep);

        showStartMenu();
    }

    void OnEnable()
    {
        gameEventsManager.instance.playerEvents.onPlayerTookDamage += playerTookDamage;
    }

    void OnDisable()
    {
        gameEventsManager.instance.playerEvents.onPlayerTookDamage -= playerTookDamage;
    }

    void showStartMenu()
    {
        gameplayGameObject.SetActive(true);
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            "Back To Earth",
            "Resist planets' gravity and reach to earth safely. Use [LEFT] or [RIGHT] button to steer and [SPACE] button to push forward. Be cautious with your fuel as it consumes when you move."
            ,
            "All time record: ", // + GetHighScore()
            () => startMinigame(),
            "Start",
            () => quitMinigameButton(),
            isQuestStepPresent
        );
    }

    void ShowResultMenu(string title, string status)
    {
        bool showQuit = isQuestStepPresent || playerHasWon;
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            title,
            status,
            "Time survived: " + timeElapsed,
            () => startMinigame(),
            "Retry",
            () => quitMinigameButton(),
            showQuit
        );
    }

    private void Update()
    {
        if (isGameActive)
        {
            timeElapsed += Time.deltaTime;
            timerText.text = "Time: " + timeElapsed.ToString("F1") + "s";
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);

        // reset game state when retry
        timeElapsed = 0f;
        isGameActive = true;
        rocket.restartRocket();
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
    }

    IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        spaceQuestStep?.playerWon();
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    public void minigameComplete(bool playerWon)
    {
        // Prevent double triggers
        // if (!isGameActive) return;

        isGameActive = false;
        playerHasWon = playerWon;
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();

        mainContentParent.SetActive(false);

        if (playerWon)
        {
            ShowResultMenu("Mission Accomplished!", "You have returned home safely.");
        }
        else ShowResultMenu("You Crashed!", "Try again and git gud.");
    }

    void playerTookDamage()
    {
        if (isGameActive) StartCoroutine(playerCrashed());
    }

    IEnumerator playerCrashed()
    {
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        isGameActive = false;

        yield return new WaitForSeconds(1f);
        minigameComplete(false);
    }
}
