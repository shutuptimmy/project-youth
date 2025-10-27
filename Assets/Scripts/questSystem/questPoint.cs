using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class questPoint : MonoBehaviour
{
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


    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<questIcon>();
    }


    // private void Update()
    // {
    //     // if (isPlayerNear && inputManager.GetInstance().GetSubmitPressed())
    //     // {
    //     //     // gameEventsManager.instance.questEvents.advanceQuest(questId);
    //     // }
    //     // else
    //     // {
    //     //     return;
    //     // }
    //     // if (inputManager.GetInstance().GetSubmitPressed())
    //     // {
    //     //     submitPressed();
    //     // }


    // }

    private void submitPressed(inputEventContext inputEventContext)
    {
        if (!isPlayerNear || !inputEventContext.Equals(inputEventContext.DEFAULT))
        {
            return;
        }
        Debug.Log("PLayer detected");
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
    }

    private void OnDisable()
    {
        gameEventsManager.instance.questEvents.onQuestStateChange -= questStateChange;
        gameEventsManager.instance.inputEvents.onSubmitPressed -= submitPressed;
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
}
