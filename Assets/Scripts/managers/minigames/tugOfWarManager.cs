using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class tugOfWarManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private GameObject gameplayGameObject;
    // [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private rope rope;
    [SerializeField] private PlayerController minigamePlayer;
    [SerializeField] private GameObject npcObject;
    [SerializeField] private Slider gaugeBar;

    [Header("Quest Step")]
    [SerializeField] private tugOfWarQuestStep tugOfWarQuestStep;
    private bool isQuestStepPresent;

    [Header("Menu Panel")]
    // The UI must be disabled in the inspector to blend in with the crossfade
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    [Header("Choices Btns")]
    // public Question[] questions;
    // private List<Question> availableQuestions;
    // private Question currentQuestion;

    [SerializeField] private string[] rightAnswerPool;
    [SerializeField] private string[] wrongAnswerPool;
    private List<string> availableAnswers;
    private string currentAnswer;

    [Header("Game Config")]
    [SerializeField] private float questionHoldTime;
    [SerializeField] private float timeStopLength;
    [SerializeField] private float pullValue;
    [SerializeField] private float pullPowerBonus;
    [SerializeField] private float maxRopeDistance;


    private float correctStreak;
    private float timeElapsed;
    private bool isGameActive = false;
    private bool isQuestionActive = false;
    private bool isPowerUpActivated = false;
    private bool playerHasWon = false;

    // Hide the minigame before the crossfade
    IEnumerator Start()
    {
        mainContentParent.SetActive(false);
        gameplayGameObject.SetActive(false);
        gameEventsManager.instance.sceneEvents.startMinigame();

        yield return new WaitForSeconds(1f);

        isQuestStepPresent = tugOfWarQuestStep == null;
        Debug.Log("Quest Step Status: " + tugOfWarQuestStep);

        showStartMenu();
    }

    void showStartMenu()
    {
        gameplayGameObject.SetActive(true);
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            "Tug of War",
            "Choose answers that are related to contact force to pull the rope to your side!",
            "Best Time: ", // + GetHighScore()
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
            title == "VICTORY!" ? "Time Record: " + Mathf.FloorToInt(timeElapsed) + "s" : "",
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
            timerText.text = Mathf.FloorToInt(timeElapsed).ToString() + "s";
            gaugeBarUpdate();
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);

        // reset game state when retry
        timeElapsed = 0f;
        rope.currentRopeValue = 0f;
        correctStreak = 0f;
        isGameActive = true;
        isQuestionActive = true;

        // TODO: Make the player and npc walk to look like they're pulling the rope
        minigamePlayer.setAnimation(0);
        // npcObject.setAnimation(0);
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();

        setAnswerBoxes();
        StartCoroutine(AIAutoPull());
    }

    IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        gameEventsManager.instance.playerEvents.EnablePlayerMovement();


        tugOfWarQuestStep?.playerWon();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    void minigameComplete(bool playerWon)
    {
        minigamePlayer.setAnimation(1);

        isGameActive = false;
        isQuestionActive = false;
        StopCoroutine(AIAutoPull());
        availableAnswers.Clear();

        mainContentParent.SetActive(false);

        if (playerWon)
        {
            playerHasWon = true;
            ShowResultMenu("VICTORY!", "You pulled the rope to your side!");
        }
        else ShowResultMenu("DEFEAT!", "Your rival overpowered you. Try again!");
    }

    void setAnswerBoxes()
    {
        isQuestionActive = true;

        // storing all answers to availableAnswers
        if (availableAnswers == null || availableAnswers.Count == 0) availableAnswers = rightAnswerPool.ToList();

        // pick random right answer for this round
        int randAnswer = Random.Range(0, availableAnswers.Count);
        currentAnswer = availableAnswers[randAnswer];

        // prepare answer List (Correct + Wrong)
        List<string> answerPool = new List<string>{currentAnswer};

        List<string> filteredWrongPool = wrongAnswerPool.Where(ans => !rightAnswerPool.Contains(ans)).ToList();

        // fill remaining buttons with unique wrong answers
        int safetyCounter = 0;
        while (answerPool.Count < choiceButtons.Length && safetyCounter < 100)
        {
            string randWrong = filteredWrongPool[Random.Range(0, filteredWrongPool.Count)];

            if (!answerPool.Contains(randWrong))
            {
                answerPool.Add(randWrong);
            }
            safetyCounter++;
        }

        // Fill the rest with random answers from the global pool
        // Loop until we have enough answers for all buttons
        // int safetyCounter = 0;
        // while (answerPool.Count < choiceButtons.Length && safetyCounter < 100)
        // {
        //     // pick random wrong answers for current choices
        //     string randWrongAnswers = wrongAnswerPool[Random.Range(0, wrongAnswerPool.Length)];

        //     // Only add if it's NOT already in the list (no duplicates)
        //     if (!answerPool.Contains(randWrongAnswers))
        //     {
        //         answerPool.Add(randWrongAnswers);
        //     }
        //     safetyCounter++;
        // }

        // Shuffle the Answer Pool so the correct answer isn't always #1
        answerPool = shuffleList(answerPool);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button btnIndex = choiceButtons[i];

            // setup or reset btns color and interactivity
            btnIndex.interactable = true;
            btnIndex.onClick.RemoveAllListeners();

            ColorBlock cb = btnIndex.colors;
            cb.disabledColor = new Color(0.7f, 0.7f, 0.7f, 1f);
            btnIndex.colors = cb;
            btnIndex.GetComponentInChildren<TextMeshProUGUI>().color = Color.black;

            // set answers in boxes' texts
            if (i < answerPool.Count)
            {
                choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = answerPool[i];
            }

            // Capture variables for lambda
            string assignedStr = answerPool[i];
            int selectedBtn = i;

            choiceButtons[i].onClick.AddListener(() => onAnswerSelected(assignedStr, selectedBtn));
        }

    }

    void onAnswerSelected(string assignedStr, int selectedBtn)
    {
        if (!isQuestionActive) return; // Prevent double clicking

        bool isCorrect = assignedStr == currentAnswer;

        StartCoroutine(showFeedbackAndNext(isCorrect, selectedBtn));
    }


    IEnumerator showFeedbackAndNext(bool isCorrect, int selectedBtn)
    {
        isQuestionActive = false; // pause the gameplay
        ropePull(isCorrect);

        // Loop using index to access both Arrays easily
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button btn = choiceButtons[i];
            TextMeshProUGUI btnAnswer = btn.GetComponentInChildren<TextMeshProUGUI>();
            btn.interactable = false;

            if (i == selectedBtn)
            {
                ColorBlock cb = btn.colors;
                cb.disabledColor = isCorrect ? Color.blue : Color.red;
                btnAnswer.color = Color.white;
                btn.colors = cb;
            }
        }

        yield return new WaitForSeconds(questionHoldTime);

        availableAnswers.Remove(currentAnswer);
        setAnswerBoxes();
    }

    void ropePull(bool playerCorrect)
    {
        int direction = playerCorrect ? -1 : 0;
        if (playerCorrect) correctStreak++;
        else correctStreak = 0f;

        float totalPullForce = isPowerUpActivated ? pullValue + pullPowerBonus : pullValue;

        // Clamp the new value so it doesn't exceed the win/lose bounds
        rope.currentRopeValue = Mathf.Clamp(rope.currentRopeValue + (totalPullForce * direction), -maxRopeDistance, maxRopeDistance);
        checkGameState();
    }

    IEnumerator AIAutoPull()
    {
        while (isGameActive)
        {
            float AIPullSeconds = Random.Range(.5f, 1f);
            yield return new WaitForSeconds(AIPullSeconds);
            if (isQuestionActive && !isPowerUpActivated)
            {
                rope.currentRopeValue = Mathf.Clamp(rope.currentRopeValue + pullValue, -maxRopeDistance, maxRopeDistance);
                checkGameState();
            }
        }
    }

    void gaugeBarUpdate()
    {
        if (isPowerUpActivated) return;

        float requiredNumStreak = 10f;
        // 10 correct answers in a row will trigger stop time
        gaugeBar.value = Mathf.Clamp01(correctStreak / requiredNumStreak);

        if (correctStreak >= requiredNumStreak) StartCoroutine(stopTimePowerUp());

    }

    IEnumerator stopTimePowerUp()
    {
        isPowerUpActivated = true;
        float timeStopRemaining = timeStopLength;
        Debug.Log("Power Up Activated");

        while (timeStopRemaining >= 0f)
        {
            timeStopRemaining -= Time.deltaTime;
            gaugeBar.value = Mathf.Clamp01(timeStopRemaining / timeStopLength);
            Debug.Log(timeStopRemaining);
            yield return null;
        }

        correctStreak = 0f;
        isPowerUpActivated = false;
    }

    void checkGameState()
    {
        float value = rope.currentRopeValue;

        if (value <= -maxRopeDistance) minigameComplete(true);
        else if (value >= maxRopeDistance) minigameComplete(false);
        else Debug.Log("still pullin...");
    }

    // Helper to shuffle the list of sprites
    private List<string> shuffleList(List<string> inputList)
    {
        List<string> shuffled = new List<string>(inputList);
        int n = shuffled.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            string value = shuffled[k];
            shuffled[k] = shuffled[n];
            shuffled[n] = value;
        }
        return shuffled;
    }

    public float getMaxRopeDistance()
    {
        return maxRopeDistance;
    }
}