using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fragileDeskManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private desk desk;
    [SerializeField] private TextMeshProUGUI objPlacedText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject stabilizeUI;
    [SerializeField] private TextMeshProUGUI stabilizeTimerText;

    [Header("Quest Step")]
    [SerializeField] private avoidAccountabilityQuestStep avoidAccountabilityQuestStep;
    private bool isQuestStepPresent;

    [Header("Menu Panel")]
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    [Header("Game Config")]
    [SerializeField] private float timeLimit;
    [SerializeField] private float stableTimeRequired;
    [SerializeField] private Transform objsParent;
    private List<ObjInitialState> originalObjStates = new List<ObjInitialState>();
    private struct ObjInitialState
    {
        public draggableMassObject script;
        public Vector3 position;
        public Transform originalParent;
    }

    private int totalObjects;
    private float currentStableTimer;
    private float timeRemaining;
    private bool setObjsActive;
    public bool isGameActive { get; private set; } = false;
    private bool areObjectsStill = false;
    private bool playerHasWon = false;

    void Awake()
    {
        foreach (Transform child in objsParent)
        {
            draggableMassObject obj = child.GetComponent<draggableMassObject>();
            if (obj != null)
            {
                ObjInitialState state = new ObjInitialState
                {
                    script = obj,
                    position = child.position,
                    originalParent = objsParent
                };
                originalObjStates.Add(state);
                totalObjects++;
            }
        }
    }

    IEnumerator Start()
    {
        mainContentParent.SetActive(false);
        gameplayGameObject.SetActive(false);

        // hide all objects at start of the game
        setObjsActive = false;
        objsParent.gameObject.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();
        yield return new WaitForSeconds(1f);

        // isQuestStepPresent = avoidAccountabilityQuestStep == null;
        // Debug.Log("Quest Step Status: " + avoidAccountabilityQuestStep);

        showStartMenu();
    }

    void showStartMenu()
    {
        gameplayGameObject.SetActive(true);
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            "Fragile Desk",
            "Drag the objects and carefully place them to the desk with your shaky mouse before the timer runs out. Careful not to drop too hard."
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
            "Time saved: " + timeRemaining,
            () => startMinigame(),
            "Retry",
            () => quitMinigameButton(),
            showQuit
        );
    }

    private void Update()
    {
        if (!isGameActive) return;
        if (!areObjectsStill) timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0 || desk.isBroken)
        {
            StartCoroutine(minigameComplete(false));
            return;
        }

        int currentPlacedObjects = desk.objInZone.Count;
        int restedObjects = desk.getTotalRestedObjs();
        objPlacedText.text = $"Total Objects: {currentPlacedObjects} / {totalObjects}";

        // Check Win Conditions
        if (restedObjects >= totalObjects && desk.areObjectsStable())
        {
            areObjectsStill = true;
            stabilizeUI.SetActive(true);
            timerText.gameObject.SetActive(false);

            currentStableTimer += Time.deltaTime;
            stabilizeTimerText.text = $"{(stableTimeRequired - currentStableTimer):F1}s";

            if (currentStableTimer >= stableTimeRequired) StartCoroutine(minigameComplete(true));
        }
        else
        {
            currentStableTimer = 0f;
            areObjectsStill = false;

            stabilizeUI.SetActive(false);
            timerText.gameObject.SetActive(true);
        }
        timerText.text = Mathf.CeilToInt(timeRemaining).ToString();
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);
        stabilizeUI.SetActive(false);

        if (!setObjsActive)
        {
            setObjsActive = true;
            objsParent.gameObject.SetActive(true);
        }

        // reset game state when retry
        timeRemaining = timeLimit;
        currentStableTimer = 0f;
        isGameActive = true;
        desk.resetDesk();
        resetObjs();
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
    }

    IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        avoidAccountabilityQuestStep?.playerWon();
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    IEnumerator minigameComplete(bool playerWon)
    {

        isGameActive = false;
        playerHasWon = playerWon;
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();

        mainContentParent.SetActive(false);
        yield return new WaitForSeconds(1.5f);

        if (playerWon) ShowResultMenu("Good Job!", "Nobody knows the difference.");
        else ShowResultMenu("You Failed!", desk.isBroken ? "The desk collapsed!" : "Time ran out!");
    }

    void resetObjs()
    {
        foreach (var state in originalObjStates)
        {
            GameObject obj = state.script.gameObject;
            // Reset Transforms
            obj.transform.SetParent(state.originalParent);
            obj.transform.SetPositionAndRotation(state.position, Quaternion.Euler(Vector3.zero));
        }
    }
}
