using UnityEngine;

public class moveTheBoxQuestStep : questStep
{
    [SerializeField] private boxPuzzleManager boxPuzzleCat;

    public void puzzleCompleted(string id)
    {
        if (!boxPuzzleCat.isPuzzleFinished)
        {
            return;
        }
        gameEventsManager.instance.miscEvents.questReward(id);
        gameEventsManager.instance.miscEvents.questReward("prologue");
        gameEventsManager.instance.dialogueEvents.enterDialogue("prologueBoxCar");
        finishQuestStep(true);
    }
}
