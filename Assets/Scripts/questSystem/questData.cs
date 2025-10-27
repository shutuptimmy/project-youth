using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class questData
{
    public questState state;
    public int questStepIndex;
    public questStepState[] questStepStates;

    public questData(questState state, int questStepIndex, questStepState[] questStepStates)
    {
        this.state = state;
        this.questStepIndex = questStepIndex;
        this.questStepStates = questStepStates;
    }
}
