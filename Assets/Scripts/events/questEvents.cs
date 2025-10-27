using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questEvents
{
    public event Action<string> onStartQuest;
    public void startQuest(string id)
    {
        onStartQuest?.Invoke(id);
    }

    public event Action<string> onAdvanceQuest;
    public void advanceQuest(string id)
    {
        onAdvanceQuest?.Invoke(id);
    }

    public event Action<string> onFinishQuest;
    public void finishQuest(string id)
    {
        onFinishQuest?.Invoke(id);
    }

    public event Action<quest> onQuestStateChange;
    public void questStateChange(quest quest)
    {
        onQuestStateChange?.Invoke(quest);
    }

    public event Action<string, int, questStepState> onQuestStepStateChange;
    public void questStepStateChange(string id, int stepIndex, questStepState questStepState)
    {
        onQuestStepStateChange?.Invoke(id, stepIndex, questStepState);
    }
}
