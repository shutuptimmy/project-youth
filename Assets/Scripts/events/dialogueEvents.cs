using System;
using System.Collections;
using System.Collections.Generic;
using Ink.Runtime;
using UnityEngine;

public class dialogueEvents
{
    public event Action<string> onEnterDialogue;
    public void enterDialogue(string knotName)
    {
        onEnterDialogue?.Invoke(knotName);
    }

    public event Action onDialogueStarted;
    public void DialogueStarted()
    {
        onDialogueStarted?.Invoke();
    }

    public event Action onDialogueFinished;
    public void DialogueFinished()
    {
        onDialogueFinished?.Invoke();
    }

    public event Action<string, List<Choice>, List<string>> onDisplayDialogue;
    public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices, List<string> tags)
    {
        if (onDisplayDialogue != null)
        {
            onDisplayDialogue(dialogueLine, dialogueChoices, tags);
        }
    }

    public event Action<int> onMakeChoice;
    public void MakeChoice(int choiceIndex)
    {
        onMakeChoice?.Invoke(choiceIndex);
    }

    public event Action<int> onUpdateChoiceIndex;
    public void UpdateChoiceIndex(int choiceIndex)
    {
        onUpdateChoiceIndex?.Invoke(choiceIndex);
    }

    public event Action<string, Ink.Runtime.Object> onUpdateInkDialogueVariable;
    public void UpdateInkDialogueVariable(string name, Ink.Runtime.Object value)
    {
        if (onUpdateInkDialogueVariable != null)
        {
            onUpdateInkDialogueVariable(name, value);
        }
    }
}
