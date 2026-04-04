using UnityEngine;

public class dialogueTrigger : MonoBehaviour
{
    [SerializeField] private string dialogueKnotName;
    [SerializeField] private string requiredQuestID;
    private bool hasTriggered;
    private questState currentQuestState;

    void Start()
    {
        quest quest = questManager.instance.getQuestById(requiredQuestID);
        if (quest != null) currentQuestState = quest.state;

        if (currentQuestState.Equals(questState.FINISHED)) Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && !hasTriggered)
        {
            gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
            hasTriggered = true;
        }
    }
}
