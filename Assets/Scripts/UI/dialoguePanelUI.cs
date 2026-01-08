using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;

public class dialoguePanelUI : MonoBehaviour, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private GameObject contentParent;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private dialogueChoiceButton[] choiceButtons;
    [SerializeField] private Animator portraitAnimator;

    private string playerGender;

    private const string CHAR_NAME = "name";
    private const string CHAR_PORTRAIT = "char";

    private void Awake()
    {
        contentParent.SetActive(false);
        resetPanel();
    }

    private void OnEnable()
    {
        gameEventsManager.instance.dialogueEvents.onDialogueStarted += dialogueStarted;
        gameEventsManager.instance.dialogueEvents.onDialogueFinished += dialogueFinished;
        gameEventsManager.instance.dialogueEvents.onDisplayDialogue += displayDialogue;
    }

    private void OnDisable()
    {

        gameEventsManager.instance.dialogueEvents.onDialogueStarted -= dialogueStarted;
        gameEventsManager.instance.dialogueEvents.onDialogueFinished -= dialogueFinished;
        gameEventsManager.instance.dialogueEvents.onDisplayDialogue -= displayDialogue;
    }

    private void dialogueStarted()
    {

        contentParent.SetActive(true);
    }

    private void dialogueFinished()
    {

        contentParent.SetActive(false);
        resetPanel();
    }

    private void displayDialogue(string dialogueLine, List<Choice> dialogueChoices, List<string> dialogueTags)
    {
        dialogueText.text = dialogueLine;


        // Handles Tags
        foreach (string tag in dialogueTags)
        {
            // parse the tag
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2)
            {
                Debug.LogError("Tag could not be appropriately parsed: " + tag);
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            // handle the tag
            switch (tagKey)
            {
                case CHAR_NAME:
                    dialogueName.text = tagValue;
                    break;

                case CHAR_PORTRAIT:
                    if (tagValue == "you") portraitAnimator.Play(playerGender);
                    else portraitAnimator.Play(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag came in but is not currently handled: " + tag);
                    break;
            }
        }

        // Choice Button functions

        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("Detected more than 4 choice selections in ink file: " + dialogueChoices.Count);
        }

        // hide all choice buttons at start
        foreach (dialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        int choiceButtonIndex = dialogueChoices.Count - 1;

        // enable and set info for buttons depending on ink choice info
        for (int i = 0; i < dialogueChoices.Count; i++)
        {
            Choice dialogueChoice = dialogueChoices[i];
            dialogueChoiceButton choiceButton = choiceButtons[i];

            choiceButton.gameObject.SetActive(true);
            choiceButton.setChoiceText(dialogueChoice.text);
            choiceButton.setChoiceIndex(i);

            if (i == 0)
            {
                choiceButton.selectButton();
                gameEventsManager.instance.dialogueEvents.UpdateChoiceIndex(0);
            }

            choiceButtonIndex--;
        }
    }

    private void resetPanel()
    {
        dialogueText.text = "";
        dialogueName.text = "";
        // portraitAnimator = null;
    }

    public void loadData(gameData data)
    {
        switch (data.playerGender)
        {
            case 0:
                playerGender = "mcBoy";
                break;
            case 1:
                playerGender = "mcGirl";
                break;
            default:
                Debug.LogWarning("This gender value doesn't exist: " + data.playerGender);
                break;
        }
    }

    public void saveData(gameData data) { }
}
