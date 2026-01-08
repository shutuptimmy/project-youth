using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class sortingBoxesManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private GameObject minigamePlayer;

    [Header("Quest Step")]
    [SerializeField] private helpingHandQuestStep helpingHandQuestStep;
    private bool isQuestStepPresent;

    [Header("Menu Panel")]
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

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
    private bool isGameActive = false;
    private bool playerHasWon = false;

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
    IEnumerator Start()
    {
        gameplayGameObject.SetActive(false);
        mainContentParent.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();
        yield return new WaitForSeconds(1f);

        isQuestStepPresent = helpingHandQuestStep == null;
        Debug.Log("Quest Step Status: " + helpingHandQuestStep);

        showStartMenu();
    }

    void showStartMenu()
    {
        gameplayGameObject.SetActive(true);

        minigameMenuPanelUI.activateMenu(
            "Sorting the Boxes",
            "Move the boxes to the right place before the timer runs out!",
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

        minigameMenuPanelUI.activateMenu(
            title,
            status,
            "Total Score: " + playerScore,
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
            timeElapsed -= Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timeElapsed).ToString() + "s";
            if (timeElapsed < 0f) minigameComplete(false);
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);

        // reset game state when retry
        boxesRemaining = totalBoxes;
        playerScore = 0;
        scoreText.text = "Score: " + 0;
        timeElapsed = timeLimit;
        minigamePlayer.transform.position = new Vector2(0, 0);
        isGameActive = true;
        resetBoxes();

        gameEventsManager.instance.playerEvents.EnablePlayerMovement();
    }

    IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        helpingHandQuestStep?.playerWon();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    void minigameComplete(bool playerWon)
    {
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        isGameActive = false;

        mainContentParent.SetActive(false);

        if (playerWon)
        {
            playerHasWon = true;
            ShowResultMenu("All boxes cleared!", "You won!");
        }
        else ShowResultMenu("You Lost!", "Try again!");
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
        playerScore += 100 + (int)(timeLimit / timeElapsed);
        scoreText.text = "Score: " + playerScore.ToString();
        timeElapsed += timeBonus;
        disableBox(box);

        boxesRemaining--;
        if (boxesRemaining <= 0) minigameComplete(true);
    }

    public void wrongGoal(sortingBox box)
    {
        boxesRemaining--;
        disableBox(box);
        if (boxesRemaining <= 0) minigameComplete(true);

    }
}