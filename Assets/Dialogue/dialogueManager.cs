using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class dialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJSON;
    private Story story;
    private bool isDialoguePlayin = false;
    private int currentChoiceIndex = -1;
    private inkExternalFunctions inkExternalFunctions;
    private inkDialogueVariables inkDialogueVariables;

    private void Awake()
    {
        story = new Story(inkJSON.text);
        inkExternalFunctions = new inkExternalFunctions();
        inkExternalFunctions.bind(story);
        inkDialogueVariables = new inkDialogueVariables(story);
    }

    private void OnDestroy()
    {
        inkExternalFunctions.unbind(story);
    }

    private void OnEnable()
    {
        gameEventsManager.instance.dialogueEvents.onEnterDialogue += enterDialogue;
        gameEventsManager.instance.inputEvents.onSubmitPressed += submitPressed;
        gameEventsManager.instance.dialogueEvents.onUpdateChoiceIndex += updateChoiceIndex;
        // gameEventsManager.instance.dialogueEvents.onUpdateInkDialogueVariable += updateInkDialogueVar;
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.dialogueEvents.onEnterDialogue -= enterDialogue;
        gameEventsManager.instance.inputEvents.onSubmitPressed -= submitPressed;
        gameEventsManager.instance.dialogueEvents.onUpdateChoiceIndex -= updateChoiceIndex;
        // gameEventsManager.instance.dialogueEvents.onUpdateInkDialogueVariable -= updateInkDialogueVar;
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
    }

    private void questStateChange(quest quest)
    {
        inkDialogueVariables.updateVarsState(quest.info.id + "State", new StringValue(quest.state.ToString()));
        // gameEventsManager.instance.dialogueEvents.UpdateInkDialogueVariable(quest.info.id + "State", new StringValue(quest.state.ToString()));
    }

    // updating ink var alternative
    // private void updateInkDialogueVar(string name, Ink.Runtime.Object value)
    // {
    //     inkDialogueVariables.updateVarsState(name, value);
    // }

    private void updateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }


    private void submitPressed(inputEventContext inputEventContext)
    {
        if (!inputEventContext.Equals(inputEventContext.DIALOGUE))
        {
            return;
        }

        continueOrExitStory();
    }

    public void enterDialogue(string knotName)
    {
        if (isDialoguePlayin)
        {
            return;
        }

        isDialoguePlayin = true;
        gameEventsManager.instance.dialogueEvents.DialogueStarted();
        gameEventsManager.instance.playerEvents.DisablePlayerMovement();
        gameEventsManager.instance.inputEvents.ChangeInputEventContext(inputEventContext.DIALOGUE);

        Debug.Log("Entering dialogue knot name: " + knotName);

        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("knot name string is empty when entering dialogue");
        }

        // start listening for variables
        inkDialogueVariables.syncVarsAndStartListening(story);

        continueOrExitStory();
    }

    private void continueOrExitStory()
    {
        // make a choice if available
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            // reset choice index for next time
            currentChoiceIndex = -1;
        }

        if (story.canContinue)
        {
            string dialogueLine = story.Continue();

            // handleTags(story.currentTags);

            while (isLineBlank(dialogueLine) && story.canContinue)
            {
                dialogueLine = story.Continue();
            }

            if (isLineBlank(dialogueLine) && !story.canContinue)
            {
                exitDialogue();
            }
            else
            {
                gameEventsManager.instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices, story.currentTags);

            }
        }
        else if (story.currentChoices.Count == 0)
        {
            exitDialogue();
        }
    }


    private void exitDialogue()
    {
        Debug.Log("Exiting dialogue");

        isDialoguePlayin = false;
        gameEventsManager.instance.dialogueEvents.DialogueFinished();
        gameEventsManager.instance.inputEvents.ChangeInputEventContext(inputEventContext.DEFAULT);
        gameEventsManager.instance.playerEvents.EnablePlayerMovement();

        // stop listening
        inkDialogueVariables.stopListening(story);

        // reset story state
        story.ResetState();

    }

    private bool isLineBlank(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }
}
