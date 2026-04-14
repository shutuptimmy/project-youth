using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fragileDeskManager : MinigameManagerBase
{
    [Header("Additional Components")]
    [SerializeField] private desk desk;
    [SerializeField] private TextMeshProUGUI objPlacedText;
    [SerializeField] private GameObject stabilizeUI;
    [SerializeField] private TextMeshProUGUI stabilizeTimerText;

    [Header("Quest Step")]
    [SerializeField] private avoidAccountabilityQuestStep avoidAccountabilityQuestStep;

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
    public new bool isGameActive { get; private set; } = false;
    private bool areObjectsStill = false;

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

    public override void StartQuestStatus()
    {
        isQuestStepPresent = avoidAccountabilityQuestStep != null;
    }

    private void Update()
    {
        if (!isGameActive) return;
        if (!areObjectsStill) timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0 || desk.isBroken)
        {
            isGameActive = false;
            StartCoroutine(DeskDestroyed());
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

            if (currentStableTimer >= stableTimeRequired)
            {
                isGameActive = false;
                MinigameCompleteBase(true);
            }
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

    public override void ShowStartMenu()
    {
        minigameMenuPanelUI.activateMenu(
            "Fragile Desk",
            "Drag the objects and carefully place them to the desk with your shaky hand (mouse) before the timer runs out. Careful not to drop too hard."
            ,
            "All time record: ", // + GetHighScore()
            () => StartMinigameBase(),
            "Start",
            () => QuitMinigameBtn(),
            "Exit Minigame"
        );
    }

    public override void ShowResultMenu(string title, string status)
    {
        minigameMenuPanelUI.activateMenu(
            title,
            status,
            "Time saved: " + timeRemaining,
            () => StartMenuBase(),
            "Retry",
            () => QuitMinigameBtn(),
            (playerHasWon && isQuestStepPresent)? "Complete Quest" : "Exit Minigame"
        );
    }

    public override void StartMinigame()
    {
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
    }

    public override void QuitMinigame()
    {
        avoidAccountabilityQuestStep?.playerWon(playerHasWon);
    }

    public override void MinigameComplete(bool playerWon)
    {
        if (playerWon) ResultMenuBase("Good Job!", "Nobody knows the difference.");
        else ResultMenuBase("You Failed!", desk.isBroken ? "The desk collapsed!" : "Time ran out!");
    }

    IEnumerator DeskDestroyed()
    {
        mainContentParent.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        MinigameCompleteBase(false);
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
