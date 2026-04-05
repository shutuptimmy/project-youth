using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]

public class computer : InteractableBase
{

    [Header("Components")]
    [SerializeField] private GameObject contentUIParent;
    [SerializeField] private string requiredQuestID;

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    private questState currentQuestState;

    public override void Interact()
    {
        // if (currentQuestState != questState.FINISHED) gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
        // else ActivateMenu();
        ActivateMenu();
    }

    void Start()
    {
        contentUIParent.SetActive(false);

        quest quest = questManager.instance.getQuestById(requiredQuestID);
        if (quest != null) currentQuestState = quest.state;

    }

    void ActivateMenu()
    {
        contentUIParent.SetActive(true);
    }

    public void DeactivateMenu()
    {
        contentUIParent.SetActive(false);
    }
}
