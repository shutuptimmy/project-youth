using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sortingBoxesManager : MinigameManagerBase
{
    [Header("Additional Components")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject minigamePlayer;
    [SerializeField] private AudioClip correctSFX;
    [SerializeField] private AudioClip wrongSFX;

    [Header("Quest Step")]
    [SerializeField] private helpingHandQuestStep helpingHandQuestStep;

    [Header("Boxes Config")]
    [SerializeField] private Transform boxesParent;
    private List<BoxInitialState> originalBoxStates = new List<BoxInitialState>();
    private struct BoxInitialState
    {
        public sortingBox script;
        public Vector3 position;
        public Transform originalParent;
    }

    [Header("Box Info UI")]
    [SerializeField] private GameObject boxInfoPanel; // Panel showing Name & Image
    [SerializeField] private TextMeshProUGUI boxNameText;
    [SerializeField] private Image boxImageDisplay;

    [Header("Game Config")]
    [SerializeField] private float timeLimit = 61f;
    [SerializeField] private float timeBonus = 5f;
    [SerializeField] private int totalBoxes = 10;

    private float timeElapsed;
    private int playerScore;
    private int boxesRemaining;

    private void Awake()
    {
        foreach (Transform child in boxesParent)
        {
            sortingBox box = child.GetComponent<sortingBox>();
            if (box != null)
            {
                box.overridePlayer(minigamePlayer);

                BoxInitialState state = new BoxInitialState
                {
                    script = box,
                    position = child.position,
                    originalParent = boxesParent
                };
                originalBoxStates.Add(state);
            }
        }
    }

    // Hide the minigame before the crossfade
    public override void StartQuestStatus()
    {
        isQuestStepPresent = helpingHandQuestStep != null;
    }

    public override void ShowStartMenu()
    {
        minigameMenuPanelUI.activateMenu(
            "Sorting the Boxes",
            "Move the boxes to the right place before the timer runs out!",
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
            "Total Score: " + playerScore,
            () => StartMinigameBase(),
            "Retry",
            () => QuitMinigameBtn(),
            (playerHasWon && isQuestStepPresent)? "Complete Quest" : "Exit Minigame"
        );
    }

    private void Update()
    {
        if (isGameActive)
        {
            timeElapsed -= Time.deltaTime;
            timerText.text = $"Time: {Mathf.FloorToInt(timeElapsed)}s";
            if (timeElapsed < 0f) MinigameCompleteBase(false);
        }
    }

    public override void StartMinigame()
    {
        // reset game state when retry
        boxesRemaining = totalBoxes;
        playerScore = 0;
        scoreText.text = "Score: " + 0;
        timeElapsed = timeLimit;
        minigamePlayer.transform.position = new Vector2(0, 0);
        isGameActive = true;
        resetBoxes();
    }

    public override void QuitMinigame()
    {
        helpingHandQuestStep?.playerWon(playerHasWon);
    }

    public override void MinigameComplete(bool playerWon)
    {
        if (playerWon)
        {
            playerHasWon = true;
            ResultMenuBase("All boxes cleared!", "You won!");
        }
        else ResultMenuBase("You Lost!", "Try again!");
    }

    void resetBoxes()
    {
        foreach (var state in originalBoxStates)
        {
            GameObject obj = state.script.gameObject;

            // Ensure it's active
            obj.SetActive(true);

            // Reset Transforms
            obj.transform.SetParent(state.originalParent);
            obj.transform.position = state.position;

            // Reset Physics to stop momentum
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }

            // Reset internal script state (ensure it's not "dragged")
            state.script.forceReset();
        }
    }

    public void showBoxDetails(boxDataSO data)
    {
        boxInfoPanel.SetActive(true);
        boxNameText.text = data.boxName;
        boxImageDisplay.sprite = data.picture;
    }

    public void hideBoxDetails()
    {
        boxInfoPanel.SetActive(false);
        boxNameText.text = "";
        boxImageDisplay.sprite = null;

    }

    void disableBox(sortingBox box)
    {
        // Force release and disable box
        box.Release();
        box.gameObject.SetActive(false);
        hideBoxDetails();
    }

    public void correctGoal(sortingBox box)
    {
        soundFXManager.instance.playSoundClip(correctSFX, this.transform, 1f);
        playerScore += 100 + (int)(timeLimit / timeElapsed);
        scoreText.text = "Score: " + playerScore.ToString();
        timeElapsed += timeBonus;
        disableBox(box);

        boxesRemaining--;
        if (boxesRemaining <= 0) MinigameCompleteBase(true);
    }

    public void wrongGoal(sortingBox box)
    {
        soundFXManager.instance.playSoundClip(wrongSFX, this.transform, 1f);
        timeElapsed -= timeBonus;
        disableBox(box);

        boxesRemaining--;
        if (boxesRemaining <= 0) MinigameCompleteBase(true);

    }
}