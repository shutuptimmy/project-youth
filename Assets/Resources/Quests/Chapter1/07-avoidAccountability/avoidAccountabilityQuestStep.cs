using UnityEngine;

public class avoidAccountabilityQuestStep : questStep
{
    public void playerWon()
    {
        Debug.Log("executing");
        finishQuestStep();
    }
}
