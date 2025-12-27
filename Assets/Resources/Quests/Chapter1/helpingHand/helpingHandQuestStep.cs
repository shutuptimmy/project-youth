using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helpingHandQuestStep : questStep
{


    public void playerWon()
    {
        Debug.Log("executing");
        finishQuestStep();
    }
}
