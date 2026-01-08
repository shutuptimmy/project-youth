using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rewardManager : MonoBehaviour, IDataPersistence
{
    private List<string> unlockedRewardIds = new List<string>();


    private void OnEnable()
    {
        gameEventsManager.instance.miscEvents.onQuestReward += UnlockReward;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.miscEvents.onQuestReward -= UnlockReward;
    }
    private void UnlockReward(string id)
    {
        if (!unlockedRewardIds.Contains(id))
        {
            unlockedRewardIds.Add(id);
            Debug.Log($"Reward Unlocked: {id}");

            // auto save
            dataPersistenceManager.instance.saveGame();
        }
    }

    // Public checker for objects like questPoint or lessonPaper
    public bool IsRewardUnlocked(string id)
    {
        return unlockedRewardIds.Contains(id);
    }

    public void loadData(gameData data)
    {
        this.unlockedRewardIds = data.unlockedRewardIds;
    }

    public void saveData(gameData data)
    {
        data.unlockedRewardIds = this.unlockedRewardIds;
    }


}
