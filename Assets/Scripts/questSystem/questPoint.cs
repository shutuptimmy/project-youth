using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
public class questPoint : MonoBehaviour, IDataPersistence
{
    [Header("Reward Id")]
    [SerializeField] private string requiredRewardId;

    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;


    [Header("Quest")]
    [SerializeField] private questInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool isPlayerNear = false;

    private string questId;
    private questState currentQuestState;
    private questIcon questIcon;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider2D;
    private Transform childVisuals;


    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<questIcon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider2D = GetComponent<CircleCollider2D>();

        if (questIcon != null) childVisuals = questIcon.transform;

        // If a reward ID is required, hide immediately to prevent flickering
        if (!string.IsNullOrEmpty(requiredRewardId))
        {
            SetVisualsActive(false);
        }
    }

    void SetVisualsActive(bool isActive)
    {
        if (spriteRenderer != null) spriteRenderer.enabled = isActive;
        if (circleCollider2D != null) circleCollider2D.enabled = isActive;
        if (childVisuals != null) childVisuals.gameObject.SetActive(isActive);
    }

    private void submitPressed(inputEventContext inputEventContext)
    {
        if (!isPlayerNear || !inputEventContext.Equals(inputEventContext.DEFAULT))
        {
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            gameEventsManager.instance.dialogueEvents.enterDialogue(dialogueKnotName);
        }
        else
        {
            if (currentQuestState.Equals(questState.CAN_START) && startPoint)
            {
                gameEventsManager.instance.questEvents.startQuest(questId);
            }
            else if (currentQuestState.Equals(questState.CAN_FINISH) && finishPoint)
            {
                gameEventsManager.instance.questEvents.finishQuest(questId);
            }

        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }

    private void OnEnable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange += questStateChange;
        gameEventsManager.instance.inputEvents.onSubmitPressed += submitPressed;
        gameEventsManager.instance.miscEvents.onQuestReward += rewardUnlocked;
    }

    private void OnDisable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
        gameEventsManager.instance.inputEvents.onSubmitPressed -= submitPressed;
        gameEventsManager.instance.miscEvents.onQuestReward -= rewardUnlocked;
    }

    private void questStateChange(quest quest)
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
        // If the unlocked ID matches MY required ID, show myself!
        if (id == requiredRewardId)
        {
            SetVisualsActive(true);
        }
    }

    public void loadData(gameData data)
    {
        if (!string.IsNullOrEmpty(requiredRewardId))
        {
            bool isUnlocked = data.unlockedRewardIds.Contains(requiredRewardId);
            SetVisualsActive(isUnlocked);
        }
    }

    public void saveData(gameData data)
    {
        // nothin here
    }
}
