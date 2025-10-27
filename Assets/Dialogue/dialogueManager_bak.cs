using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class dialogueManager_bak : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueName;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Animator portraitAnimator;
    // Add player level
    private int playerLevel;

    [Header("Load Globals JSON")]
    [SerializeField] private TextAsset loadGlobalsJSON;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;
    // Add Button Levels
    private TextMeshProUGUI[] choicesLevelText;
    private int choicesLevel;

    private Story currentStory;
    public bool isDialoguePlayin { get; private set; }


    private static dialogueManager_bak instance;

    private const string CHAR_NAME = "charName";
    private const string CHAR_PORTRAIT = "charPortrait";
    // private const string BTN_LEVEL_REQ = "btnLvl";

    private dialogueVariables dialogueVariables;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("Found more than one dialogue manager in the scene. Removing duplicate..");
            Destroy(gameObject);
            return;
        }
        instance = this;

        dialogueVariables = new dialogueVariables(loadGlobalsJSON);
    }

    public static dialogueManager_bak GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        isDialoguePlayin = false;
        dialoguePanel.SetActive(false);

        // get all the choices and required level texts if present in dialogue
        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }


    }

    private void Update()
    {
        // return right away if dialogue isnt playing
        if (!isDialoguePlayin)
        {
            return;
        }

        // handle continuing to the next line in the dialogue when submit is pressed
        // if (inputManager.GetInstance().GetSubmitPressed())
        // {
        //     continueStory();
        // }
    }


    public void ResumeTimeline()
    {
        // Find the current playable director in the scene
        PlayableDirector director = FindObjectOfType<PlayableDirector>();

        // Check if the director is playing and not paused, then resume
        if (director != null)
        {
            director.playableGraph.GetRootPlayable(0).SetSpeed(1);
        }
    }


    public void enterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        isDialoguePlayin = true;
        dialoguePanel.SetActive(true);

        dialogueVariables.startListening(currentStory);

        currentStory.BindExternalFunction("externalCall", (string callCommand) =>
        {
            SceneManager.LoadScene(callCommand, LoadSceneMode.Additive);
        });

        // reset portrait
        dialogueName.text = "???";
        continueStory();
    }

    private IEnumerator exitDialogueMode()
    {
        // wait for 0.2 seconds after finishing dialogue to avoid input actions, especially if submit and jump buttons are same inputs.
        yield return new WaitForSeconds(0.2f);

        dialogueVariables.stopListening(currentStory);
        currentStory.UnbindExternalFunction("externalCall");

        isDialoguePlayin = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";

        ResumeTimeline();
    }

    private void continueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            dialogueText.text = currentStory.Continue();
            // display choices, if any, for this dialogue line
            displayChoices();
            // handle tags
            handleTags(currentStory.currentTags);
        }
        else
        {
            StartCoroutine(exitDialogueMode());
        }
    }

    private void handleTags(List<string> currentTags)
    {
        foreach (string tag in currentTags)
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
                    portraitAnimator.Play(tagValue);
                    break;

                default:
                    Debug.LogWarning("Tag came in but is not currently handled: " + tag);
                    break;
            }
        }
    }

    private void displayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        // defensive check to ensure our UI can support the number of choices coming in
        if (currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: " + currentChoices);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue
        foreach (Choice choice in currentChoices)
        {
            choices[index].SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }

        // go through remaining choices the UI supports and ensure they're hidden
        for (int i = index; i < choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }

        StartCoroutine(selectFirstChoice());

    }

    private IEnumerator selectFirstChoice()
    {
        // Event system requires we clear it first, then wait
        // for at least one frame before we set the current selected object
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void makeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
    }

    public Ink.Runtime.Object getVarState(string varName)
    {
        Ink.Runtime.Object varValue = null;
        dialogueVariables.variables.TryGetValue(varName, out varValue);
        if (varValue == null)
        {
            Debug.LogWarning("Ink variable was found to be null" + varValue);
        }
        return varValue;
    }
}
