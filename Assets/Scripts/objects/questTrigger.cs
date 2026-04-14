using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questTrigger : MonoBehaviour, IDataPersistence
{
    [SerializeField] private questInfoSO questInfo;
    [SerializeField] private string rewardID;
    private questState currentQuestState;
    private string questId;
    private bool isQuestComplete;

    void Awake()
    {
        if (questInfo != null)
        {
            questId = questInfo.id;
        }
    }

    private void OnEnable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && currentQuestState.Equals(questState.CAN_START))
        {
            gameEventsManager.instance.questEvents.startQuest(questId);
        }
    }

    void questStateChange(quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log("quest id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    public void loadData(gameData data)
    {
        isQuestComplete = data.unlockedRewardIds.Contains(rewardID);
        bool hasRead = data.unlockedRewardIds.Contains(rewardID + "_READ");

        if (!isQuestComplete || hasRead) Destroy(this.gameObject);
        else gameEventsManager.instance.miscEvents.questReward(rewardID + "_READ");
    }

    public void saveData(gameData data)
    {
    }
}
