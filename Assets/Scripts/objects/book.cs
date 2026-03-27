using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class book : InteractableBase, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private string rewardId;
    private SpriteRenderer sprite;
    private BoxCollider2D boxCollider;

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    [Header("Quest")]
    [SerializeField] private questInfoSO questInfoForPoint;

    private string questId;
    private questState currentQuestState;

    void Start()
    {
        questId = questInfoForPoint.id;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
        gameEventsManager.instance.miscEvents.onQuestReward += rewardUnlocked;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
        gameEventsManager.instance.miscEvents.onQuestReward -= rewardUnlocked;
    }

    public override void Interact()
    {
        gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
    }

    void SetVisualsActive(bool isActive)
    {
        // Safety Check (Lazy Loading)
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        if (boxCollider == null) boxCollider = GetComponent<BoxCollider2D>();

        sprite.enabled = isActive;
        boxCollider.enabled = isActive;
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

    void rewardUnlocked(string id)
    {
        // show if id is present during gameplay
        if (id == rewardId)
        {
            SetVisualsActive(true);
        }
    }

    public void loadData(gameData data)
    {
        if (!string.IsNullOrEmpty(rewardId))
        {
            bool isUnlocked = data.unlockedRewardIds.Contains(rewardId);
            SetVisualsActive(isUnlocked);
        }
    }

    public void saveData(gameData data) { }
}
