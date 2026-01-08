using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class imageQuizManager : MonoBehaviour
{
    [System.Serializable]
    public class imageQuestion
    {
        public string description;
        public Sprite trueAnswer;
    }
    [Header("Question Manager")]
    public imageQuestion[] questions;
    [SerializeField] private Sprite[] imagePool; // all images goes here
    private List<imageQuestion> availableQuestions;
    private imageQuestion currentQuestion;

    [Header("Components")]
    [SerializeField] private GameObject mainContentParent;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private TextMeshProUGUI descriptionText;
    public Button[] choiceButtons = new Button[4]; // up to 4 buttons
    public Image[] choiceImages = new Image[4]; // up to 4 picture

    [Header("Menu Panel")]
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    [Header("Left Panel")]
    [SerializeField] private TextMeshProUGUI pagesList;
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite hourglassSprite;
    [SerializeField] private Sprite correctSprite;
    [SerializeField] private Sprite wrongSprite;
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Quest Step")]
    [SerializeField] private knowledgeTestQuestStep knowledgeTestQuestStep;
    private bool isQuestStepPresent;

    [Header("Game Config")]
    [SerializeField] private float timeLimit = 10f;
    [SerializeField] private float questionHoldTime = 2f;
    [SerializeField] private int passingScore = 7;

    private int answeredQuestions;
    private int totalQuestions;
    private int playerScore;
    private float currentTime;
    private bool isGameActive = false;
    private bool isQuestionActive = false; // Essential for pausing timer during feedback
    private bool playerHasWon = false;

    private IEnumerator Start()
    {
        gameplayGameObject.SetActive(false);
        mainContentParent.SetActive(false);

        gameEventsManager.instance.sceneEvents.startMinigame();

        // Safety Check: Ensure we have enough images to fill the buttons
        if (imagePool.Length < choiceButtons.Length)
        {
            Debug.LogError("Not enough images in Global Image Pool to fill buttons!");
        }

        yield return new WaitForSeconds(1f);
        isQuestStepPresent = knowledgeTestQuestStep == null;
        Debug.Log("Quest Step Status: " + knowledgeTestQuestStep);

        showStartMenu();
    }

    private void showStartMenu()
    {
        gameplayGameObject.SetActive(true);

        resultImage.color = Color.clear;
        minigameMenuPanelUI.activateMenu(
            "Guess the Force",
            "Select a picture that matches the description before the timer runs out.",
            "Highest Score: ", // + GetHighScore()
            () => startMinigame(),
            "Start",
            () => quitMinigameButton(),
            isQuestStepPresent
        );
    }

    private void ShowResultMenu(string title, string status)
    {
        bool showQuit = isQuestStepPresent || playerHasWon;
        mainContentParent.SetActive(false);

        minigameMenuPanelUI.activateMenu(
            title,
            status,
            "Final Score: " + playerScore + "/" + questions.Length,
            () => startMinigame(),
            "Retry",
            () => quitMinigameButton(),
            showQuit
        );
    }

    private void Update()
    {
        if (isGameActive && isQuestionActive)
        {
            currentTime -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(currentTime).ToString();

            if (currentTime <= 0f) handleTimeOut();
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);

        // reset game state when retry
        resultImage.color = Color.white;
        resultImage.sprite = hourglassSprite;
        answeredQuestions = 1;
        playerScore = 0;
        isGameActive = true;


        availableQuestions = questions.ToList();
        totalQuestions = availableQuestions.Count;

        SetCurrentQuestion();
    }

    private IEnumerator quitMinigame()
    {
        Debug.Log("suceess");
        gameEventsManager.instance.sceneEvents.quitMinigame();
        yield return new WaitForSeconds(1f);

        knowledgeTestQuestStep?.playerWon();

        Destroy(gameObject);
    }

    public void quitMinigameButton()
    {
        StartCoroutine(quitMinigame());
    }

    private void minigameComplete(bool playerWon)
    {

        isGameActive = false;
        isQuestionActive = false;
        availableQuestions.Clear();

        mainContentParent.SetActive(false);

        // reset components
        descriptionText.text = "";

        if (playerWon)
        {
            playerHasWon = true;
            ShowResultMenu("You have Passed!", "You aced the test!");
        }
        else
        {
            ShowResultMenu("Nice Try!", "Try again with by trusting your gut.");
        }
    }

    private void SetCurrentQuestion()
    {
        // if the availableQuestions have ran out, finish the minigame
        if (availableQuestions == null || availableQuestions.Count == 0)
        {
            if (playerScore >= passingScore) minigameComplete(true);
            else minigameComplete(false);
            return;
        }

        resultImage.sprite = hourglassSprite;
        int randomQuestionIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[randomQuestionIndex];

        descriptionText.text = currentQuestion.description;
        currentTime = timeLimit;
        pagesList.text = answeredQuestions + "/" + totalQuestions;
        isQuestionActive = true;

        // Prepare Answer List (Correct + Wrong)
        List<Sprite> answerPool = new List<Sprite>();
        answerPool.Add(currentQuestion.trueAnswer);

        // Fill the rest with random images from the global pool
        // Loop until we have enough images for all buttons
        int safetyCounter = 0;
        while (answerPool.Count < choiceButtons.Length && safetyCounter < 100)
        {
            Sprite randSprite = imagePool[Random.Range(0, imagePool.Length)];

            // Only add if it's NOT the true answer AND NOT already in the list (no duplicates)
            if (randSprite != currentQuestion.trueAnswer && !answerPool.Contains(randSprite))
            {
                answerPool.Add(randSprite);
            }
            safetyCounter++;
        }

        // Shuffle the Answer Pool so the correct answer isn't always #1
        answerPool = shuffleList(answerPool);

        // Setup Buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            // 1. Setup the clickable Button (Parent)
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].interactable = true;
            choiceButtons[i].onClick.RemoveAllListeners();

            // 2. Setup the visual Image (Child)
            // We use the NEW array here, so we don't accidentally grab the parent background
            choiceImages[i].color = Color.white;

            if (i < answerPool.Count)
            {
                choiceImages[i].sprite = answerPool[i];
            }

            // Capture variables for lambda
            Sprite assignedSprite = answerPool[i];
            int btnIndex = i; // Save index to identify button later

            choiceButtons[i].onClick.AddListener(() => onAnswerSelected(assignedSprite, btnIndex));
        }
    }

    // Helper to shuffle the list of sprites
    private List<Sprite> shuffleList(List<Sprite> inputList)
    {
        List<Sprite> shuffled = new List<Sprite>(inputList);
        int n = shuffled.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            Sprite value = shuffled[k];
            shuffled[k] = shuffled[n];
            shuffled[n] = value;
        }
        return shuffled;
    }

    void onAnswerSelected(Sprite selectedSprite, int btnIndex)
    {
        if (!isQuestionActive) return; // Prevent double clicking

        bool isCorrect = (selectedSprite == currentQuestion.trueAnswer);
        answeredQuestions++;

        if (isCorrect) playerScore++;

        StartCoroutine(showFeedbackAndNext(isCorrect, btnIndex));
    }

    void handleTimeOut()
    {
        // Pass the index instead of the GameObject to simplify array lookups
        StartCoroutine(showFeedbackAndNext(false, -1));
    }

    IEnumerator showFeedbackAndNext(bool isCorrect, int clickedBtnIndex)
    {
        isQuestionActive = false; // pause the gameplay

        // Loop using index to access both Arrays easily
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            Button btn = choiceButtons[i];
            Image img = choiceImages[i]; // Access the child image directly

            btn.interactable = false;

            if (img.sprite == currentQuestion.trueAnswer)
            {
                // Highlight correct answer (Green is standard feedback)
                img.color = Color.white;
            }
            else
            {
                if (isCorrect) btn.gameObject.SetActive(false);
            }
        }

        // show result
        timerText.text = isCorrect ? "Correct!" : "Wrong!";
        resultImage.sprite = isCorrect ? correctSprite : wrongSprite;

        // If player clicked WRONG, turn THAT child image red
        if (!isCorrect && clickedBtnIndex != -1)
        {
            choiceImages[clickedBtnIndex].color = Color.red;
        }

        yield return new WaitForSeconds(questionHoldTime);

        availableQuestions.Remove(currentQuestion);
        SetCurrentQuestion();
    }
}
