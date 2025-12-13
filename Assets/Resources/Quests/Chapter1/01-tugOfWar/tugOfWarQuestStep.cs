using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tugOfWarQuestStep : questStep
{
    public void playerWon()
    {
        Debug.Log("executing");
        finishQuestStep();
    }

}
