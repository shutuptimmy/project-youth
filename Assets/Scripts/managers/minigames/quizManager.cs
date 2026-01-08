using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class quizManager : MonoBehaviour
{
    [Header("Question Manager")]
    public Question[] questions;
    private static List<Question> unansweredQuestions;
    private Question currentQuestion;
    private int answeredCount;


    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private TextMeshProUGUI questionText;
    public Button[] choiceButtons = new Button[4];
    [SerializeField] private TextMeshProUGUI pagesList;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Menu Panel")]
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    [Header("Quest Step")]
    [SerializeField] private preTestTimeQuestStep preTestQuestStep;
    private bool isQuestStepPresent;

    [Header("Game Config")]
    [SerializeField] private float timer = 60f;
    [SerializeField] private float questionHoldTime = 2f;
    [SerializeField] private int xpPerCorrectAnswer = 20;

    private float timeElapsed;
    private int playerScore;
    private bool isGameActive = false;
    private bool isQuestionActive = false; // Essential for pausing timer during feedback

    IEnumerator Start()
    {
        gameplayGameObject.SetActive(false);
        mainContentParent.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();


        yield return new WaitForSeconds(1f);
        isQuestStepPresent = preTestQuestStep == null;
        Debug.Log("Quest Step Status: " + preTestQuestStep);

        showStartMenu();

    }

    private void showStartMenu()
    {
        gameplayGameObject.SetActive(true);

        minigameMenuPanelUI.activateMenu(
            "Pretest Time!",
            "Test your knowledge. There is no failing (for now), only learning!",
            "",
            () => startMinigame(),
            "Start",
            () => quitMinigameButton(),
            true
        );
    }

    private void ShowResultMenu(string title, string status, int xpGained)
    {
        // bool showQuit = isQuestStepPresent || playerHasWon;
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            title,
            status,
            $"Score: {playerScore}/{questions.Length}\nXP Gained: {xpGained}",
            () => quitMinigameButton(), // one time quiz
            "Finish",
            () => quitMinigameButton(),
            false // !showQuit
        );
    }

    private void Update()
    {
        if (isGameActive && isQuestionActive)
        {
            timeElapsed -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timeElapsed).ToString();

            if (timeElapsed <= 0) minigameComplete();
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);

        // reset game state when retry
        timeElapsed = timer;
        answeredCount = 0;
        playerScore = 0;
        isGameActive = true;

        unansweredQuestions = questions.ToList();

        SetCurrentQuestion();
    }

    private IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        preTestQuestStep?.quizFinished();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    private void minigameComplete()
    {
        isGameActive = false;
        isQuestionActive = false;
        unansweredQuestions.Clear();

        // 2. Calculate and Award XP
        int totalXPGained = playerScore * xpPerCorrectAnswer;
        // if (totalXPGained > 0) gameEventsManager.instance.playerEvents.ExperienceGained(totalXPGained);

        string feedbackMsg = playerScore == questions.Length ? "Perfect Score!" : "Good effort!";
        ShowResultMenu("PreTest Complete!", feedbackMsg, totalXPGained);
    }

    void SetCurrentQuestion()
    {
        // if the unansweredQuestions have ran out, finish the minigame
        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            minigameComplete();
            return;
        }

        isQuestionActive = true;

        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        answeredCount++;
        pagesList.text = $"{answeredCount}/{questions.Length}";
        questionText.text = $"{answeredCount}. {currentQuestion.description}";

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button btn = choiceButtons[i];
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            Image btnImg = btn.GetComponent<Image>(); // for coloring buttons when chosen

            // Check if this button is needed (in case a question has only 2 or 3 answers)
            if (i < currentQuestion.answers.Length)
            {
                btn.gameObject.SetActive(true);
                btn.interactable = true;
                btnImg.color = Color.white; // Reset Color
                btnText.text = currentQuestion.answers[i];

                // Remove old listeners
                btn.onClick.RemoveAllListeners();

                // 3. Lambda Listener to pass specific data
                string myAnswerText = currentQuestion.answers[i];
                GameObject myBtnObj = btn.gameObject;

                btn.onClick.AddListener(() => onAnswerSelected(myAnswerText, myBtnObj));
            }
            else
            {
                btn.gameObject.SetActive(false);
            }
        }
    }

    void onAnswerSelected(string selectedAnswer, GameObject clickedButtonObj)
    {
        if (!isQuestionActive) return;

        bool isCorrect = (selectedAnswer == currentQuestion.trueAnswer);

        if (isCorrect) playerScore++;

        StartCoroutine(showFeedbackAndNext(isCorrect, clickedButtonObj));
    }

    IEnumerator showFeedbackAndNext(bool isCorrect, GameObject clickedButtonObj)
    {
        isQuestionActive = false; // Pause Timer

        // 4. Feedback Logic
        foreach (Button btn in choiceButtons)
        {
            if (!btn.gameObject.activeSelf) continue;

            btn.interactable = false; // Disable clicking

            TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
            Image img = btn.GetComponent<Image>();

            // Always highlight the Correct Answer in Green
            if (txt.text == currentQuestion.trueAnswer)
            {
                img.color = Color.green;
            }
            // If this is the button we clicked, and it was WRONG, make it Red
            else if (clickedButtonObj == btn.gameObject && !isCorrect)
            {
                img.color = Color.red;
            }
            // Optional: Hide irrelevant wrong answers to reduce clutter
            else
            {
                // btn.gameObject.SetActive(false); 
            }
        }

        yield return new WaitForSeconds(questionHoldTime);

        unansweredQuestions.Remove(currentQuestion);
        SetCurrentQuestion();
    }
}
