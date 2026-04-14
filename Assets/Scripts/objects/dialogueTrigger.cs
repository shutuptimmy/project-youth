using UnityEngine;

public class dialogueTrigger : MonoBehaviour, IDataPersistence
{
    [SerializeField] private string dialogueKnotName;
    [SerializeField] private string requiredQuestID;
    private bool hasTriggered;

    private void OnEnable() 
    {
        gameEventsManager.instance.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable() 
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
    }

    void QuestStateChange(quest quest)
    {
        // If the specific quest is finished, remove the trigger immediately
        if (quest.info.id == requiredQuestID && quest.state == questState.FINISHED)
        {
            Destroy(gameObject);
        }
    }

    // void Start()
    // {
    //     quest quest = questManager.instance.getQuestById(requiredQuestID);
    //     if (quest != null) currentQuestState = quest.state;

    //     if (currentQuestState.Equals(questState.FINISHED)) Destroy(gameObject);
    // }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !hasTriggered)
        {
            gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
            hasTriggered = true;
        }
    }

    public void loadData(gameData data)
    {
        bool isQuestComplete = data.unlockedRewardIds.Contains(requiredQuestID);
        if (isQuestComplete) Destroy(this.gameObject);
    }

    public void saveData(gameData data)
    {
    }
}
