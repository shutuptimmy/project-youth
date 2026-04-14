using System;
using System.Collections;
using UnityEngine;

public class aftermathQuestStep : questStep, IDataPersistence
{
    [SerializeField] private Animator transition;
    [SerializeField] private string rewardId;


    void OnEnable()
    {
        gameEventsManager.instance.miscEvents.onQuestReward += rewardUnlocked;
        
    }

    void OnDisable()
    {
        gameEventsManager.instance.miscEvents.onQuestReward -= rewardUnlocked;
        
    }

    void rewardUnlocked(string id)
    {
        if (id == rewardId)
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        transition.Play("fadeOutToOrange");
    }

    public void loadData(gameData data)
    {
        bool isQuestComplete = data.unlockedRewardIds.Contains(rewardId);

        if (isQuestComplete) Destroy(this.gameObject);
    }

    public void saveData(gameData data)
    {
    }
}
