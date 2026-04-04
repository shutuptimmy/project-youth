using System.Collections;
using TMPro;
using UnityEngine;

public abstract class MinigameManagerBase : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] protected GameObject mainContentParent;
    [SerializeField] protected GameObject gameplayGameObject;
    [SerializeField] protected TextMeshProUGUI timerText;

    [Header("Menu Panel")]
    [SerializeField] protected minigameMenuPanelUI minigameMenuPanelUI;
    
    protected bool isQuestStepPresent;
    protected bool isGameActive = false;
    protected bool playerHasWon = false;


    protected virtual IEnumerator Start()
    {
        mainContentParent.SetActive(false);
        gameplayGameObject.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();
        yield return new WaitForSeconds(1f);
        StartQuestStatus();
        StartMenuBase();
    }

    private void StartMenuBase()
    {
        gameplayGameObject.SetActive(true);
        mainContentParent.SetActive(false);
        ShowStartMenu();
    }

    protected void ResultMenuBase(string title, string status)
    {
        bool showQuit = isQuestStepPresent || playerHasWon; // 
        mainContentParent.SetActive(false);
        ShowResultMenu(title, status, showQuit);
    }

    protected void StartMinigameBase()
    {
        mainContentParent.SetActive(true);
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
        StartMinigame();
    }

    private IEnumerator QuitMinigameBase()
    {
        Debug.Log("sucess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);
        QuitMinigame();
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
        Destroy(gameObject);
    }

    public void QuitMinigameBtn()
    {
        StartCoroutine(QuitMinigameBase());
    }

    public void MinigameCompleteBase(bool playerWon)
    {
        isGameActive = false;
        playerHasWon = playerWon;
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        mainContentParent.SetActive(false);
        MinigameComplete(playerWon);
    }

    public abstract void StartQuestStatus();
    public abstract void ShowStartMenu();
    public abstract void StartMinigame();
    public abstract void MinigameComplete(bool resultCheck);
    public abstract void QuitMinigame();
    public abstract void ShowResultMenu(string title, string status, bool showQuit);
}
