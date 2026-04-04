using UnityEngine;

public class tugOfWarQuestStep : questStep
{
    public void playerWon()
    {
        Debug.Log("executing");
        finishQuestStep();
    }

}
