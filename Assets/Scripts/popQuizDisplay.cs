using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class popQuizDisplay : MonoBehaviour
{
    public PopQuiz popQuiz;
    public TextMeshProUGUI description;
    // public GameObject[] choiceButtons;
    public GameObject trueButton;
    public GameObject falseButton;
    public TextMeshProUGUI factAnswer;

    void Start()
    {
        description.text = popQuiz.questionDescription;
        var trueText = trueButton.GetComponentInChildren<TextMeshProUGUI>();
        var falseText = falseButton.GetComponentInChildren<TextMeshProUGUI>();
        trueText.text = popQuiz.quizChoices[0];
        falseText.text = popQuiz.quizChoices[1];
        factAnswer.text = "";
    }

    public void userSelectTrue()
    {
        var button = trueButton.GetComponent<Button>();
        button.interactable = false;
        falseButton.SetActive(false);

        if (popQuiz.isTrue)
        {
            factAnswer.text = popQuiz.fact;
            gameEventsManager.instance.playerEvents.ExperienceGained(10);
        }
        else
        {
            factAnswer.text = popQuiz.wrong;
        }

    }

    public void userSelectFalse()
    {
        var button = falseButton.GetComponent<Button>();
        button.interactable = false;
        trueButton.SetActive(false);

        if (!popQuiz.isTrue)
        {
            factAnswer.text = popQuiz.fact;
        }
        else
        {
            factAnswer.text = popQuiz.wrong;
        }
    }
}
