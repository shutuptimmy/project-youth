using UnityEngine;

public class gravitationalResearchQuestStep : questStep
{
    public void playerWon()
    {
        Debug.Log("executing");
        finishQuestStep();
    }
}
