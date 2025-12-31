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
    [SerializeField] private TextMeshProUGUI questionText;
    [SerializeField] private TextMeshProUGUI timerText;
    public GameObject[] choiceButtons;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private rope rope;
    [SerializeField] private PlayerController minigamePlayer;
    [SerializeField] private GameObject npcObject;

    [Header("Quest Step")]
    [SerializeField] private tugOfWarQuestStep tugOfWarQuestStep;
    private bool isQuestStepPresent;


    [Header("Menu Panel")]
    // The UI must be disabled in the inspector to blend in with the crossfade
    [SerializeField] private minigameMenuPanelUI minigameMenuPanelUI;

    [Header("Question Manager")]
    public Question[] questions;
    private List<Question> availableQuestions;
    private Question currentQuestion;

    [Header("Game Config")]
    [SerializeField] private float questionHoldTime = 0.7f;
    [SerializeField] private float pullValue = 0.3f;
    [SerializeField] private float rivalPullStrength = 0.05f;
    [SerializeField] private float maxRopeDistance = 1f;
    private float timeElapsed;
    private bool isGameActive = false;
    private bool isQuestionActive = false;
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
            "Answer questions correctly and quickly to pull the rope to your side!",
            "Best Time: ", // + GetHighScore()
            () => startMinigame(),
            "Start",
            () => quitMinigameButton(),
            isQuestStepPresent
        );
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        minigamePlayer.transform.position = new Vector2(-2f, 0.4866666f);
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
        }
    }

    public void startMinigame()
    {
        mainContentParent.SetActive(true);

        // reset game state when retry
        timeElapsed = 0f;
        rope.currentRopeValue = 0f;
        isGameActive = true;
        isQuestionActive = true;

        availableQuestions = questions.ToList();

        // TODO: Make the player and npc walk to look like they're pulling the rope
        minigamePlayer.setAnimation(0);

        SetCurrentQuestion();
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
        availableQuestions.Clear();

        mainContentParent.SetActive(false);

        // reset components
        questionText.text = "";
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

        if (playerWon)
        {
            playerHasWon = true;
            ShowResultMenu("VICTORY!", "You pulled the rope to your side!");
        }
        else ShowResultMenu("DEFEAT!", "Your rival overpowered you. Try again!");
    }

    void SetCurrentQuestion()
    {
        isQuestionActive = true;

        if (availableQuestions == null || availableQuestions.Count == 0) availableQuestions = questions.ToList();

        int randomQuestionIndex = Random.Range(0, availableQuestions.Count);
        currentQuestion = availableQuestions[randomQuestionIndex];

        questionText.text = currentQuestion.description;

        // deactivate all choice buttons
        for (int i = 0; i < choiceButtons.Length; i++) choiceButtons[i].SetActive(false);

        // Then, set up and activate only the buttons for the current question's answers
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            if (i < choiceButtons.Length) // safety check to prevent array out of bounds
            {
                choiceButtons[i].SetActive(true);
                TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                choiceText.text = currentQuestion.answers[i];
                Button button = choiceButtons[i].GetComponent<Button>();

                // Reset button color and interactivity
                button.interactable = true;
                ColorBlock cb = button.colors;
                cb.normalColor = Color.white; // Set to your default color
                button.colors = cb;

                button.onClick.RemoveAllListeners();

                if (choiceText.text == currentQuestion.trueAnswer) button.onClick.AddListener(userSelectTrue);
                else button.onClick.AddListener(userSelectFalse);
            }
        }
    }

    public void transitionToNextQuestion()
    {
        // Remove the current question from the list
        availableQuestions.Remove(currentQuestion);

        SetCurrentQuestion();

        // Re-enable all buttons and reset their colors
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].GetComponent<Button>().interactable = true;
            TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            choiceText.color = Color.black; // Or your default text color

            Button button = choiceButtons[i].GetComponent<Button>();
            ColorBlock cb = button.colors;
            cb.normalColor = Color.white;
            cb.selectedColor = Color.white;
            cb.disabledColor = Color.white;
            button.colors = cb;
        }
    }

    void ropePull(bool playerCorrect)
    {
        int direction = playerCorrect ? -1 : 1;

        // Clamp the new value so it doesn't exceed the win/lose bounds
        rope.currentRopeValue = Mathf.Clamp(rope.currentRopeValue + (pullValue * direction), -maxRopeDistance, maxRopeDistance);
        checkGameState();
    }

    IEnumerator AIAutoPull()
    {
        while (isGameActive)
        {
            float AIPullSeconds = Random.Range(.5f, 1.0f);
            yield return new WaitForSeconds(AIPullSeconds);
            if (isQuestionActive)
            {
                rope.currentRopeValue = Mathf.Clamp(rope.currentRopeValue + rivalPullStrength, -maxRopeDistance, maxRopeDistance);
                checkGameState();
            }
        }
    }

    void checkGameState()
    {
        float value = rope.currentRopeValue;

        if (value <= -maxRopeDistance) minigameComplete(true);
        else if (value >= maxRopeDistance) minigameComplete(false);
        else Debug.Log("still pullin...");
    }


    IEnumerator WaitAndNextQuestion(bool playerWasCorrect)
    {
        isQuestionActive = false;
        yield return new WaitForSeconds(questionHoldTime);
        availableQuestions.Remove(currentQuestion);
        transitionToNextQuestion();
        isQuestionActive = true;
    }

    public void userSelectTrue()
    {
        Debug.Log("eyy it works!");
        DisableButtons(false);
        ropePull(true); // Player pulls towards their side
        StartCoroutine(WaitAndNextQuestion(true));
    }

    public void userSelectFalse()
    {
        Debug.Log("EEE Wong!");
        DisableButtons(false);
        ropePull(false); // Player pulls towards the rival side
        StartCoroutine(WaitAndNextQuestion(false));
    }

    void DisableButtons(bool playerSelectedCorrect)
    {
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Button button = choiceButtons[i].GetComponent<Button>();
            button.interactable = false; // Disable all buttons

            ColorBlock cb = button.colors;

            if (choiceText.text == currentQuestion.trueAnswer)
            {
                // Correct answer is always GREEN
                cb.disabledColor = Color.green;
            }
            else
            {
                // Incorrect answers turn RED if the player selected it
                if (!playerSelectedCorrect)
                {
                    cb.disabledColor = Color.red;
                    choiceText.color = Color.white;
                }
            }
            button.colors = cb;
        }
    }

    public float getMaxRopeDistance()
    {
        return maxRopeDistance;
    }
}