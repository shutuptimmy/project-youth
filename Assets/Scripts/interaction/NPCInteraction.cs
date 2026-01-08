using System.Collections;
using System.Collections.Generic;
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

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private string questId;
    private questState currentQuestState;
    private questIcon questIcon;
    private Transform childVisuals;


    private void Start()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<questIcon>();

        if (questIcon != null) childVisuals = questIcon.transform;

        // If a reward ID is required, hide immediately to prevent flickering
        // if (!string.IsNullOrEmpty(npcId))
        // {
        //     SetVisualsActive(false);
        // }
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
        // else
        // {
        //     if (currentQuestState.Equals(questState.CAN_START) && startPoint)
        //     {
        //         gameEventsManager.instance.questEvents.startQuest(questId);
        //     }
        //     else if (currentQuestState.Equals(questState.CAN_FINISH) && finishPoint)
        //     {
        //         gameEventsManager.instance.questEvents.finishQuest(questId);
        //     }
        // }
    }

    void SetVisualsActive(bool isActive)
    {
        // Safety Check (Lazy Loading)
        if (sprite == null) sprite = GetComponent<SpriteRenderer>();
        if (circleCollider == null) circleCollider = GetComponent<CircleCollider2D>();

        sprite.enabled = isActive;
        circleCollider.enabled = isActive;
        childVisuals.gameObject.SetActive(isActive);
    }

    void OnEnable()
    {
        base.OnEnable();

        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
        gameEventsManager.instance.miscEvents.onQuestReward += rewardUnlocked;
    }

    void OnDisable()
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
    }

    void rewardUnlocked(string id)
    {
        // show if id is present during gameplay
        if (id == npcId)
        {
            SetVisualsActive(true);
        }
    }

    public void loadData(gameData data)
    {
        if (!string.IsNullOrEmpty(npcId))
        {
            bool isUnlocked = data.unlockedRewardIds.Contains(npcId);
            SetVisualsActive(isUnlocked);
        }
    }

    public void saveData(gameData data)
    {
    }
}
