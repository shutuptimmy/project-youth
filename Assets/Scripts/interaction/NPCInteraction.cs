using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class NPCInteraction : InteractableBase, IDataPersistence
{
    [Header("Components")]
    [SerializeField] private string npcId;
    private SpriteRenderer sprite;
    private CircleCollider2D circleCollider;

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    [Header("Quest")]
    [SerializeField] private questInfoSO questInfoForPoint;
    [SerializeField] private questIcon questIcon;
    [SerializeField] private questInfoSO aftermathQuest;
    private string questId;
    private string aftermathQuestId;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private questState currentQuestState;

    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        // Safety Check. Not all NPCs have quests.
        if (questInfoForPoint != null) questId = questInfoForPoint.id;
        if (aftermathQuest != null) aftermathQuestId = aftermathQuest.id;
    }

    private void Reset()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = true;
        circleCollider.radius = .25f;
    }
    public override void Interact()
    {
        if (!dialogueKnotName.Equals(""))
        {
            gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
        }
    }

    void SetVisualsActive(bool isActive)
    {
        // Safety Check (Lazy Loading)
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        if (circleCollider == null) circleCollider = GetComponent<CircleCollider2D>();

        sprite.enabled = isActive;
        circleCollider.enabled = isActive;

        // If the NPC is invisible, the Quest Icon must also be invisible.
        if (questIcon != null)
        {
            questIcon.gameObject.SetActive(isActive);
            if (isActive)
            {
                questIcon.setState(currentQuestState, startPoint, finishPoint);
            }
        }
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

    void questStateChange(quest quest)
    {
        // only update the quest state if this point has the corresponding quest
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.setState(currentQuestState, startPoint, finishPoint);
            Debug.Log("quest id: " + questId + " updated to state: " + currentQuestState);
        }

        if (quest.info.id.Equals(aftermathQuestId) && quest.state.Equals(questState.IN_PROGRESS))
        {
            questIcon.gameObject.SetActive(false);
        }
    }

    void rewardUnlocked(string id)
    {
        // show if id is present during gameplay
        if (id == npcId)
        {
            Debug.Log($"Activatin {npcId}");
            SetVisualsActive(true);
        }
    }

    public void loadData(gameData data)
    {
        // visibility check
        if (!string.IsNullOrEmpty(npcId))
        {
            bool isUnlocked = data.unlockedRewardIds.Contains(npcId);
            SetVisualsActive(isUnlocked);
        }
        // if ordinary NPC, always visible
        else SetVisualsActive(true);

        // quest status check
        if (!string.IsNullOrEmpty(questId))
        {
            quest quest = questManager.instance.getQuestById(questId);
            quest aftermathQuest = questManager.instance.getQuestById(aftermathQuestId);
            Debug.Log($"aftermath quest state: {aftermathQuest}");

            // skip if quest not present
            if (quest != null)
            {
                currentQuestState = quest.state;

                if (questIcon != null)
                {
                    // Force the icon to update right now
                    questIcon.setState(currentQuestState, startPoint, finishPoint);
                }
            }

            if (aftermathQuest != null && aftermathQuest.state == questState.IN_PROGRESS)
            {
                if (questIcon != null)
                {
                    questIcon.gameObject.SetActive(false);                
                }
            }
        }
    }

    public void saveData(gameData data)
    {
    }
}
