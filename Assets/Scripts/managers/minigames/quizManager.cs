using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class quizManager : MonoBehaviour
{
    [Header("Question Manager")]
    public Question[] questions;
    private static List<Question> unansweredQuestions;


    [Header("Main UI Panel")]

    [SerializeField] private GameObject contentParent;
    [SerializeField] private float timeLimit;
    [SerializeField] private GameObject openingUI;


    [SerializeField] private TextMeshProUGUI factDescription;
    public GameObject[] choiceButtons;
    [SerializeField] private TextMeshProUGUI pagesList;
    [SerializeField] private GameObject nextButton;


    [Header("Result UI Panel")]
    [SerializeField] private GameObject resultUI;
    [SerializeField] private TextMeshProUGUI scoreText;
    public pretestTimeQuestStep pretestQuestStep;



    private int totalQuestions;
    private Question currentQuestion;
    private int playerScore;

    private void Start()
    {
        openingUI.SetActive(true);
        factDescription.text = "";
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Start Quiz";
        nextButton.GetComponent<Button>().onClick.AddListener(startQuiz);

    }

    public void startQuiz()
    {
        openingUI.SetActive(false);

        if (unansweredQuestions == null || unansweredQuestions.Count == 0)
        {
            unansweredQuestions = questions.ToList<Question>();
            totalQuestions = unansweredQuestions.Count;
        }
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(true);
        }
        nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "Next";
        nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
        nextButton.GetComponent<Button>().onClick.AddListener(transitionToNextQuestion);
        SetCurrentQuestion();
    }

    void SetCurrentQuestion()
    {
        int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
        currentQuestion = unansweredQuestions[randomQuestionIndex];

        factDescription.text = currentQuestion.description;
        pagesList.text = questions.Length - unansweredQuestions.Count + 1 + "/" + questions.Length.ToString();
        nextButton.SetActive(false);

        // First, deactivate all choice buttons
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            choiceButtons[i].SetActive(false);
        }

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

                if (choiceText.text == currentQuestion.trueAnswer)
                {
                    button.onClick.AddListener(userSelectTrue);
                }
                else
                {
                    button.onClick.AddListener(userSelectFalse);
                }
            }
        }
    }

    public void transitionToNextQuestion()
    {
        // Remove the current question from the list
        unansweredQuestions.Remove(currentQuestion);

        // If there are more questions, set up the next one
        if (unansweredQuestions.Count > 0)
        {
            SetCurrentQuestion(); // Call the method to load the next question in the same scene

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
        else
        {
            int playerScoreXP = playerScore * 8;
            Debug.Log("Quiz finished! Earned " + playerScoreXP + " XP");
            gameEventsManager.instance.playerEvents.ExperienceGained(playerScoreXP);

            resultUI.SetActive(true);
            nextButton.SetActive(true);

            factDescription.text = "";
            for (int i = 0; i < choiceButtons.Length; i++)
            {
                choiceButtons[i].SetActive(false);
            }

            scoreText.text = "You have scored " + playerScore.ToString() + " out of " + totalQuestions + "! " + playerScoreXP.ToString() + " gained!";

            nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "finish";
            nextButton.GetComponent<Button>().onClick.RemoveAllListeners();
            // You might want to add a function to handle the "finish" button, like loading a final scene or quitting the game.
            nextButton.GetComponent<Button>().onClick.AddListener(pretestCompleted);

        }
    }

    public void pretestCompleted()
    {
        pretestQuestStep.quizFinished();
        contentParent.SetActive(false);
    }

    public void userSelectTrue()
    {
        Debug.Log("eyy it works!");
        nextButton.SetActive(true);

        // highlight the selected button to blue and disable other buttons
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Button button = choiceButtons[i].GetComponent<Button>();

            if (choiceText.text == currentQuestion.trueAnswer)
            {
                ColorBlock cb = button.colors;
                cb.selectedColor = Color.blue;
                cb.disabledColor = Color.blue;
                button.colors = cb;
                choiceText.color = Color.white;
                playerScore += 1;
            }
            button.interactable = false;
        }
    }

    public void userSelectFalse()
    {
        Debug.Log("EEE Wong!");
        nextButton.SetActive(true);

        // highlight the selected button to red and disable all buttons except correct button
        for (int i = 0; i < currentQuestion.answers.Length; i++)
        {
            TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            Button button = choiceButtons[i].GetComponent<Button>();

            if (choiceText.text != currentQuestion.trueAnswer)
            {
                ColorBlock cb = button.colors;
                cb.disabledColor = Color.red;
                button.colors = cb;
                choiceText.color = Color.white;
            }
            else
            {
                ColorBlock cb = button.colors;
                cb.disabledColor = Color.green;
                button.colors = cb;
            }
            button.interactable = false;
        }
    }

    // old ones
    // void SetCurrentQuestion()
    // {
    //     int randomQuestionIndex = Random.Range(0, unansweredQuestions.Count);
    //     currentQuestion = unansweredQuestions[randomQuestionIndex];

    //     factDescription.text = currentQuestion.description;
    //     // pagesList.text = 1 + "/" + questions.Length.ToString();
    //     nextButton.SetActive(false);

    //     for (int i = 0; i < currentQuestion.answers.Length; i++)
    //     {
    //         choiceButtons[i].SetActive(true);
    //         TextMeshProUGUI choiceText = choiceButtons[i].GetComponentInChildren<TextMeshProUGUI>();
    //         choiceText.text = currentQuestion.answers[i];
    //         Button button = choiceButtons[i].GetComponent<Button>();

    //         button.onClick.RemoveAllListeners();

    //         if (choiceText.text == currentQuestion.trueAnswer)
    //         {
    //             button.onClick.AddListener(userSelectTrue);
    //         }
    //         else
    //         {
    //             button.onClick.AddListener(userSelectFalse);
    //         }
    //     }

    // }

    // // public void transitionToNextQuestion()
    // // {
    // //     unansweredQuestions.Remove(currentQuestion);
    // //     SceneManager.LoadScene("PreTest");
    // // }

    // // for test result UI
    // public void transitionToNextQuestion()
    // {
    //     // Remove the current question from the list
    //     unansweredQuestions.Remove(currentQuestion);

    //     // If there are more questions, set up the next one
    //     if (unansweredQuestions.Count > 0)
    //     {
    //         SceneManager.LoadScene("PreTest");

    //     }
    //     else
    //     {
    //         Debug.Log("Quiz finished!");
    //         resultUI.SetActive(true);

    //         factDescription.text = "";
    //         for (int i = 0; i < choiceButtons.Length; i++)
    //         {
    //             choiceButtons[i].SetActive(false);
    //         }

    //         scoreText.text = "You have scored " + playerScore.ToString() + " out of " + totalQuestions;

    //         nextButton.GetComponentInChildren<TextMeshProUGUI>().text = "finish";
    //     }
    // }
}
