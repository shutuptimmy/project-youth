using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class questStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;
    private int stepIndex;

    public void initializeQuestStep(string questId, int stepIndex, string questStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;

        // if (questStepState != null && questStepState != "")
        // {
        //     setQuestStepState(questStepState);
        // }
    }

    protected void finishQuestStep(bool isQuestCompleted)
    {
        if (!isFinished && isQuestCompleted)
        {
            isFinished = true;

            gameEventsManager.instance.questEvents.advanceQuest(questId);
            Destroy(this.gameObject);
        }
        else
        {
            gameEventsManager.instance.questEvents.revertQuest(questId);
            Destroy(this.gameObject);
        }
    }

    protected void changeState(string newState, string newStatus)
    {
        gameEventsManager.instance.questEvents.questStepStateChange(questId, stepIndex, new questStepState(newState, newStatus));
    }

    // protected abstract void setQuestStepState(string state);

}
